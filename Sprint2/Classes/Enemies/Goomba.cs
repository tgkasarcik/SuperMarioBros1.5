using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5
{
    public class Goomba : IEnemy, ISimpleMovementType, IUpdateable
    {
        public IXMove XMoveState { get; set; }
        public IObjHealth HState { get; set; }
        public IYMove YMoveState { get; set; }
        public IPhysicsY yPhysics { get; set; }
        public IPhysicsX xPhysics { get; set; }
        private ISprite Sprite;
        private static SpriteFactory SpriteFactory;

        private bool FaceRight;
        public Vector2 Location { get; set; }
        public bool destroyObject { get; set; }

        public Goomba(Vector2 position, bool moving, bool faceRight, bool jump, bool floored, SpriteFactory spriteFactory)
        {
            FaceRight = faceRight;
            Location = position;
            destroyObject = false;
            XMoveState = new EnemyMoveLeft(this, moving, faceRight, null, 1.0);
            HState = new EnemyAlive(this);
            YMoveState = new EnemyNoJump(this, jump);
            SpriteFactory = spriteFactory;
            Sprite = spriteFactory.GetSprite("GoombaMove");
            Sprite.Location = position;

        }

        public void TakeDamage()
        {
            if (!HState.IsDead()) {
                HState.TakeDamage();
                destroyObject = true;
                Sprite = SpriteFactory.GetSprite("GoombaDead");
                Sprite.Location = Location;
                TextFactory.Instance.CreateTimedText(HUD.Instance.KillGoomba().ToString(), Location);
            }
        }
        public void Move(bool FaceRight)
        {
            XMoveState.Movement(FaceRight);                   
        }
        public void Update(GameTime gametime)
        {
            Move(FaceRight);
            XMoveState.Update(gametime);
            YMoveState.Update(gametime);
            HState.Update(gametime);
            Sprite.Location = Location;
            Sprite.Update(gametime);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            Sprite.Draw(spritebatch);
        }

        public bool IsDrawn()
        {
            return Sprite.IsDrawn();
        }

        public void Reset(Vector2 location, GameTime gameTime)
        {
            Location = location;
            Sprite.Location = location;
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            if (isSolid)
            {
                Location = Vector2.Add(Location, new Vector2(XMoveState.CollisionAdjustment(isRight, collisionOverlap), 0));
                Sprite.Location = Location;
            }
            if (isRight)
            {
                FaceRight = false;
            }
            else if (!isRight)
            {
                FaceRight = true;
            }
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            if (isSolid)
            {
                Location = Vector2.Add(Location, new Vector2(0, YMoveState.CollisionAdjustment(isUp, collisionOverlap)));
                Sprite.Location = Location;
            }


        }

        public Rectangle GetHitBox()
        {
            return Sprite.GetHitBox();
        }

        public void DestroyObject()
        {
            destroyObject = true;
        }
    }
}
