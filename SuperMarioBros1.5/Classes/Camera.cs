using Microsoft.Xna.Framework;

/*
 * Author: Tommy Kasarcik 
 */

namespace Sprint5
{
	/*
	 * Class for modifying the current viewport in the game world
	 */
	public class Camera : IUpdateable
	{
		/*
		 * Public Members
		 */
		public Matrix Transform { get; private set; }

		/*
		 * Private Members
		 */
		private IGameObject target;					// IGameObject to center camera on

		// Important Constants
		private const int DEFAULT_Y = 104;
		private const int MAX_Y = -16;				// Smallest Y value (highest point on physical screen) target can move to before shifting camera

		/*
		 * Constructor
		 */
		public Camera(IGameObject target)
		{
			this.target = target;
		}

		/*
		 * Update the camera defined by <c> this </c> to follow the specified target
		 */
		public void Update(GameTime gametime)
		{
			Matrix targetLocation;

			// Center screen on target
			float x = target.Location.X + (target.GetHitBox().Width / 2);

			float leftBound = (float)((Game1.BLOCKS_PER_SCREEN / 2) * Game1.BLOCK_WIDTH) + (Game1.BLOCK_WIDTH / 2);

			float rightBound = (float)((LevelLoader.Instance.NumBlocks - (Game1.BLOCKS_PER_SCREEN / 2)) * Game1.BLOCK_WIDTH) + (Game1.BLOCK_WIDTH / 2);

			// Stop camera scrolling 10 blocks before edges of world
			// First, check to see if there are <= BLOCKS_PER_SCREEN blocks in the world, the case where screen will not move
			if (LevelLoader.Instance.NumBlocks <= Game1.BLOCKS_PER_SCREEN)
			{
				x = (((LevelLoader.Instance.NumBlocks) / 2) * Game1.BLOCK_WIDTH);
			}
			// Check if player witin 10 blocks of left side of world
			else if (x < leftBound)
			{
				x = leftBound;
			}

			// Check if player witin 10 blocks of right side of world
			else if (x > rightBound)
			{
				x = rightBound;
			}

			// Move camera up if Mario moves to high
			float y = DEFAULT_Y;
			if (target.Location.Y < MAX_Y)
			{
				y += target.Location.Y - MAX_Y;
			}

			targetLocation = Matrix.CreateTranslation(-x, -y, 0);

			// Calculate translation to center the game's Render Target within the actual window

			// offset x by half of the Render Target's width
			float offsetX = Game1.RenderTargetWidth / 2;
			// offset y by 3/4 of the Render Target's height
			float offsetY = (3 * Game1.RenderTargetHeight) / 4;
			Matrix screenOffset = Matrix.CreateTranslation(offsetX, offsetY, 0);

			Transform = targetLocation * screenOffset;
		}
	}
}
