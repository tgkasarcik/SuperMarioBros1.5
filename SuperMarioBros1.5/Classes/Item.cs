using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
	public enum ItemType
	{
		None,
		RedMushroom,
		OneUpMushroom,
		FireFlower,
		Star,
		Coin,
		CoinCollect
	}

	public class Item : IItem
	{
		private static double POWER_ANIMATION_LENGTH = 0.3;    //Length of time in seconds that is takes for a power to emerge from a block
		private static double COIN_ANIMATION_LENGTH = 3.0;      //Length of time in seconds that it takes for a coin to run its collect animation

		// Public Members         
		public Vector2 Location { get; set; }
		public IObjHealth HState { get; set; }
		public IXMove XMoveState { get; set; }
		public IYMove YMoveState { get; set; }
		public bool destroyObject { get; set; }


		// Private Members         
		private ISprite sprite;
		public ItemType type;
		private double spawnTime = 0.0;         //Time at which the item was spawned in seconds
		private double collectionStart = 0.0;   //Time at which the item was collected in seconds
		private bool animating;
		private int ITEM_SPAWN_MOVE = 5;

		/*
         * Constructor
         */
		public Item(ItemType type, Vector2 location, bool itemMoving)
		{
			this.type = type;
			Location = location;
			spawnTime = 0.0;
			animating = false;
			destroyObject = false;
			AssignMoveStates(itemMoving);
			sprite = SpriteFactory.Instance.GetSprite(type.ToString());
			sprite.Location = location;
		}

		public Item(ItemType type, Vector2 location, bool itemMoving, GameTime gameTime)
		{
			this.type = type;
			Location = location;
			animating = true;
			if (type != ItemType.Coin)
			{
				spawnTime = gameTime.TotalGameTime.TotalSeconds;
			}
			else
			{
				collectionStart = gameTime.TotalGameTime.TotalSeconds;
			}
			destroyObject = false;
			AssignMoveStates(itemMoving);
			sprite = SpriteFactory.Instance.GetSprite(type.ToString());
			sprite.Location = location;
		}

		private void AssignMoveStates(bool itemMoving)
		{
			if (itemMoving)
			{
				XMoveState = new ItemMoveRight(this, true, true);
			}
			else
			{
				XMoveState = new ItemNoMoveRight(this, false, true);
			}
			if (type == ItemType.Star) {
				YMoveState = new ItemNoJump(this, true);
			} else if (type != ItemType.Coin)
            {
				YMoveState = new ItemNoJump(this, false);
			}
		}

		public void Draw(SpriteBatch spritebatch)
		{
			sprite.Draw(spritebatch);
		}

		public bool IsDrawn()
		{
			return sprite.IsDrawn();
		}

		private bool ShouldAnimate(GameTime gameTime)
		{
			if (spawnTime != 0 && (gameTime.TotalGameTime.TotalSeconds - spawnTime < POWER_ANIMATION_LENGTH) && type != ItemType.Coin)
			{
				AnimationEngine.Instance.AnimateItemOnSpawn(this);
				animating = true;
				return true;
			}
			else if (collectionStart != 0 && (gameTime.TotalGameTime.TotalSeconds - collectionStart < COIN_ANIMATION_LENGTH) && type != ItemType.Coin)
			{
				AnimationEngine.Instance.AnimateCollectedCoin(this);
				animating = true;
				return true;
			}
			else
			{
				animating = false;
				return false;
			}
		}

		public void Update(GameTime gametime)
		{
			if (type == ItemType.CoinCollect)
			{
				Location = Vector2.Add(Location, new Vector2(0, -ITEM_SPAWN_MOVE));
				if(gametime.TotalGameTime.TotalSeconds- spawnTime > 0.25)
                {
					DestroyObject();
                }
				
			}
			else if (!ShouldAnimate(gametime))
			{
				XMoveState.Update(gametime);
				if (type != ItemType.Coin)
				{
					YMoveState.Update(gametime);
				}
			}


			sprite.Location = Location;
			sprite.Update(gametime);
		}

		public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
		{
			if (isSolid && !animating)
			{
				Location = Vector2.Add(Location, new Vector2(XMoveState.CollisionAdjustment(isRight, collisionOverlap), 0));
				sprite.Location = Location;
			}
		}

		public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
		{
			if (isSolid && !animating && (type != ItemType.Coin))
			{
				Location = Vector2.Add(Location, new Vector2(0, YMoveState.CollisionAdjustment(isUp, collisionOverlap)));
				sprite.Location = Location;
			}
		}

		public Rectangle GetHitBox()
		{
			return sprite.GetHitBox();
		}

		public void Collect(GameTime gameTime)
		{
			if (type == ItemType.Coin)
			{
				//collectionStart = gameTime.TotalGameTime.TotalSeconds;
				HUD.Instance.GetCoin();
				SoundFactory.Instance.GetSoundEffect("CollectCoin").Play();
			}
			else 
			{
				DestroyObject();

				string text;

				if (type != ItemType.OneUpMushroom)
				{
					text = HUD.Instance.GetItem().ToString();
				} else
				{
					text = "1UP";
				}

				TextFactory.Instance.CreateTimedText(text, Location);
				
			}
			DestroyObject();
		}

		public void DestroyObject()
		{
			destroyObject = true;
		}
	}
}
