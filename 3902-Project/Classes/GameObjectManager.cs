using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
	/*
     * Singleton class used for handling of all game objects.
     */
	public class GameObjectManager
	{

		// Public Master List to hold all <c> IGameObjects </c> in the current level
		public List<List<List<IGameObject>>> MasterList { get; private set; }
		
		//Public Entity List to hold all lists of movable and interactible <c> IGameObjects </c> grouped by type       
		public List<List<IGameObject>> EntityList { get; private set; }

		// Public Structure List to hold all non-movable and interactible blocks and structures, grouped by X value
		public List<List<IGameObject>> StructureList { get; private set; }

		// Public Background List to hold all non-movable and non-interactible <c> IGameObjects </c>
		public List<List<IGameObject>> BackgroundList { get; private set; }

		// List of new enemy objects (Specifically shells for Sprint3) to add to game after objects are interacted with
		public List<IGameObject> CreatedShells { get; private set; }

		// Public lists of players, enemies, blocks, structures, and items         
		public List<IGameObject> PlayerList;
		public List<IGameObject> EnemyList;
		public List<IGameObject> ItemList;
		public List<IGameObject> FireballList;
		public List<IGameObject> SkyObjectList;
		public List<IGameObject> GroundObjectList;

		// Private Instance of <c> this </c>.         
		private static GameObjectManager instance = new GameObjectManager();

		/*
         * Public Instance of <c> this </c>.
         */
		public static GameObjectManager Instance
		{
			get
			{
				return instance;
			}
		}

		/*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
		private GameObjectManager()
		{
		}

		/*
         * Initialize <c> this </c>.  Must be called before <c> this </c> can be used.
         */
		public void Initialize()
		{
			MasterList = new List<List<List<IGameObject>>>();
			EntityList = new List<List<IGameObject>>();
			StructureList = new List<List<IGameObject>>();
			BackgroundList = new List<List<IGameObject>>();
			CreatedShells = new List<IGameObject>();

			PlayerList = new List<IGameObject>();
			EnemyList = new List<IGameObject>();
			ItemList = new List<IGameObject>();
			FireballList = new List<IGameObject>();
			SkyObjectList = new List<IGameObject>();
			GroundObjectList = new List<IGameObject>();

			EntityList.Add(PlayerList);
			EntityList.Add(EnemyList);
			EntityList.Add(ItemList);
			EntityList.Add(FireballList);

			BackgroundList.Add(SkyObjectList);
			BackgroundList.Add(GroundObjectList);

			MasterList.Add(EntityList);
			MasterList.Add(StructureList);
			MasterList.Add(BackgroundList);
		}

		/*
         * Draw each game object to screen.
         */
		public void DrawObjects(SpriteBatch spritebatch)
		{
			foreach (List<List<IGameObject>> listOfList in MasterList)
			{
				foreach (List<IGameObject> listOfObj in listOfList)
				{
					foreach (IGameObject obj in listOfObj)
					{
						obj.Draw(spritebatch);
					}
				}
			}
		}

		/*
         * Update each game object.
         */
		public void Update(GameTime gameTime)
		{
			foreach (List<List<IGameObject>> listOfList in MasterList)
			{
				foreach (List<IGameObject> listOfObj in listOfList)
				{
					foreach (IGameObject obj in listOfObj)
					{
						obj.Update(gameTime);
					}
					listOfObj.RemoveAll(x => x.destroyObject == true);
				}
			}
		}

		/*
		 * Clears all the objects stored in <c> this </c>.  Should only be called by GameState when changing level.
		 */
		public void ClearObjects()
		{
			// Clear all entities except for Players
			// PlayerList is 0th entry in EntityList
			for (int i = 1; i < EntityList.Count; i++)
			{
				EntityList[i].Clear();
			}

			// Clear all background objects in BackgroundList
			foreach (List<IGameObject> list in BackgroundList)
			{
				list.Clear();
			}

			// Clear the entire StructureList, because next level may have different number of columns of blocks
			StructureList.Clear();
		}
	}
}
