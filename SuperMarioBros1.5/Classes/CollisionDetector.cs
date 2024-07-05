using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sprint5
{
	class CollisionDetector : IUpdateable
	{
		private ICollisionHandler collisionHandler;
		private GameObjectManager gom;


		public CollisionDetector(GameState gameState, GameObjectManager gom)
		{
			this.gom = gom;

			this.collisionHandler = new CollisionHandler(gameState);
		}

		/*
		 * Detects if any collisions have occured
		 */
		private void DetectCollisions()
		{

			foreach (var player in gom.PlayerList)
			{
				CheckForBlockCollisions(player);
				CheckForCollision(player, gom.ItemList);
				CheckForCollision(player, gom.EnemyList);
				CheckForCollision(player, gom.PlayerList);
			}

			foreach (var enemy in gom.EnemyList.ToList())
			{
				CheckForBlockCollisions(enemy);
				CheckForCollision(enemy, gom.EnemyList);
			}

			foreach (var item in gom.ItemList)
			{
				CheckForBlockCollisions(item);
			}

			foreach (var fireball in gom.FireballList)
			{
				CheckForBlockCollisions(fireball);
				CheckForCollision(fireball, gom.EnemyList);
			}


		}

		/*
		 * To optimize block collisions, only check for collisions with blocks adjacent to item
		 */
		private void CheckForBlockCollisions(IGameObject toCheck)
        {
			int currBlock = ((int)toCheck.Location.X) / Game1.BLOCK_WIDTH;

			if (currBlock >= 0 && currBlock <= LevelLoader.Instance.NumBlocks)
			{
				if (currBlock != 0)
				{
					CheckForCollision(toCheck, gom.StructureList[currBlock - 1]);
				}

				CheckForCollision(toCheck, gom.StructureList[currBlock]);

				if (currBlock != LevelLoader.Instance.NumBlocks)
				{
					CheckForCollision(toCheck, gom.StructureList[currBlock + 1]);
				}
			}
		}

		/*
         * Returns what side the collision occurred on, relative to the first object
         */
		private Direction CollisionSide(Rectangle object1, Rectangle object2, Rectangle overlap)
		{
			if (overlap.Width >= overlap.Height)
			{
				//top or bottom
				if (overlap.Center.Y < object2.Center.Y)
				{
					return Direction.TOP;
				}
				else
				{
					return Direction.BOTTOM;
				}
			}
			else
			{
				//left or right
				if (overlap.Center.X < object2.Center.X)
				{
					return Direction.LEFT;
				}
				else
				{
					return Direction.RIGHT;
				}
			}

		}

		/*
         * Given one game object and list of game objects, this determines if a collision has occured between the single game object and any of the items in the list.
         */
		private void CheckForCollision(IGameObject currObject, List<IGameObject> listOfObjects)
		{

			foreach (var element in listOfObjects.ToList())
			{
				if (!currObject.destroyObject && !element.destroyObject)
                {
					Rectangle collider = currObject.GetHitBox();
					Rectangle collided = element.GetHitBox();

					// Make sure you are not checking for a collision with the object itself.
					if (collider.Intersects(collided) && !currObject.Equals(element))
					{
						Rectangle overlap = Rectangle.Intersect(collider, collided);
						Direction side = CollisionSide(collider, collided, overlap);
						collisionHandler.HandleCollision(currObject, element, side, overlap);
					}

				}

			}

		}

		public void Update(GameTime gametime)
		{
			DetectCollisions();
		}
	}
}
