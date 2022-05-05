using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sprint5
{
    public abstract class PlayerMoveX : IXMove
    {
        protected IPhysicsX physicsX;
        protected IGameObject gameObject;
        protected bool moving, faceRight, crouching;
        protected int Y_CHANGE = 0;   //Distance changed in the Y direction is 0 in the XMove class
        protected IGameObject marioObj;

        public int CollisionAdjustment(bool isRight, Rectangle collisionOverlap)
        {
            return physicsX.CollisionAdjustmentX(isRight, collisionOverlap);
        }

        /*
         * Changes the truth of the crouch boolean to true as player should be crouching while this function is called
         */
        public void Crouch()
        {
            crouching = true;
        }

        public List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();

            string keyVal = "Left"; //Assume player is facing left to start
            if (faceRight)
            {
                keyVal = "Right";   //Facing right was in fact the truth, so change it to face right
            }

            keys.Add(keyVal);

            if (crouching)
            {
                keys.Add("Crouch"); //Player found crouching, will add crouching part on the end
            }
            if (moving)
            {
                keys.Add("Move");   //Player instead found moving, will add moving part on the end instead
            } else
            {
                keys.Add("Still");  //Player wwasn't found to be moving or crouching, so a default for still will be added on the end instead
            }

            return keys;
        }

        /*
         * Returns the truth value of crouching
         *      Returns true if crouching, false otherwise
         */
        public bool IsCrouching()
        {
            return crouching;
        }

        public void Movement(bool faceRight)
        {
            this.moving = true;
            this.faceRight = faceRight;
        }

        public bool FaceRight()
        {
            return faceRight;
        }

        public void SetMoving(bool move)
        {
            moving = move;
        }

        public bool Moving()
        {
            return moving;
        }

        public abstract void Update(GameTime gametime);
    }

    public abstract class ItemEnemyMoveX : IXMove
    {
        protected IPhysicsX physicsX;
        protected IGameObject gameObject;
        protected bool moving, faceRight;
        protected int Y_CHANGE = 0;   //Distance changed in the Y direction is 0 in the XMove class

        public int CollisionAdjustment(bool isRight, Rectangle collisionOverlap)
        {
            return physicsX.CollisionAdjustmentX(isRight, collisionOverlap);
        }

        public List<string> GetSpriteKey()
        {
            throw new NotImplementedException();
        }

        public void Movement(bool faceRight)
        {
            this.moving = true;
            this.faceRight = faceRight;
        }

        public bool FaceRight()
        {
            return faceRight;
        }

        public void SetMoving(bool move)
        {
            moving = move;
        }

        public bool Moving()
        {
            return moving;
        }

        public abstract void Update(GameTime gametime);
    }
}
