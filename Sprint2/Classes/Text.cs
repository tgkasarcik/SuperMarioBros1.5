using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
	public class Text : IGameObject
	{
		// Public Members
		public Vector2 Location { get; set; }
		public bool destroyObject { get; set; }
		public bool Moving { get; set; }
		public TimeSpan Lifetime { get; private set; }

		// Private Members
		private SpriteFont font;
		private string text;
		private Color color;

		/*
         * Constructor
         */
		public Text(string text, Vector2 location, SpriteFont font, Color color, bool moving)
		{
			this.text = text;
			this.font = font;
			Location = location;
			this.color = color;
			Moving = moving;
			Lifetime = TimeSpan.FromMilliseconds(0);
		}

		public void Draw(SpriteBatch spritebatch)
		{
			spritebatch.DrawString(font, text, Location, color);
		}

		public bool IsDrawn()
		{
			return true;
		}

		public void DisableDrawing()
		{
			// Not supported
		}

		public void EnableDrawing()
		{
			// Not supported
		}

		public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
		{
			// Does not collide
			return;
		}

		public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
		{
			// Does not collide
			return;
		}

		public Rectangle GetHitBox()
		{
			return new Rectangle();
		}

		public void DestroyObject()
		{
			destroyObject = true;
		}

		public void Update(GameTime gametime)
		{
			// For moving Text objects, decrease Y by 1 for each update
			if (Moving)
			{
				Location = Vector2.Add(Location, new Vector2(0, -1));
			}

			Lifetime += gametime.ElapsedGameTime;
		}
	}
}
