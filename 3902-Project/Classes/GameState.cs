using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

/*
 * Author: Ben Borszcz, Tommy Kasarcik
 */

namespace Sprint5
{
	/* 
     * Enum to control which game objects should update, based on the current state
     */
	public enum WorldState
	{
		Playing,
		Paused,
		Dead,
		PlayerInteraction,
		PreLevel
	}

	/*
     * Singleton class to store various attributes about the game.
     * 
     * NOTE: Various many methods here have been commented out; not sure if they are necessary
     */
	public class GameState : IUpdateable
	{
		/*
         * Public Members
         */
		public GameTime GameTime { get; set; }
		public WorldState WorldState { get; set; }
		public string CurrentLevelName { get; set; }
		public Song BackgroundSong { get; set; }
		public bool ExitingPipe { get; set; }
		public int CastleX { get; set; }

		/*
         * Private Members
         */
		private int numLevels;
		private int currLevel;
		private IKeyboardController keyboard;
		private IMouseController mouse;

		/*
         * Private instance of <c> this </c>
         */
		private static GameState instance = new GameState();

		/*
         * Public instance of <c> this </c>
         */
		public static GameState Instance
		{
			get
			{
				return instance;
			}
		}

		/*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
		private GameState()
		{
		}

		/*
         * Public Methods
         */

		/*
         * Initialize <c> this </c>
         */
		public void Initialize(int numLevels, int initLevel)
		{
			this.numLevels = numLevels;
			currLevel = initLevel;
			WorldState = WorldState.Playing;
		}

		/*
         * Add a new keyboard to <c> this </c>
         */
		public void AddKeyboard(IKeyboardController keyboard)
		{
			this.keyboard = keyboard;
		}

		/*
         * Add a new mouse to <c> this </c>
         */
		public void AddMouse(IMouseController mouse)
		{
			this.mouse = mouse;
		}
		/* 
         * Returnes the current level
         */
		public int GetLevel()
		{
			return currLevel;
		}

		/* 
         * Increases the level by one
         * @requires: 
         * @param: 
         * @ensures: Level = Level + 1
         * @returns: 
         */
		public void NextLevel()
		{
			// Increment the current level
			currLevel++;

			// For now, change back to first level after leaving last level, may change in future
			if (currLevel > (numLevels + 1))
				currLevel = 1;

			GoToLevel(currLevel);
		}


		/* 
         * Sets the level to value
         * @requires: value > 0
         * @param: int value
         * @ensures: Level = value
         * @returns: 
         */
		public void GoToLevel(int level)
		{
			currLevel = level;

			// Remove current level objects from GameObjectManager
			GameObjectManager.Instance.ClearObjects();

			// Delete current camera
			CameraManager.Instance.DeleteCamera(GameObjectManager.Instance.EntityList[0][0]);

			// Load new level
			LevelLoader.Instance.LoadLevel(currLevel);
			CameraManager.Instance.CreateCamera(GameObjectManager.Instance.EntityList[0][0]);

			// Reset controllers
			keyboard.ResetCommandBindings();
			mouse.ResetCommandBindings();

			// Load new sprites for level to ScreenManager
			ScreenManager.Instance.LoadSprites();
		}


		/*
         * Update <c> this.GameTime </c>
         */
		public void Update(GameTime gameTime)
		{
			GameTime = gameTime;

			// Update the proper IGameObject lists based on the current world state
			switch (WorldState)
			{
				case WorldState.Playing:
					//Update all IGameObjects currently in GameObjectManager
					updateList(GameObjectManager.Instance.PlayerList);
					updateListRange(GameObjectManager.Instance.EnemyList);
					updateList(GameObjectManager.Instance.ItemList);
					updateList(GameObjectManager.Instance.FireballList);

					updateListList(GameObjectManager.Instance.StructureList);
					updateListList(GameObjectManager.Instance.BackgroundList);
					AnimationEngine.Instance.Update(gameTime);
					break;
				case WorldState.PlayerInteraction:
					// Update mario and blocks only
					updateList(GameObjectManager.Instance.PlayerList);
					updateListList(GameObjectManager.Instance.StructureList);
					AnimationEngine.Instance.Update(gameTime);
					break;
				case WorldState.Paused:
					// Update blocks only
					updateListList(GameObjectManager.Instance.StructureList);
					break;
				case WorldState.Dead:
					// Still update player so death animation can play
					updateList(GameObjectManager.Instance.PlayerList);
					break;
				case WorldState.PreLevel:
					//TODO: Hold off on writing actual level's screen, and instead draw the level start screen, then load the real level screen
					//if (gameTime.TotalGameTime.TotalSeconds - ScreenManager.Instance.TimeOfLastToggle() > 2)
					//{
					//    this.WorldState = WorldState.Playing;
					//    ScreenManager.Instance.ToggleDrawing(ScreenType.LevelLoadScreen);
					//}
					this.WorldState = WorldState.Playing;
					break;
			}
		}

		/*
         * Private Helper Methods
         */

		/*
         * Update a list of lists of IGameObjects
         */
		private void updateListList(List<List<IGameObject>> listOfList)
		{
			foreach (List<IGameObject> list in listOfList)
			{
				updateList(list);
			}
		}

		/* 
         * Update a list of IGameObjects
         */
		private void updateList(List<IGameObject> list)
		{
			foreach (IGameObject obj in list)
			{
				obj.Update(GameTime);
			}
			list.RemoveAll(x => x.destroyObject == true);
		}

		/*
         * Update each IGameObject in a list when they are within a certain distance of the player
         */
		private void updateListRange(List<IGameObject> list)
		{
			IPlayer player = (IPlayer)GameObjectManager.Instance.PlayerList[0];

			foreach (IGameObject obj in list)
			{
				// only update the objects that are currently on screen
				if (Math.Abs(player.Location.X - obj.Location.X) < ((Game1.BLOCKS_PER_SCREEN / 2) + 1) * Game1.BLOCK_WIDTH)
				{
					obj.Update(GameTime);
				}
			}
			list.RemoveAll(x => x.destroyObject == true);
		}
	}
}
