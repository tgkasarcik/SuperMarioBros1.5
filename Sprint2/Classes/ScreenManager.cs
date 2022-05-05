using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{

	/*
	 * Enum to differentiate the different types of Screens.
	 */
	public enum ScreenType
	{
		GameScreen,
		StartScreen,
		PausedScreen,
		HUD,
		DebugScreen,
		LevelLoadScreen,
		GameOver,
		TimeUp
	}

	/*
	 * Singleton class used for creating, managing, and drawing all <c> IScreen </c> objects
	 */
	public class ScreenManager : Sprint5.IUpdateable
	{
		/*
         * Private Members
         */
		private GraphicsDevice graphicsDevice;
		private GraphicsDeviceManager graphicsManager;
		private bool gameScreenVisible;
		private bool levelLoadScreenVisible;
		private bool startScreenVisible;
		private bool gameOverVisible;
		private bool timeUpVisible;
		private bool hudVisible;
		private bool debugVisible;
		private SpriteFont hudFont;
		private SpriteFont pauseFont;
		private ISprite coinSprite;
		private ISprite marioLevelLoadSprite;
		private ISprite brickSprite;
		private float fps;
		private Color backgroundColor;
		private double timeOfToggle;


		/*
		 * Important Constants
		 */
		private int SCREEN_WIDTH = Game1.ScreenWidth;
		private int SCREEN_HEIGHT = Game1.ScreenHeight;
		private int RENDER_TARGET_WIDTH = Game1.RenderTargetWidth;
		private int RENDER_TARGET_HEIGHT = Game1.RenderTargetHeight;
		private int PX_PER_CHAR;
		private int HUD_COLUMNS;

		/*
         * Private Instance of <c> this </c>.
         */
		private static ScreenManager instance = new ScreenManager();

		/*
         * Public Instance of <c> this </c>.
         */
		public static ScreenManager Instance
		{
			get
			{
				return instance;
			}
		}

		/*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
		private ScreenManager() { }

		/*
         * Public Methods
         */
		public void Initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsManager)
		{
			this.graphicsDevice = graphicsDevice;
			this.graphicsManager = graphicsManager;

			// Initialize constants
			PX_PER_CHAR = 20;
			HUD_COLUMNS = 4;

			// Set Start Screen, HUD visible to start
			gameScreenVisible = true;
			startScreenVisible = false;     //change back to true once implemented
			levelLoadScreenVisible = false;
			hudVisible = true;
			gameOverVisible = false;
			timeUpVisible = false;
			debugVisible = false;
		}

		/*
		 * Set the current background color of the world
		 */
		public void SetBackgroundColor(Color color)
		{
			backgroundColor = color;
		}

		/*
		 * Load Content into <c> this </c>.
		 * 
		 * NOTE: Will move to level loader soon.
		 */
		public void LoadContent(ContentManager content)
		{
			hudFont = content.Load<SpriteFont>("HUDFont");
			pauseFont = content.Load<SpriteFont>("PauseFont");
		}

		/*
		 * Load Sprites for <c> this </c>
		 */
		public void LoadSprites()
		{
			coinSprite = SpriteFactory.Instance.GetSprite("Coin");
			brickSprite = SpriteFactory.Instance.GetSprite("Brick");
			marioLevelLoadSprite = SpriteFactory.Instance.GetSprite("SmallMarioRightStill");
		}

		/*
         * Toggle the drawing of the <c> IScreen </c> corresponding to the given <c> ScreenType </c>
         */
		public void ToggleDrawing(ScreenType type)
		{
			timeOfToggle = GameState.Instance.GameTime.TotalGameTime.TotalSeconds;
			switch (type)
			{
				case ScreenType.GameScreen:
					gameScreenVisible = !gameScreenVisible;
					break;
				case ScreenType.StartScreen:
					startScreenVisible = !startScreenVisible;
					break;
				case ScreenType.PausedScreen:
					// Do nothing, visibility of Paused Screen handled with GameState.WorldState
					break;
				case ScreenType.HUD:
					hudVisible = !hudVisible;
					break;
				case ScreenType.GameOver:
					gameOverVisible = !gameOverVisible;
					break;
				case ScreenType.TimeUp:
					timeUpVisible = !timeUpVisible;
					break;
				case ScreenType.DebugScreen:
					debugVisible = !debugVisible;
					break;
				case ScreenType.LevelLoadScreen:
					levelLoadScreenVisible = !levelLoadScreenVisible;
					break;
			}
		}

		/*
		 * Update <c> this </c>.
		 */
		public void Update(GameTime gameTime)
		{
			coinSprite.Update(gameTime);
			fps = (1 / (float)gameTime.ElapsedGameTime.TotalSeconds);

			HUD.Instance.Update(gameTime);
		}

		/*
         * Draw all currently visible <c> IScreens </c> in the order specified by <c> visibleScreens </c>
         */
		public void DrawScreens(SpriteBatch spriteBatch)
		{
			// Draw to Render Target to allow for screen scaling
			RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

			// Draw Game Screen
			if (gameScreenVisible)
			{
				drawGameScreen(spriteBatch, renderTarget);
			}

			// Draw render target to screen
			graphicsDevice.SetRenderTarget(null);
			graphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
			spriteBatch.Draw(renderTarget, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

			// Draw other screens as necessary
			if (startScreenVisible)
			{
				drawStartScreen(spriteBatch);
			}

			if (levelLoadScreenVisible)
			{
				drawLevelLoadScreen(spriteBatch);
			}

			if (GameState.Instance.WorldState == WorldState.Paused)
			{
				drawPauseScreen(spriteBatch);
			}

			if (hudVisible)
			{
				drawHUD(spriteBatch);
			}

			if (gameOverVisible)
			{
				drawGameOver(spriteBatch);
			}

			if (timeUpVisible)
			{
				drawTimeUp(spriteBatch);
			}

			if (debugVisible)
			{
				drawDebugScreen(spriteBatch);
			}

			spriteBatch.End();
			renderTarget.Dispose();
		}

		/*
         * Private Helper Methods
         */

		/*
		 * Draw the Game Screen to a <c> RenderTarget2D </c>.
		 */
		private RenderTarget2D drawGameScreen(SpriteBatch spriteBatch, RenderTarget2D renderTarget)
		{
			graphicsDevice.SetRenderTarget(renderTarget);
			graphicsDevice.Clear(backgroundColor);
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, transformMatrix: CameraManager.Instance.GetCamera(GameObjectManager.Instance.EntityList[0][0]).Transform);

			// Draw sprites for all objects in GameObjectManager
			GameObjectManager.Instance.DrawObjects(spriteBatch);
			spriteBatch.End();

			return renderTarget;
		}

		/*
		 * Draw the Start Screen to the specified <c> SpriteBatch </c>.
		 */
		private void drawStartScreen(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(hudFont, "This will eventually be the start screen", new Vector2(25, 250), Color.White);
		}

		/*
		 * Draw the Level Loading Screen for the current level to the specified <c> SpriteBatch </c>.
		 */
		private void drawLevelLoadScreen(SpriteBatch spriteBatch)
		{
			// Magic numbers here correspond to on screen locations
			string world = "World " + GameState.Instance.CurrentLevelName;
			spriteBatch.DrawString(hudFont, world, new Vector2(calculateCenteredX(world.Length), 150), Color.White);

			marioLevelLoadSprite.Location = new Vector2(350, 200);
			marioLevelLoadSprite.Draw(spriteBatch);

			string lives = "  * " + HUD.Instance.getValue("Lives").ToString();
			spriteBatch.DrawString(hudFont, lives, new Vector2(calculateCenteredX(lives.Length), 200), Color.White);
		}

		/*
		 * Draw the Pause Screen to the specified <c> SpriteBatch </c>.
		 */
		private void drawPauseScreen(SpriteBatch spriteBatch)
		{
			// Magic numbers here correspond to on screen locations
			string paused = "PAUSED";
			spriteBatch.DrawString(hudFont, paused, new Vector2(calculateCenteredX(paused.Length), 200), Color.White);
		}

		/*
		 * Draw the HUD to the specified <c> SpriteBatch </c>.
		 */
		private void drawHUD(SpriteBatch spriteBatch)
		{
			// First paramater of calculateColX() corresponds to the column of the screen to draw to
			// All other magic numbers correspond to on screen coordinates

			string playerName = "Mario";
			spriteBatch.DrawString(hudFont, playerName, new Vector2(calculateColX(1, playerName.Length), 10), Color.White);
			spriteBatch.DrawString(hudFont, HUD.Instance.getValue("Score"), new Vector2(calculateColX(1, HUD.Instance.getValue("Score").Length), 30), Color.White);

			coinSprite.Location = new Vector2(230, 34);
			coinSprite.Draw(spriteBatch);

			string coins = " * " + HUD.Instance.getValue("Coin").ToString();
			spriteBatch.DrawString(hudFont, coins, new Vector2(calculateColX(2, coins.Length), 30), Color.White);

			brickSprite.Location = new Vector2(230, 14);
			brickSprite.Draw(spriteBatch);

			string bricks = " * " + HUD.Instance.getValue("Block").ToString();
			spriteBatch.DrawString(hudFont, bricks, new Vector2(calculateColX(2, bricks.Length), 10), Color.White);

			string world = "World";
			spriteBatch.DrawString(hudFont, world, new Vector2(calculateColX(3, world.Length), 10), Color.White);

			string worldName = GameState.Instance.CurrentLevelName;
			spriteBatch.DrawString(hudFont, worldName, new Vector2(calculateColX(3, worldName.Length), 30), Color.White);

			string time = "Time";
			spriteBatch.DrawString(hudFont, time, new Vector2(calculateColX(4, time.Length), 10), Color.White);

			spriteBatch.DrawString(hudFont, "Time", new Vector2(calculateColX(4, 4), 10), Color.White);
			spriteBatch.DrawString(hudFont, HUD.Instance.getValue("Time"), new Vector2(calculateColX(4, 3), 30), Color.White);
		}

		/*
		 * Draw the Game Over Screen to the specified <c> SpriteBatch </c>.
		 */
		private void drawGameOver(SpriteBatch spriteBatch)
		{
			// Magic numbers here correspond to on screen locations
			string msg = "Game Over";
			spriteBatch.DrawString(hudFont, msg, new Vector2(calculateCenteredX(msg.Length), 150), Color.White);
		}

		/*
		 * Draw the Time Up Screen to the specified <c> SpriteBatch </c>.
		 */
		private void drawTimeUp(SpriteBatch spriteBatch)
		{
			// Magic numbers here correspond to on screen locations
			string msg = "Time Up";
			spriteBatch.DrawString(hudFont, msg, new Vector2(calculateCenteredX(msg.Length), 150), Color.White);
		}

		/*
		 * Draw the Debug Screen to the specified <c> SpriteBatch </c>.
		 */
		private void drawDebugScreen(SpriteBatch spriteBatch)
		{
			// Magic numbers here correspond to on screen locations
			spriteBatch.DrawString(hudFont, "FPS: " + fps.ToString(), new Vector2(0, 60), Color.Yellow);
			spriteBatch.DrawString(hudFont, "X: " + GameObjectManager.Instance.EntityList[0][0].Location.X, new Vector2(0, 90), Color.Yellow);
			spriteBatch.DrawString(hudFont, "Y: " + GameObjectManager.Instance.EntityList[0][0].Location.Y, new Vector2(0, 120), Color.Yellow);
			spriteBatch.DrawString(hudFont, "Block: " + ((int)GameObjectManager.Instance.EntityList[0][0].Location.X / 16), new Vector2(0, 150), Color.Yellow);
		}

		/*
		 * Calculate X such that text can be alligned to be centered within 4 columns on screen
		 */
		private int calculateColX(int column, int numChars)
		{
			int textWidth = numChars * PX_PER_CHAR;
			int colWidth = SCREEN_WIDTH / HUD_COLUMNS;
			int x = (colWidth / 2) - (textWidth / 2);
			x += (column - 1) * colWidth;

			return x;
		}

		/*
		 * Calculate X such that text will be centered in the middle of the screen
		 */
		private int calculateCenteredX(int numChars)
		{
			int textWidth = numChars * PX_PER_CHAR;
			int x = (SCREEN_WIDTH / 2) - (textWidth / 2);

			return x;
		}

		public double TimeOfLastToggle()
        {
			return timeOfToggle;
        }

	}
}