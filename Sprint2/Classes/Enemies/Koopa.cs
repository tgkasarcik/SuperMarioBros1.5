using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Sprint5
{
    public class Koopa : IEnemy, ISimpleMovementType, IUpdateable
    {
        public IXMove XMoveState { get; set; }
        public IObjHealth HState { get; set; }
        public IYMove YMoveState { get; set; }
        public IPhysicsY yPhysics { get; set; }
        public IPhysicsX xPhysics { get; set; }
        private ISprite Sprite;
        private static SpriteFactory SpriteFactory;

        public bool faceRight;
        public Vector2 Location { get; set; }
        public bool destroyObject { get; set; }

        public Koopa(Vector2 position, bool moving, bool faceRight, bool jump, bool floored, SpriteFactory sprite)
        {
            Location = position;
            destroyObject = false;
            XMoveState = new EnemyMoveLeft(this, moving, faceRight, null, 1.0);
            HState = new EnemyAlive(this);
            YMoveState = new EnemyNoJump(this, jump);
            SpriteFactory = sprite;
            this.faceRight = faceRight;
            if (faceRight)
            {
                Sprite = SpriteFactory.GetSprite("Koopa", "Right", "Move");
            } else
            {
                Sprite = SpriteFactory.GetSprite("Koopa", "Left", "Move");
            }
            Sprite.Location = Location;
        }

        public void TakeDamage()
        {
            HState.TakeDamage();
            GameObjectManager.Instance.EnemyList.Add(new Shell(new Vector2(Location.X, Location.Y + 8), false, true, false, true, SpriteFactory));
            destroyObject = true;
            TextFactory.Instance.CreateTimedText(HUD.Instance.KillKoopa().ToString(), Location);
        }

        public void Move(bool FaceRight)
        {
            XMoveState.Movement(FaceRight);
        }
        public void Update(GameTime gametime)
        {
            Move(faceRight);
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
            }
            if (isRight)
            {
                Sprite = SpriteFactory.GetSprite("Koopa", "Left", "Move");
                faceRight = false;
            }
            else if (!isRight)
            {
                Sprite = SpriteFactory.GetSprite("Koopa", "Right", "Move");
                faceRight = true;
            }
            Sprite.Location = Location;
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
