using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

/* 
 * Author: Ruidong Zhang
 */

namespace Sprint5
{
    public enum PowerType { Star, FireFlower, RedMushroom, OneUpMushroom }

    public class Mario : IPlayer
    {
        private static double DEATH_FREEZE_TIME = 1.0;    //Time that mario remains still before falling off screen after death

        /*
         * Public Members
         */
        public IPlayerHealth HState { get; set; }
        public IVulnerability VState { get; set; }
        public IXMove XMoveState { get; set; }
        public IYMove YMoveState { get; set; }
        public Vector2 Location { get; set; }

        /*
         * Private Members
         */
        private ISprite Sprite;
        private SpriteFactory spriteFactory;
        private bool faceRight;
        private bool hadStar, wasDamaged;   //Holds truth of if last sprite was for star or damaged mario
        public bool destroyObject { get; set; }
        public string mostRecentSprite;
        public bool processFlagAnimation;     //Used to determine if Mario has grabbed the flag (Used to determine if Mario's sprite should be that of grabbing the flag)
        private bool moveLocked;    //Used to determine if the player can move mario, false if player can move, true otherwise
        private double timeOfDamage;
        public bool gameEnded;

        /*
         * Default Constructor
         */
        public Mario(Vector2 location, GameTime gametime, bool moving, bool faceRight, bool jump, bool floored, SpriteFactory spriteFactory)
        {

            Location = location;
            HState = new SmallMario(this); 
            VState = new Damageable(this);
            XMoveState = new PlayerMoveRight(this, moving, faceRight, false);
            YMoveState = new PlayerNoJump(this, jump, false);
            this.spriteFactory = spriteFactory;

            this.faceRight = faceRight;
            hadStar = false;
            wasDamaged = false;
            destroyObject = false;
            processFlagAnimation = false;
            moveLocked = false;
            gameEnded = false;

            List<string> ykey = YMoveState.GetSpriteKey();
            List<string> xkey = XMoveState.GetSpriteKey();
            string currAction;
            if (ykey.Count == 1)
            {
                currAction = ykey[0];   //Mario is jumping as his action
            } else if (HState.GetSpriteKey().Count > 1)
            {
                currAction = HState.GetSpriteKey()[1];  //Mario is throwing as his action
            }
            else
            {
                currAction = xkey[0];   //Mario is crouching, moving, or still as his action
            }

            Sprite = spriteFactory.GetSprite(HState.GetSpriteKey()[0], XMoveState.GetSpriteKey()[0], currAction);
            mostRecentSprite = HState.GetSpriteKey()[0] + XMoveState.GetSpriteKey()[0] + currAction;

            Sprite.Location = Location;

        }

