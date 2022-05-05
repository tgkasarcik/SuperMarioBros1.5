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
	 * Singleton Utility class for instantiating all <c> Text </c> objects
	 */

	public class TextFactory : IUpdateable
	{
		/*
		 * Private font
		 */
		private SpriteFont font;

		/*
		 * Private List to hold all timed <c> Text </c> objects.
		 */
		private List<Text> timedList;

		/*
		 * TimeSpan defining how long timed text should be drawn.
		 */
		private readonly TimeSpan MAX_LIFETIME = TimeSpan.FromMilliseconds(1500);

		/*
		 * Private Instance of <c> this </c>.
		 */
		private static TextFactory instance = new TextFactory();

		/*
         * Public Instance of <c> this </c>.
         */
		public static TextFactory Instance
		{
			get
			{
				return instance;
			}
		}

		/*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
		private TextFactory() 
		{
			timedList = new List<Text>();
		}

		/*
		 * Public Methods
		 */

		/*
		 * Load Content into <c> this </c>.
		 * 
		 * TODO: move to level loader
		 */
		public void LoadContent(ContentManager content)
		{
			font = content.Load<SpriteFont>("ScreenFont");
		}

		/*
		 * Create a <c> Text </c> object that is only visible for a specific amount of time.
		 */
		public void CreateTimedText(string text, Vector2 location)
		{
			// Create a new Text object
			Text temp = new Text(text, location, font, Color.White, true);

			// Give to GameObjectManager
			GameObjectManager.Instance.BackgroundList[1].Add(temp);

			// Add to timedList
			timedList.Add(temp);
		}

		/*
		 * Create a <c> Text </c> object that is visible indefinitely.
		 */
		public Text CreateUntimedText(string text, Vector2 location)
		{
			// Create a new Text object
			Text temp = new Text(text, location, font, Color.White, false);

			// Give to GameObjectManager
			GameObjectManager.Instance.BackgroundList[1].Add(temp);

			return temp;
		}

		/*
		 * Update <c> this </c>.
		 */
		public void Update(GameTime gameTime)
		{
			// Check for Text objects that need to be removed
			foreach (Text text in timedList)
			{ 
				if (text.Lifetime > MAX_LIFETIME)
				{
					text.DestroyObject();
				}
			}
			timedList.RemoveAll(x => x.destroyObject == true);
		}
	}
}
