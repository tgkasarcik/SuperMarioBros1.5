using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    public enum DebrisType
    {
        BrickPiece1,
        BrickPiece2,
        BrickPiece3,
        BrickPiece4
    }
    public class Debris : IGameObject
    {
        public bool destroyObject { get; set; }
        public Vector2 Location { get; set; }
        public ISprite sprite;
        private DebrisType Type;
        private int XSpeed = 1;
        private int YSpeed = -2;
        private int YSpeed2 = -1;
        private int YAcceraltion = 1;
        private int DestroyTime = 15;

        private int creationTime;    //Time in seconds of when the Debris was created
        private int LIFE_TIME = 5; //Number of seconds the Debris is allowed to last before disappearing

        public Debris(Vector2 location, DebrisType type, GameTime gameTime)
        {
            creationTime = gameTime.TotalGameTime.Seconds;
            destroyObject = false;
            this.Location = location;
            Type = type;
            sprite = SpriteFactory.Instance.GetSprite(type.ToString());
            sprite.Location = Location;
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            return;
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            return;
        }

        public void DestroyObject()
        {
            destroyObject = true;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            sprite.Draw(spritebatch);
        }

        public Rectangle GetHitBox()
        {
            return sprite.GetHitBox();
        }

        public bool IsDrawn()
        {
            return sprite.IsDrawn();
        }

        public void Update(GameTime gametime)
        {
            if(Type == DebrisType.BrickPiece1)
            {
                Location = new Vector2(Location.X - XSpeed, Location.Y + YSpeed);
            }
            else if (Type == DebrisType.BrickPiece2)
            {
                Location = new Vector2(Location.X + XSpeed, Location.Y + YSpeed);
            }
            else if (Type == DebrisType.BrickPiece3)
            {
                Location = new Vector2(Location.X - XSpeed, Location.Y + YSpeed2);
            }
            else if (Type == DebrisType.BrickPiece4)
            {
                Location = new Vector2(Location.X + XSpeed, Location.Y + YSpeed2);
            }
            YSpeed += YAcceraltion;
            YSpeed2 += YAcceraltion;
            if(YSpeed == DestroyTime || YSpeed2 == DestroyTime)
            {
                DestroyObject();
            }

            sprite.Location = Location;
            sprite.Update(gametime);

        }
    }
}
