using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

namespace Sprint5
{
    public class Shell : IEnemy, ICollideable, ISimpleMovementType
    {
        private ISprite Sprite { get; set; }
        public Vector2 Location { get; set; }
        public IObjHealth HState { get; set; }
        public IXMove XMoveState { get; set; }
        public IYMove YMoveState { get; set; }
        public IPhysicsY yPhysics { get; set; }
        public IPhysicsX xPhysics { get; set; }
        public bool destroyObject { get; set; }

        private SpriteFactory SpriteFactory;
        private static int ZERO_CHANGE = 0;
        private bool faceRight;
        public bool moving;
        private static double SHELL_BASE_X_VEL = 3.0;   //The base velocity of the shell

        public Shell(Vector2 location, bool moving, bool faceRight, bool jump, bool floored, SpriteFactory spritefactory)
        {
            this.faceRight = faceRight;
            Location = location;
            destroyObject = false;
            this.moving = false;
            XMoveState = new EnemyNoMoveLeft(this, false, faceRight, null, SHELL_BASE_X_VEL);
            HState = new EnemyAlive(this);
            YMoveState = new EnemyNoJump(this, jump);
            SpriteFactory = spritefactory;
            Sprite = SpriteFactory.GetSprite("ShellGreenStill");
            Sprite.Location = location;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Sprite.Draw(spritebatch);
        }

        public bool IsDrawn()
        {
            return Sprite.IsDrawn();
        }

        public void Move(bool FaceRight)
        {
            this.faceRight = FaceRight;
            XMoveState.Movement(FaceRight);
        }

        public void Kick(bool kickRight)
        {
            if (!moving)
            {
                SoundFactory.Instance.GetSoundEffect("KickShell").Play();
                Sprite = SpriteFactory.GetSprite("ShellGreenMove");
                Move(kickRight);
                moving = true;
                Sprite.Location = Location;
            }
        }

        public void TakeDamage()
        {
            if (moving)
            {
                Sprite = SpriteFactory.GetSprite("ShellGreenStill");
                XMoveState.SetMoving(false);
                moving = false;
                Sprite.Location = Location;
            }
        }


        public void Reset(Vector2 location, GameTime gameTime)
        {
            Location = location;
            Sprite.Location = location;
        }

        public void Update(GameTime gametime)
        {
            if (moving) Move((bool)faceRight);
            XMoveState.Update(gametime);
            YMoveState.Update(gametime);
            HState.Update(gametime);
            Sprite.Location = Location;
            Sprite.Update(gametime);
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            if (isSolid) Location = Vector2.Add(Location, new Vector2(XMoveState.CollisionAdjustment(isRight, collisionOverlap), 0));
            if (isRight)
            {
                faceRight = false;
            } else
            {
                faceRight = true;
            }
            Sprite.Location = Location;
            //faceRight = !faceRight;
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