        // Used by the animation engine to assign Mario a certain sprite directly for proper animation processing
        public void AssignSprite(ISprite newSprite)
        {
            this.Sprite = newSprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public bool IsDrawn()
        {
            return Sprite.IsDrawn();
        }

        public void AnimationLock(bool lockTruth)
        {
            moveLocked = lockTruth;
        }

        public void Jump()
        {
            if (!HState.IsDead() && !moveLocked)
            {
                YMoveState.Jump();
            }
        }

        public void Bounce()
        {
            if (!HState.IsDead() && !moveLocked)
            {
                YMoveState.Bounce();
            }
        }

        public void Bounce(int maxHeight)
        {
            if (!HState.IsDead() && !moveLocked)
            {
                YMoveState.Bounce(maxHeight);
            }
        }

        public void Crouch()
        {
            if (!HState.IsDead() && !moveLocked) ((PlayerMoveX)XMoveState).Crouch(); 
        }
        
        //Mario move left or move right with the bool direction.
        public void Move(bool FaceRight)
        {
            if (!HState.IsDead() && !moveLocked) XMoveState.Movement(FaceRight);
        }

        //Mario takes damage in different states and changes its state.
        public void TakeDamage(GameTime gametime)
        {
            if (!HState.IsDead()) VState.TakeDamage(gametime);
            timeOfDamage = gametime.TotalGameTime.TotalSeconds;
        }

        //Mario picks up different powers and changes its state.
        public void PickupPower(PowerType power, GameTime gameTime)
        {
            if (!HState.IsDead()) HState.PickupPower( power, gameTime);
        }

        //Mario throws fire ball, changes its state, returns bool value.
        public void Fireball(GameTime gametime)
        {
            if (!HState.IsDead() && HState.CanThrow(gametime) && !moveLocked) {
                GameObjectManager.Instance.FireballList.Add(HState.ThrowFireBall(XMoveState.FaceRight(), Location, spriteFactory, gametime));
                SoundFactory.Instance.GetSoundEffect("ThrowFireball").Play();
            }
        }

        //Mario updates its state and gets/updates its sprite.
        public void Update(GameTime gametime)
        {
            if (!AnimationEngine.Instance.HaltSpriteChanges()) {
                string hKey = HState.GetSpriteKey()[0];
                string dirKey = XMoveState.GetSpriteKey()[0];
                List<string> ykey = YMoveState.GetSpriteKey();
                List<string> xkey = XMoveState.GetSpriteKey();
                string currAction;
                if (ykey.Count == 1)
                {
                    currAction = ykey[0];   //Mario is jumping as his action
                }
                else if (HState.GetSpriteKey().Count > 1)
                {
                    currAction = HState.GetSpriteKey()[1];  //Mario is throwing as his action
                }
                else if (!hKey.Contains("SmallMario"))
                {
                    currAction = xkey[1];   //Mario is crouching
                } else
                {
                    if (xkey.Count > 2)
                    {
                        currAction = xkey[2];   //Mario is moving, or still as his action
                    } else
                    {
                        currAction = xkey[1];   //Mario is moving, or still as his action, but was trying to crouch as Small Mario
                    }
                }

                if (!mostRecentSprite.Contains(hKey) || !mostRecentSprite.Contains(dirKey[0]) || !mostRecentSprite.Contains(currAction) || HState.HasStar() != hadStar || VState.IsDamaged() != wasDamaged) {
                    if (hKey.Contains("Dead")) {
                        Sprite = spriteFactory.GetSprite("DeadMario");
                    } else
                    {
                        Sprite = spriteFactory.GetSprite(hKey, dirKey, currAction);
                        if (HState.HasStar() != hadStar) hadStar = HState.HasStar();
                        if (VState.IsDamaged() != wasDamaged) wasDamaged = VState.IsDamaged();
                    }
                    mostRecentSprite = hKey + dirKey + currAction;
                }
            }

            HState.Update(gametime);
            VState.Update(gametime);

            if (!moveLocked || processFlagAnimation)
            {
                XMoveState.Update(gametime);
            }

            if (HState.IsDead() && gametime.TotalGameTime.TotalSeconds - timeOfDamage > DEATH_FREEZE_TIME)
            {
                YMoveState.Jump();
            }

            if (!moveLocked || (HState.IsDead() && gametime.TotalGameTime.TotalSeconds - timeOfDamage > DEATH_FREEZE_TIME))
            {
                YMoveState.Update(gametime);
            }

            if (HState.HasStar())
			{
                Sprite.ToggleStar();
			}

            if (VState.IsDamaged())
			{
                Sprite.ToggleDamaged();
			}

            if (Location.Y > 500 && !gameEnded)
            {
                SoundFactory.Instance.GetSoundEffect("GameOver").Play();
                ScreenManager.Instance.ToggleDrawing(ScreenType.GameOver);
                GameState.Instance.WorldState = WorldState.Dead;
                gameEnded = true;
            }

            Sprite.Location = Location;

            if (!AnimationEngine.Instance.HaltSpriteChanges()) Sprite.Update(gametime);

        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            if (isSolid && !HState.IsDead() && !moveLocked)
            {
                Location = Vector2.Add(Location, new Vector2(XMoveState.CollisionAdjustment(isRight, collisionOverlap), 0));
                Sprite.Location = Location;
            }
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            if (isSolid && !HState.IsDead() && (!moveLocked || processFlagAnimation))
            {
                Location = Vector2.Add(Location, new Vector2(0, YMoveState.CollisionAdjustment(isUp, collisionOverlap)));
                Sprite.Location = Location;
            }
                
        }

        public Rectangle GetHitBox()
        {
            return Sprite.GetHitBox();
        }

        public void DestroyObject()
        {
            destroyObject = true;
        }

        public void PlaceBlock(IPlayer.CardinalDir dir)
        {
            if (HUD.Instance.hasBlock()) {
                SoundFactory.Instance.GetSoundEffect("PlaceBlock").Play();
                bool isSmall = false;
                if (HState.GetSpriteKey()[0].Contains("Small")) isSmall = true;
                int xTileLoc = ((int)this.Location.X / Game1.BLOCK_WIDTH);
                int xTileOffset = ((int)this.Location.X % Game1.BLOCK_WIDTH); 
                int yTileLoc = ((int)this.Location.Y / Game1.BLOCK_HEIGHT); 
                int yTileOffset = ((int)this.Location.Y % Game1.BLOCK_HEIGHT); 

                if (xTileOffset > 8)
                {
                    xTileLoc++;
                }

                if (yTileOffset > 8)
                {
                    yTileLoc++;
                }

                switch (dir)
                {
                    case (IPlayer.CardinalDir.Down):
                        if (isSmall)
                        {
                            yTileLoc += 1;
                        } else
                        {
                            yTileLoc += 2;
                        }
                        break;
                    case (IPlayer.CardinalDir.Left):
                        xTileLoc -= 2;
                        if (!isSmall) yTileLoc += 1;
                        break;
                    case (IPlayer.CardinalDir.Right):
                        xTileLoc += 2;
                        if (!isSmall) yTileLoc += 1;
                        break;
                    default:
                        yTileLoc -= 1;
                        break;
                }

                Vector2 blockLoc = new Vector2(Game1.BLOCK_WIDTH * xTileLoc, Game1.BLOCK_HEIGHT * yTileLoc);  

                // Add new block in to GOM list.
                GameObjectManager.Instance.StructureList[xTileLoc].Add(new Block(blockLoc, SpriteFactory.Instance, BlockType.Brick, ItemType.None, 0));

                HUD.Instance.Placeblock();
            } else
            {
                SoundFactory.Instance.GetSoundEffect("NoBlocks").Play();
            }
        }
    }
}
