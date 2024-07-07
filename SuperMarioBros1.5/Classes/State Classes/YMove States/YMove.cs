using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    public abstract class YMove : IYMove
    {
        protected IPhysicsY physicsY;
        protected IGameObject gameObject;
        protected bool jump;
        protected bool bounce;
        protected bool hadGroundCollision;
        protected int X_CHANGE = 0;   //Distance changed in the X direction is 0 in the YMove class

        public int CollisionAdjustment(bool isUp, Rectangle collisionOverlap)
        {
            if (!isUp)
            {
                hadGroundCollision = true;
            } else
            {
                jump = false;
            }
            
            return physicsY.CollisionAdjustmentY(collisionOverlap);
        }

        public List<string> GetSpriteKey()
        {
            //Should only provide a key part if object is Jumping
            List<string> keys = new List<string>();
            if (jump) keys.Add("Jump");
            return keys;
            
        }

        public void Jump()
        {
            this.jump = true;
            this.bounce = false;
        }

        public void Bounce()
        {
            this.jump = true;
            this.bounce = true;
        }

        public void Bounce(int maxHeight)
        {
            physicsY.AdjustYMovementVal(maxHeight);
            this.jump = true;
            this.bounce = true;
        }

        public abstract void Update(GameTime gametime);
    }
}
