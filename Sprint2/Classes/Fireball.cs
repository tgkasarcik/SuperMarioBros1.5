using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Sprint5
{
    public class Fireball : IGameObject, ISimpleMovementType
    {
        private static int ZERO_CHANGE = 0; //A change of 0 to be used when modifying numbers such as in the collision adjustments

        public ISprite sprite;
        public Vector2 Location { get; set; }
        public bool destroyObject { get; set; }
        private bool faceRight;
        private SpriteFactory spriteFactory;
        public IPhysicsY yPhysics { get; set; }
        public IPhysicsX xPhysics { get; set; }

        private int creationTime;    //Time in seconds of when the Fireball was created
        private int LIFE_TIME = 5; //Number of seconds the fireball is allowed to last before disappearing


        public Fireball(bool faceRight, Vector2 location, SpriteFactory spriteFactory, GameTime gameTime)
        {
            creationTime = gameTime.TotalGameTime.Seconds;
            destroyObject = false;
            this.faceRight = faceRight;
            this.Location = location;
            this.spriteFactory = spriteFactory;
            yPhysics = new BouncyPhysicsY();
            xPhysics = new ItemPhysicsX(faceRight, 2);  //Max velocity of Fireballs in the X direction is 2 pixels

            sprite = spriteFactory.GetSprite("Fireball");
            sprite.Location = Location;
        }

        public void Update(GameTime gameTime)
        {
            int yAdj = yPhysics.MovementAdjustmentY(gameTime, false);    //Parameter passed in here does not matter as the physics do not consider it
            int xAdj = xPhysics.MovementAdjustmentX(gameTime, true, faceRight);  //Fireball is always moving, and therefore the first parameter should always be true

            Location = Vector2.Add(Location, new Vector2(xAdj, yAdj));

            if ((gameTime.TotalGameTime.Seconds - creationTime) >= LIFE_TIME)
            {
                destroyObject = true;
            }

            spriteFactory.GetSprite("Fireball");
            sprite.Location = Location;
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            sprite.Draw(spritebatch);
        }

        public bool IsDrawn()
        {
            return sprite.IsDrawn();
        }

        public void Reset(Vector2 location, GameTime gameTime)
        {
            Location = location;
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            if (isSolid)
            {
                sprite.Location = new Vector2(xPhysics.CollisionAdjustmentX(faceRight, collisionOverlap), 0);
                faceRight = !faceRight;
            }
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            if (isSolid) sprite.Location = new Vector2(0, yPhysics.CollisionAdjustmentY(collisionOverlap));
        }

        public Rectangle GetHitBox()
        {
            return sprite.GetHitBox();
        }

        public void DestroyObject()
        {
            //destroyObject = true;
        }
    }
}
