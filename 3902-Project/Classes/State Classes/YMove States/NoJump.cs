using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class PlayerNoJump : YMove
    {                                           

        public PlayerNoJump(IGameObject targetObj, bool jump, bool bounce, IPhysicsY? physicsY = null)
        {
            gameObject = targetObj;
            this.jump = jump;
            this.bounce = bounce;
            if (physicsY == null)
            {
                hadGroundCollision = true;
                this.physicsY = new RigidPhysicsY();
            }
            else
            {
                hadGroundCollision = false;
                this.physicsY = physicsY;
                this.physicsY.AdjustYMovementVal(2);
            }
        }

        public override void Update(GameTime gameTime)
        {
            gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(X_CHANGE, physicsY.MovementAdjustmentY(gameTime, false)));
            if (jump && hadGroundCollision)
            {
                ((Mario)gameObject).YMoveState = new PlayerJump(gameObject, gameTime, jump, bounce, physicsY);
            }
            jump = false;
            bounce = false;
        }
    }

    public class EnemyNoJump : YMove
    {                                           

        public EnemyNoJump(IGameObject targetObj, bool jump, IPhysicsY? physicsY = null)
        {
            gameObject = targetObj;
            this.jump = jump;
            if (physicsY == null)
            {
                hadGroundCollision = true;
                this.physicsY = new RigidPhysicsY();
            }
            else
            {
                hadGroundCollision = false;
                this.physicsY = physicsY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(X_CHANGE, physicsY.MovementAdjustmentY(gameTime, false)));
            if (jump && hadGroundCollision)
            {
                ((IEnemy)gameObject).YMoveState = new EnemyJump(gameObject, gameTime, jump, physicsY);
            }
        }
    }

    public class ItemNoJump : YMove
    {                                           

        public ItemNoJump(IGameObject targetObj, bool isBouncy, IPhysicsY? physicsY = null)
        {
            gameObject = targetObj;
            this.jump = isBouncy;
            if (physicsY == null)
            {
                if (jump) {
                    this.physicsY = new BouncyPhysicsY();
                } else
                {
                    this.physicsY = new RigidPhysicsY();
                }
            }
            else
            {
                this.physicsY = physicsY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(X_CHANGE, physicsY.MovementAdjustmentY(gameTime, jump)));
            if (jump)
            {
                ((Item)gameObject).YMoveState = new ItemJump(gameObject, gameTime, jump, physicsY);
            }
        }
    }
}
