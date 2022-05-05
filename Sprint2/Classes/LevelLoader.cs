using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
	/*
     * Singleton class to handle loading of levels from data files
     * 
     * NOTE: this is a LONG class (currently 700+ lines).  I would have loved to optimize it, but there simply wasn't any time.
     */
	public class LevelLoader
	{
		/*
         * Public Level Data
         */
		public int NumBlocks { get; private set; }

		/*
         * Dictionary of files containing Level data
         */
		private Dictionary<int, string> levelFileDict;

		/*
		 * Dictionary of files containing Sprite data
		 */
		private Dictionary<string, string> spriteFileDict;

		/*
		 * Dictionary of files containing data for various other classes
		 */
		private Dictionary<string, string> dataFileDict;

		/*
		 * Y Value of the top block of the world floor
		 */
		private static readonly int FLOOR_Y = 128;

		/*
		 * Constants for drawing clouds
		 */
		private static readonly int BLOCKS_PER_CLOUD_REPEAT = 48;
		private static readonly int UPPER_CLOUD_Y = -48;
		private static readonly int LOWER_CLOUD_Y = -32;
		private static readonly int CLOUD1_BLOCK = 8;
		private static readonly int CLOUD2_BLOCK = 19;
		private static readonly int CLOUD3_BLOCK = 27;
		private static readonly int CLOUD4_BLOCK = 36;

		/*
		 * Constants for drawing OSU Logos
		 */
		private static readonly int BLOCKS_PER_OSU_REPEAT = 32;
		private static readonly int OSU_LOGO_Y = 32;
		private static readonly int OSU_LOGO_OFFSET_X = 23;

		/*
         * Private instance of <c> this </c>
         */
		private static LevelLoader instance = new LevelLoader();

		/*
         * Public instance of <c> this </c>
         */
		public static LevelLoader Instance
		{
			get
			{
				return instance;
			}
		}

		/*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
		private LevelLoader()
		{
			NumBlocks = -1;
			levelFileDict = new Dictionary<int, string>();
			spriteFileDict = new Dictionary<string, string>();
			dataFileDict = new Dictionary<string, string>();
		}

		/*
         * Public methods
         */

		/*
         * Load XML files into <c> this </c>.  Should only be called in Game1.LoadContent()
         */
		public void LoadFiles()
		{
			// NOTE: Add all underground levels after all main levels
			//levelFileDict.Add(0, @"Data\DevTest-data.xml");
			//levelFileDict.Add(1, @"Data\Level1-data.xml");
			levelFileDict.Add(1, @"Data\New-Level1-data.xml");
			levelFileDict.Add(2, @"Data\Level2-data.xml");
			levelFileDict.Add(3, @"Data\Level3-data.xml");
			levelFileDict.Add(4, @"Data\Level4-data.xml");
			//...

			levelFileDict.Add(5, @"Data\Level1-Underground1-data.xml");

			spriteFileDict.Add("Overworld", @"Data\Overworld-Palette.xml");
			spriteFileDict.Add("Underworld", @"Data\Underworld-Palette.xml");
			spriteFileDict.Add("Mario", @"Data\Mario-Palette.xml");
			spriteFileDict.Add("Background", @"Data\Background-Palette.xml");
			spriteFileDict.Add("Tropical", @"Data\Tropical-Palette.xml");
			spriteFileDict.Add("Fire", @"Data\Fire-Palette.xml");
			spriteFileDict.Add("Ice", @"Data\Ice-Palette.xml");
			spriteFileDict.Add("OSU", @"Data\OSU-Palette.xml");

			dataFileDict.Add("Spritesheets", @"Data\Spritesheets.xml");
		}

		/*
         * Load the data from the specified data file and pass it to <c> GameObjectManager </c>
         */
		public void LoadLevel(int level)
		{
			using (XmlReader reader = XmlReader.Create(levelFileDict[level]))
			{

				while (reader.Read())
				{

					if (reader.IsStartElement())
					{

						switch (reader.Name)
						{
							case "name":
								GameState.Instance.CurrentLevelName = reader.ReadElementContentAsString();
								break;
							case "song":
								loadSong(reader.ReadElementContentAsString());
								break;
							case "palette":
								loadPalette(reader.ReadElementContentAsString());
								break;
							case "terrain":
								loadTerrain(reader.ReadSubtree());
								break;
							case "mario":
								loadMario(reader.ReadSubtree());
								break;
							case "block":
								loadBlock(reader.ReadSubtree());
								break;
							case "background":
								loadBackground(reader.ReadSubtree());
								break;
							case "pipe":
								loadPipe(reader.ReadSubtree());
								break;
							case "item":
								loadItem(reader.ReadSubtree());
								break;
							case "koopa":
								loadKoopa(reader.ReadSubtree());
								break;
							case "goomba":
								loadGoomba(reader.ReadSubtree());
								break;
							case "flag":
								loadFlag(reader.ReadSubtree());
								break;
							default:
								break;
						}
					}
				}
			}

		}

		/*
         * Load Spritesheets into Spritefactory
         */
		public void LoadSpritesheets(ContentManager content)
		{
			// Load Spritesheets
			using (XmlReader reader = XmlReader.Create(dataFileDict["Spritesheets"]))
			{

				while (reader.Read())
				{

					if (reader.IsStartElement())
					{

						switch (reader.Name)
						{
							case "spritesheet":
								loadSpritesheet(content, reader.ReadSubtree());
								break;
						}
					}
				}
			}
		}

		/*
		 * Load Sprites into SpriteFactory
		 */
		public void LoadSprites()
		{
			// Load Sprites that will always be present, regardless of level palette
			loadPalette("Background");
			loadPalette("Mario");
		}

		/*
         * Private helper methods
         */

		/*
         * Load one Spritesheet
         */
		private void loadSpritesheet(ContentManager content, XmlReader reader)
		{
			string name = "", path = "";

			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "name":
						name = reader.ReadElementContentAsString();
						break;
					case "path":
						path = reader.ReadElementContentAsString();
						break;
				}
			}

			SpriteFactory.Instance.RegisterTexture(content, name, path);
		}

		/*
         * Load the specified Color Palette
         */
		private void loadPalette(string palette)
		{
			using (XmlReader reader = XmlReader.Create(spriteFileDict[palette]))
			{
				while (reader.Read())
				{

					if (reader.IsStartElement())
					{

						switch (reader.Name)
						{
							case "background-color":
								ScreenManager.Instance.SetBackgroundColor(loadColor(reader.ReadSubtree()));
								break;
							case "sprite":
								loadSprite(reader.ReadSubtree());
								break;
						}
					}
				}
			}

		}

		/*
         * Load the Background Color for the level
         */
		private Color loadColor(XmlReader reader)
		{
			// Set to white by default so errors are obvious
			int r = 255, g = 255, b = 255, a = 255;

			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "r":
						r = reader.ReadElementContentAsInt();
						break;
					case "g":
						g = reader.ReadElementContentAsInt();
						break;
					case "b":
						b = reader.ReadElementContentAsInt();
						break;
					case "a":
						a = reader.ReadElementContentAsInt();
						break;
					default:
						break;
				}
			}

			return new Color(r, g, b, a);
		}

		/*
         * Load a Sprite for the Level
         */
		private void loadSprite(XmlReader reader)
		{
			// Set variables to default values
			string name = "MissingTexture", texture = "missingTexture";
			int x = 0, y = 0, width = 16, height = 16, frames = 1, msPerFrame = 200;
			SpriteEffects effect = SpriteEffects.None;
			Color color = Color.White;
			SpriteOverlay overlay = SpriteOverlay.None;
			float depth = 1.0f;

			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "name":
						name = reader.ReadElementContentAsString();
						break;
					case "texture":
						texture = reader.ReadElementContentAsString();
						break;
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					case "width":
						width = reader.ReadElementContentAsInt();
						break;
					case "height":
						height = reader.ReadElementContentAsInt();
						break;
					case "frames":
						frames = reader.ReadElementContentAsInt();
						break;
					case "msPerFrame":
						msPerFrame = reader.ReadElementContentAsInt();
						break;
					case "effect":
						effect = (SpriteEffects)Enum.Parse(typeof(SpriteEffects), reader.ReadElementContentAsString());
						break;
					case "color":
						color = loadColor(reader.ReadSubtree());
						break;
					case "overlay":
						overlay = (SpriteOverlay)Enum.Parse(typeof(SpriteOverlay), reader.ReadElementContentAsString());
						break;
					case "depth":
						depth = reader.ReadElementContentAsFloat();
						break;
					default:
						break;
				}
			}

			SpriteFactory.Instance.RegisterSprite(name, texture, x, y, width, height, frames, msPerFrame, effect, color, overlay, depth);
		}

		/*
		 * Load the background song for the current level
		 */
		private void loadSong(string songName)
		{
			MediaPlayer.Stop();
			GameState.Instance.BackgroundSong = SoundFactory.Instance.GetSong(songName);
			MediaPlayer.Play(GameState.Instance.BackgroundSong);
			MediaPlayer.IsRepeating = true;
		}

		/*
         * Load the ground for the level
         */
		private void loadTerrain(XmlReader reader)
		{
			bool floor = false;
			bool clouds = false;
			bool osu = false;
			List<int> holes = new List<int>();
			BlockType borderType = BlockType.HiddenBlock;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "floor":
						floor = reader.ReadElementContentAsBoolean();
						break;
					case "clouds":
						clouds = reader.ReadElementContentAsBoolean();
						break;
					case "osu":
						osu = reader.ReadElementContentAsBoolean();
						break;
					case "numblocks":
						NumBlocks = reader.ReadElementContentAsInt();
						break;
					case "hole":
						holes.Add(reader.ReadElementContentAsInt());
						break;
					case "border-block":
						borderType = (BlockType)Enum.Parse(typeof(BlockType), reader.ReadElementContentAsString());
						break;
					default:
						break;
				}
			}

			BlockType floorType;
			if (floor)
			{
				floorType = BlockType.GroundBlock;
			}
			else
			{
				floorType = BlockType.HiddenBlock;
			}

			// Create the ground for the world
			for (int i = 0; i <= NumBlocks; i++)
			{

				// Create a list of IGameObjects to represent each column of blocks in the world
				GameObjectManager.Instance.StructureList.Add(new List<IGameObject>());

				// Add clouds
				if (clouds)
				{
					if (i % BLOCKS_PER_CLOUD_REPEAT == 0)
					{
						GameObjectManager.Instance.BackgroundList[0].Add(new BackgroundObject(new Vector2((i + CLOUD1_BLOCK) * Game1.BLOCK_WIDTH, LOWER_CLOUD_Y), SpriteFactory.Instance, BackgroundType.Cloud1));
						GameObjectManager.Instance.BackgroundList[0].Add(new BackgroundObject(new Vector2((i + CLOUD2_BLOCK) * Game1.BLOCK_WIDTH, UPPER_CLOUD_Y), SpriteFactory.Instance, BackgroundType.Cloud1));
						GameObjectManager.Instance.BackgroundList[0].Add(new BackgroundObject(new Vector2((i + CLOUD3_BLOCK) * Game1.BLOCK_WIDTH, LOWER_CLOUD_Y), SpriteFactory.Instance, BackgroundType.Cloud3));
						GameObjectManager.Instance.BackgroundList[0].Add(new BackgroundObject(new Vector2((i + CLOUD4_BLOCK) * Game1.BLOCK_WIDTH, UPPER_CLOUD_Y), SpriteFactory.Instance, BackgroundType.Cloud2));
					}
				}

				// Add OSU logos
				if (osu)
				{
					if (i % BLOCKS_PER_OSU_REPEAT == 0)
					{
						int x = ((i - OSU_LOGO_OFFSET_X) * Game1.BLOCK_WIDTH) + (Game1.BLOCK_WIDTH / 2);
						GameObjectManager.Instance.BackgroundList[0].Add(new BackgroundObject(new Vector2(x, OSU_LOGO_Y), SpriteFactory.Instance, BackgroundType.OSULogo));
					}
				}

				// Create world borders of Hidden Blocks at beginning and end of world 
				if ((i == 0) || (i == NumBlocks))
				{
					// Make borders 8 blocks tall
					for (int j = 0; j <= 8; j++)
					{
						GameObjectManager.Instance.StructureList[i].Add(new Block(new Vector2(i * Game1.BLOCK_WIDTH, ((FLOOR_Y - Game1.BLOCK_HEIGHT) - (j * Game1.BLOCK_HEIGHT))), SpriteFactory.Instance, borderType, ItemType.Star, 0));
						if (j == 7 && i == 0)
						{
							GameObjectManager.Instance.StructureList[i].Add(new Block(new Vector2(i + Game1.BLOCK_WIDTH, ((FLOOR_Y - Game1.BLOCK_HEIGHT) - (j * Game1.BLOCK_HEIGHT))), SpriteFactory.Instance, borderType, ItemType.Star, 0));
						}

						if (j == 7 && i == NumBlocks)
						{
							GameObjectManager.Instance.StructureList[i].Add(new Block(new Vector2((i * Game1.BLOCK_WIDTH) - Game1.BLOCK_WIDTH, ((FLOOR_Y - Game1.BLOCK_HEIGHT) - (j * Game1.BLOCK_HEIGHT))), SpriteFactory.Instance, borderType, ItemType.Star, 0));
						}
					}
				}

				// If there is a hole at the current index, do not create the ground blocks
				if (holes.Contains(i)) continue;

				// Otherwise, create the ground blocks and add to the ith List
				GameObjectManager.Instance.StructureList[i].Add(new Block(new Vector2(i * Game1.BLOCK_WIDTH, FLOOR_Y), SpriteFactory.Instance, floorType, ItemType.Star, 0));

				GameObjectManager.Instance.StructureList[i].Add(new Block(new Vector2(i * Game1.BLOCK_WIDTH, FLOOR_Y + Game1.BLOCK_HEIGHT), SpriteFactory.Instance, floorType, ItemType.Star, 0));
			}
		}

		/*
         * Load one Mario object and send it to GameObjectManager
         */
		private void loadMario(XmlReader reader)
		{
			int x = 0, y = 0;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					default:
						break;
				}
			}

			// Check if there is already a player
			if (GameObjectManager.Instance.PlayerList.Count > 0)
			{
				// Override player's current position and move them to the specified spawnpoint
				if (!GameState.Instance.ExitingPipe)
				{
					foreach (IGameObject obj in GameObjectManager.Instance.PlayerList)
					{
						IPlayer player = (IPlayer)obj;
						player.Location = new Vector2(x, y);
					}
				}
				GameState.Instance.ExitingPipe = false;
			}
			// Otherwise create a new player at the current location
			else
			{
				Mario temp = new Mario(new Vector2(x, y), GameState.Instance.GameTime, false, true, false, true, SpriteFactory.Instance);
				GameObjectManager.Instance.PlayerList.Add(temp);
			}
		}

		/*
         * Load one block from the xml file, create a new object, send to GameObjectManager
         */
		private void loadBlock(XmlReader reader)
		{
			BlockType type = BlockType.UsedBlock;
			ItemType power = ItemType.Star;
			int x = 0, y = 0, coins = 0;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "type":
						type = (BlockType)Enum.Parse(typeof(BlockType), reader.ReadElementContentAsString());
						break;
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					case "coins":
						coins = reader.ReadElementContentAsInt();
						break;
					case "power":
						power = (ItemType)Enum.Parse(typeof(ItemType), reader.ReadElementContentAsString());
						break;
					default:
						break;
				}
			}

			// Create new block object based off data
			Block temp = new Block(new Vector2(x, y), SpriteFactory.Instance, type, power, coins);

			// Add temp to proper list of StructureList based on its X coordinate
			GameObjectManager.Instance.StructureList[x / 16].Add(temp);

		}

		/*
         * Load one BackgroundObject and send to GameObjectManager
         */
		private void loadBackground(XmlReader reader)
		{
			BackgroundType type = BackgroundType.Cloud1;
			int x = 0, y = 0;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "type":
						type = (BackgroundType)Enum.Parse(typeof(BackgroundType), reader.ReadElementContentAsString());
						break;
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					default:
						break;
				}
			}

			if (type == BackgroundType.Castle)
			{
				GameState.Instance.CastleX = x;
			}

			BackgroundObject temp = new BackgroundObject(new Vector2(x, y), SpriteFactory.Instance, type);
			GameObjectManager.Instance.BackgroundList[0].Add(temp);
		}

		/*
         * Load one Pipe object and send it to GameObjectManager
         */
		private void loadPipe(XmlReader reader)
		{
			PipeType type = PipeType.UpPipe;
			PipeType outType = PipeType.None;
			int x = 0, y = 0, nextLevel = -1, outX = -1, outY = -1;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "type":
						type = (PipeType)Enum.Parse(typeof(PipeType), reader.ReadElementContentAsString());
						break;
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					case "next-level":
						nextLevel = reader.ReadElementContentAsInt();
						break;
					case "out-x":
						outX = reader.ReadElementContentAsInt();
						break;
					case "out-y":
						outY = reader.ReadElementContentAsInt();
						break;
					case "out-pipe-type":
						outType = (PipeType)Enum.Parse(typeof(PipeType), reader.ReadElementContentAsString());
						break;
					default:
						break;
				}
			}

			// Create new pipe from file data

			Pipe temp;

			// If nextLevel has been entered, an interactive pipe is present
			if (nextLevel != -1)
			{
				temp = new Pipe(new Vector2(x, y), SpriteFactory.Instance, type, nextLevel, new Vector2(outX, outY), outType);
			} 
			
			// Otherwise, pipe is not interactive 
			else
			{
				temp = new Pipe(new Vector2(x, y), SpriteFactory.Instance, type);
			}

			// Add temp to proper list of StructureList based on its X coordinate
			GameObjectManager.Instance.StructureList[x / 16].Add(temp);
		}

		/*
         * Load one Item object and send it to GameObjectManager
         */
		private void loadItem(XmlReader reader)
		{
			ItemType type = ItemType.Coin;
			int x = 0, y = 0;
			bool moving = false;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "type":
						type = (ItemType)Enum.Parse(typeof(ItemType), reader.ReadElementContentAsString());
						break;
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					default:
						break;
				}
			}

			Item temp = new Item(type, new Vector2(x, y), moving);
			GameObjectManager.Instance.EntityList[2].Add(temp);
		}

		//TODO: find a better way to load enemies, this is just to get it working

		/*
         * Load one Koopa object and send it to GameObjectManager
         */
		private void loadKoopa(XmlReader reader)
		{
			int x = 0, y = 0;
			bool moving = true;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					case "moving":
						moving = reader.ReadElementContentAsBoolean();
						break;
					default:
						break;
				}
			}

			Koopa temp = new Koopa(new Vector2(x, y), moving, false, false, true, SpriteFactory.Instance);
			GameObjectManager.Instance.EntityList[1].Add(temp);
		}

		/*
         * Load one Goomba object and send it to GameObjectManager
         */
		private void loadGoomba(XmlReader reader)
		{
			int x = 0, y = 0;
			bool moving = true;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					case "moving":
						moving = reader.ReadElementContentAsBoolean();
						break;
					default:
						break;
				}
			}

			Goomba temp = new Goomba(new Vector2(x, y), moving, false, false, true, SpriteFactory.Instance);
			GameObjectManager.Instance.EntityList[1].Add(temp);
		}

		/*
         * Load one Flag object and send to GameObjectManager
         */
		private void loadFlag(XmlReader reader)
		{
			int x = 0, y = 0;

			// Load data from file
			while (reader.Read())
			{
				switch (reader.Name)
				{
					case "x":
						x = reader.ReadElementContentAsInt();
						break;
					case "y":
						y = reader.ReadElementContentAsInt();
						break;
					default:
						break;
				}
			}

			Flag temp = new Flag(new Vector2(x, y), SpriteFactory.Instance);
			GameObjectManager.Instance.StructureList[x / 16].Add(temp);
		}
	}
}
