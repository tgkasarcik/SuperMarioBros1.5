using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint5
{
	public class Game1 : Game
	{
		/*
		 * Public Members
		 */

		// These constants are not known until runtime and therefore cannot be readonly
		public static int ScreenHeight;
		public static int ScreenWidth;
		public static int RenderTargetHeight;
		public static int RenderTargetWidth;

		public static readonly int BLOCK_WIDTH = 16;
		public static readonly int BLOCK_HEIGHT = 16;
		public static readonly int BLOCKS_PER_SCREEN = 20;

		/*
		 * Private Members
		 */

		private static readonly int NUM_LEVELS = 3;
		private static readonly int INIT_LEVEL = 1;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private SpriteFactory spriteFactory;
		private SoundFactory soundFactory;
		private GameObjectManager GOM;
		private GameState gameState;
		private List<IUpdateable> updateable;
		private LevelLoader levelLoader;
		private CameraManager cameraManager;
		private ScreenManager screenManager;
		private TextFactory textFactory;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			//Initialize screen size
			ScreenHeight = graphics.PreferredBackBufferHeight;
			ScreenWidth = graphics.PreferredBackBufferWidth;

			// Use a 2.5x scale factor for render region
			RenderTargetHeight = (2 * ScreenHeight) / 5;
			RenderTargetWidth = (2 * ScreenWidth) / 5;

			// Initialize GameState
			gameState = GameState.Instance;
			gameState.Initialize(NUM_LEVELS, INIT_LEVEL);

			// Initialize SpriteFactory
			spriteFactory = SpriteFactory.Instance;

			// Initialize SoundFactory
			soundFactory = SoundFactory.Instance;

			// Initialize GameObjectManager
			GOM = GameObjectManager.Instance;
			GOM.Initialize();

			// Initialize LevelLoader
			levelLoader = LevelLoader.Instance;
			levelLoader.LoadFiles();

			// Load Spritesheets into SpriteFactory
			levelLoader.LoadSpritesheets(Content);

			// Load Sprites into SpriteFactory
			levelLoader.LoadSprites();

			// Initialize CameraManager
			cameraManager = CameraManager.Instance;

			// Initialize ScreenManager
			screenManager = ScreenManager.Instance;
			screenManager.Initialize(GraphicsDevice, graphics);

			// Initialize TextFactory
			textFactory = TextFactory.Instance;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load Sounds into SoundFactory
			soundFactory.LoadSounds(Content);

			// Load the current level, specified by GameState
			//levelLoader.LoadLevel(gameState.GetLevel());


			//TESTING
			levelLoader.LoadLevel(1);



			// Create Camera(s)
			// NOTE: Will need to change this logic to support multiplayer
			cameraManager.CreateCamera(GOM.EntityList[0][0]);

			// Load Screen Content
			screenManager.LoadContent(Content);
			screenManager.LoadSprites();

			// Load Text Content
			textFactory.LoadContent(Content);

			updateable = new List<IUpdateable>();

			updateable.Add(new KeyboardController(this, GOM, gameState));
			updateable.Add(new MouseController(this, gameState));
			updateable.Add(new CollisionDetector(gameState, GOM));
		}

		protected override void Update(GameTime gameTime)
		{

			// Update GameState
			gameState.Update(gameTime);

			foreach (var element in updateable)
			{
				element.Update(gameTime);
			}

			// Update all Cameras
			foreach (Camera cam in cameraManager.GetList())
			{
				cam.Update(gameTime);
			}

			screenManager.Update(gameTime);

			textFactory.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			screenManager.DrawScreens(spriteBatch);
			base.Draw(gameTime);
		}
	}
}
