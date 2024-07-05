using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sprint5
{
	public class AnimationEngine : IUpdateable
	{
		private static double PIPE_ANIMATE_LEN = 1.0;    //Time for the animation of entering and leaving a pipe will take (seconds)
		private static int MAX_PIPE_MOVE_DIST = 32;     //The max number of pixels a pipe animation can move Mario, prevents Mario from being pushed extra far out of a pipe

		private static double MARIO_WAIT_AFTER_FLAG = 0.5;   //Time for Mario to wait after finishing lowering flag before detaching
		private static int DISTANCE_TO_FLAGPOLE_BOTTOM = 137;    //Number of pixels from Flag Y location to bottom of pole location for pennant
		private static int FLAGPOLE_MARIO_START_OFFSET = 12;   //Pixel difference between Mario's location and the flagpole location when initially grabbing pole

		//Used to define the number of points alotted based on where on the flag mario touches
		//First item in tuple is the lowest y-value of the section relative to the flag height, second item is the number of points alotted for the section
		private static List<List<int>> FLAG_POLE_SECTIONS = new List<List<int>>() { new List<int>() { 136, 100 }, new List<int>() { 96, 400 }, new List<int>() { 72, 800 }, new List<int>() { 26, 2000 }, new List<int>() { 0, 4000 } };

		private static int ITEM_SPAWN_MOVE = 1;    //Amount of pixels an item should move every update after spawning, specifically from a qblock in most cases
		private static int COIN_COLLECTION_MOVE = 2;    //Amount of pixels a coin will move when it gets collected

		private static double ANIMATION_UPDATE_INTERVAL = 0.0; //Time between location changes for animations (Used for slowing down animations)
		private static int TILE_DIMENSION = 16;  //Number of pixels wide/tall a single tile in Mario is (For reference, SmallMario is 16 pixels tall and wide)

		enum CurrentAnimation
		{
			None,
			EnteringPipe,
			ExitingPipe,
			SlideDownFlag,
			WaitOnFlag,
			WalkOffLevel,
			LevelOver
		}

		private Mario targetMario;
		private string targetMarioState;
		private int totalAnimationMove = 0; //Total number of pixels Mario has moved during an animation
		private Pipe targetPipe;
		private int xMoveToLoc, yMoveToLoc; //Used to hold specific x and y values a target object to should move during an animation
		private int nextArea;
		private Flag targetFlag;
		private Text flagScoreText;
		private Vector2 marioMoveDir;
		private CurrentAnimation currAnimation = CurrentAnimation.None;
		private bool animating = false;
		private double animationStart = 0.0;
		private double animationInterval = 0.0;

		private static AnimationEngine instance = new AnimationEngine();

		public static AnimationEngine Instance
		{
			get
			{
				return instance;
			}
		}

		private AnimationEngine() { }

		public bool FinishedAnimating()
		{
			return !animating;
		}

		public bool HaltSpriteChanges()
		{
			if (currAnimation == CurrentAnimation.None || currAnimation == CurrentAnimation.WalkOffLevel)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public void AnimateItemOnSpawn(Item item)
		{
			item.Location = Vector2.Add(item.Location, new Vector2(0, -ITEM_SPAWN_MOVE));
		}

		public void AnimateCollectedCoin(Item item)
		{
			item.Location = Vector2.Add(item.Location, new Vector2(0, -COIN_COLLECTION_MOVE));
		}

		private void MarioLocationChangePipe(bool goingIn, PipeType targetType)
		{
			int moveAmnt = 1;   //Distance Mario will travel while moving into and out of pipes every time his position is updated
			int initialLocMod = 1;  //Holds the factor of how far away, relative to the targetPipe.OutLoc, Mario will spawn
			if (targetMarioState.Contains("Small"))
			{
				initialLocMod = 2;
			}

			switch (targetType)
			{
				case (PipeType.None):   //In the case of the exit pipe actually not existing, then Mario shouldn't move and should go straight to the target location
					marioMoveDir = new Vector2(0, 0);
					xMoveToLoc = (int)targetPipe.OutLoc.X;
					yMoveToLoc = (int)targetPipe.OutLoc.Y;
					break;
				case (PipeType.LeftPipe):
					if (goingIn)
					{
						marioMoveDir = new Vector2(moveAmnt, 0);
					}
					else
					{
						xMoveToLoc = (int)(targetPipe.OutLoc.X + (TILE_DIMENSION * initialLocMod));
						yMoveToLoc = (int)targetPipe.OutLoc.Y;
						marioMoveDir = new Vector2(-moveAmnt, 0);
					}
					break;
				case (PipeType.RightPipe):
					if (goingIn)
					{
						marioMoveDir = new Vector2(-moveAmnt, 0);
					}
					else
					{
						xMoveToLoc = (int)(targetPipe.OutLoc.X - (TILE_DIMENSION * initialLocMod));
						yMoveToLoc = (int)targetPipe.OutLoc.Y;
						marioMoveDir = new Vector2(moveAmnt, 0);
					}
					break;
				default:    //Assume mario's interacting pipe is an Up-Facing pipe
					if (goingIn)
					{
						marioMoveDir = new Vector2(0, moveAmnt);
					}
					else
					{
						xMoveToLoc = (int)targetPipe.OutLoc.X;
						yMoveToLoc = (((int)targetPipe.OutLoc.Y) + (TILE_DIMENSION * initialLocMod));
						marioMoveDir = new Vector2(0, -moveAmnt);
					}
					break;
			}
		}

		// If leftFlagSide, then it will provide the info for mario on the left of the flag pole and to move items on screen
		// Otherwise it only provides the info needed to change mario to be on the right side of the flag pole
		private void MarioUpdatesFlagAnimation(bool leftFlagSide)
		{
			if (leftFlagSide)
			{
				marioMoveDir = new Vector2(0, 1);   //No matter what state Mario is in, he should move down the flag pole the same amount
				switch (targetMarioState)
				{
					case ("FireMario"):
						targetMario.AssignSprite(SpriteFactory.Instance.GetSprite("FireMario", "Right", "Climb"));
						break;
					case ("BigMario"):
						targetMario.AssignSprite(SpriteFactory.Instance.GetSprite("BigMario", "Right", "Climb"));
						break;
					default:
						targetMario.AssignSprite(SpriteFactory.Instance.GetSprite("SmallMario", "Right", "Climb"));
						break;
				}
			}
			else
			{
				switch (targetMarioState)
				{
					case ("FireMario"):
						targetMario.AssignSprite(SpriteFactory.Instance.GetSprite("FireMario", "Left", "Climb"));
						break;
					case ("BigMario"):
						targetMario.AssignSprite(SpriteFactory.Instance.GetSprite("BigMario", "Left", "Climb"));
						break;
					default:
						targetMario.AssignSprite(SpriteFactory.Instance.GetSprite("SmallMario", "Left", "Climb"));
						break;
				}
			}
			if (targetFlag.Location.Y > targetMario.Location.Y)
			{
				targetMario.Location = new Vector2(targetFlag.Location.X, targetFlag.Location.Y + TILE_DIMENSION);
			}
			targetMario.mostRecentSprite = "";
		}

		public void EnterPipe(GameTime gametime, Mario mario, Pipe pipe, int nextArea)
		{
			GameState.Instance.WorldState = WorldState.PlayerInteraction;
			mario.AnimationLock(true);
			animationStart = gametime.TotalGameTime.TotalSeconds;
			currAnimation = CurrentAnimation.EnteringPipe;
			animating = true;
			targetMario = mario;
			targetMarioState = mario.HState.GetSpriteKey()[0];
			targetPipe = pipe;
			this.nextArea = nextArea;
			MarioLocationChangePipe(true, pipe.pipeType);
		}


		private int FlagScore()
		{
			int poleTouchLoc = (int)(targetMario.Location.Y - targetFlag.Location.Y);
			int poleRegion = 0;
			int regionLoc = FLAG_POLE_SECTIONS[poleRegion][0];  //Get the first region to check for
			while (poleRegion < FLAG_POLE_SECTIONS.Count)
			{ //Keep the game from checking a non-existent index in the List of sections on the pole
				if (poleTouchLoc >= regionLoc)
				{
					return FLAG_POLE_SECTIONS[poleRegion][1];   //Get the score of the region landed on
				}
				else
				{
					poleRegion++;
					if (poleRegion < FLAG_POLE_SECTIONS.Count) regionLoc = FLAG_POLE_SECTIONS[poleRegion][0];  //Get the next region to check for
				}
			}
			return 5000;	//If no other regions we found to be touched, then the top of the pole must have been touched and therefore the score is 5000 points
		}

		public void GrabFlag(GameTime gametime, Mario mario, Flag flag)
		{
			GameState.Instance.WorldState = WorldState.PlayerInteraction;
			mario.AnimationLock(true);
			mario.XMoveState = new PlayerNoMoveRight(mario, false, true, false);
			mario.processFlagAnimation = true;
			animationStart = gametime.TotalGameTime.TotalSeconds;
			currAnimation = CurrentAnimation.SlideDownFlag;
			animating = true;
			targetMario = mario;
			targetMarioState = mario.HState.GetSpriteKey()[0];
			targetFlag = flag;
			int flagScore = FlagScore();
			MarioUpdatesFlagAnimation(true);
			HUD.Instance.ReachFlag(flagScore);
			flagScoreText = TextFactory.Instance.CreateUntimedText(flagScore.ToString(), new Vector2(flag.Location.X + (Game1.BLOCK_WIDTH / 2), flag.Location.Y + DISTANCE_TO_FLAGPOLE_BOTTOM));
			MediaPlayer.Stop();
			SoundFactory.Instance.GetSoundEffect("Flagpole").Play();
			targetMario.Location = new Vector2(targetFlag.Location.X - FLAGPOLE_MARIO_START_OFFSET, targetMario.Location.Y);
		}

		public void MarioLeftLevel(Mario mario)
		{
			//mario.AnimationLock(false);
			//animating = false;
		}

		private bool DoAnimate(GameTime gametime)
		{
			if (ANIMATION_UPDATE_INTERVAL > animationInterval)
			{
				animationInterval += gametime.ElapsedGameTime.TotalSeconds;
				return false;
			}
			else
			{
				animationInterval = 0.0;
				return true;
			}
		}

		public void Update(GameTime gametime)
		{
			if (animating)
			{
				switch (currAnimation)
				{
					case (CurrentAnimation.EnteringPipe):
						if (gametime.TotalGameTime.TotalSeconds - animationStart < PIPE_ANIMATE_LEN)
						{
							if (DoAnimate(gametime) && (totalAnimationMove < MAX_PIPE_MOVE_DIST))
							{
								targetMario.Location = Vector2.Add(targetMario.Location, marioMoveDir);
								totalAnimationMove += (int)(marioMoveDir.X + marioMoveDir.Y);
							}
						}
						else
						{
							totalAnimationMove = 0;
							GameState.Instance.GoToLevel(nextArea);
							if (targetPipe.outPipeType != PipeType.None) SoundFactory.Instance.GetSoundEffect("PipeSound").Play();
							currAnimation = CurrentAnimation.ExitingPipe;
							MarioLocationChangePipe(false, targetPipe.outPipeType);
							targetMario.Location = new Vector2(xMoveToLoc, yMoveToLoc);
							animationStart = gametime.TotalGameTime.TotalSeconds;
						}
						break;
					case (CurrentAnimation.ExitingPipe):
						if ((gametime.TotalGameTime.TotalSeconds - animationStart < PIPE_ANIMATE_LEN) && (targetPipe.outPipeType != PipeType.None))
						{
							if (DoAnimate(gametime) && (totalAnimationMove < MAX_PIPE_MOVE_DIST))
							{
								targetMario.Location = Vector2.Add(targetMario.Location, marioMoveDir);
								totalAnimationMove += Math.Abs((int)(marioMoveDir.X + marioMoveDir.Y));
							}
						}
						else
						{
							totalAnimationMove = 0;
							targetMario.AnimationLock(false);
							GameState.Instance.WorldState = WorldState.Playing;
							currAnimation = CurrentAnimation.None;
							animating = false;
						}
						break;
					case (CurrentAnimation.SlideDownFlag):
						if (targetFlag.Pennant.Location.Y < (targetFlag.Location.Y + DISTANCE_TO_FLAGPOLE_BOTTOM))
						{
							targetMario.Location = Vector2.Add(targetMario.Location, marioMoveDir);
							targetFlag.Pennant.Location = Vector2.Add(targetFlag.Pennant.Location, new Vector2(0, 1));
							flagScoreText.Location = Vector2.Add(flagScoreText.Location, new Vector2(0, -1));
						}
						else
						{
							currAnimation = CurrentAnimation.WaitOnFlag;
							MarioUpdatesFlagAnimation(false);
							targetMario.Location = new Vector2((targetMario.Location.X + TILE_DIMENSION), targetMario.Location.Y);
							animationStart = gametime.TotalGameTime.TotalSeconds;
							SoundFactory.Instance.GetSoundEffect("ClearingLevel").Play();
						}
						break;
					case (CurrentAnimation.WaitOnFlag):
						if (!(gametime.TotalGameTime.TotalSeconds - animationStart < MARIO_WAIT_AFTER_FLAG))
						{
							currAnimation = CurrentAnimation.WalkOffLevel;

						}
						break;
					case (CurrentAnimation.WalkOffLevel):
						if (targetMario.Location.X >= GameState.Instance.CastleX) 
						{
							currAnimation = CurrentAnimation.LevelOver;
						}

						targetMario.XMoveState.Movement(true);
						targetMario.YMoveState.Update(gametime);
						break;
					case (CurrentAnimation.LevelOver):
						HUD.Instance.Transfer();
						
						if (Int32.Parse(HUD.Instance.getValue("Time")) <= 0) 
						{
							System.Threading.Thread.Sleep(new TimeSpan(0, 0, 3));
							targetMario.AnimationLock(false);
							currAnimation = CurrentAnimation.None;
							animating = false;
							GameState.Instance.WorldState = WorldState.Playing;
							//Go to the next level
							GameState.Instance.NextLevel();
							HUD.Instance.DeathReset();
							//ScreenManager.Instance.ToggleDrawing(ScreenType.LevelLoadScreen); TODO: Add level startup screen, currently not working
							GameState.Instance.WorldState = WorldState.PreLevel;
						}

						break;
					default:
						break;
				}
			}
		}
	}
}
