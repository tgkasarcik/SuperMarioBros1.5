using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    class PlayerMoveRight : PlayerMoveX
    {

        public PlayerMoveRight(IGameObject targetObj, bool moving, bool faceRight, bool crouching, IPhysicsX? physicsX = null)
        {
            gameObject = targetObj;
            this.moving = moving;
            this.faceRight = true;
            this.crouching = crouching;
            if (physicsX == null)
            {
                this.physicsX = new PlayerPhysicsX();
            }
            else
            {
                this.physicsX = physicsX;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.moving && !this.crouching)
            {
                if (faceRight)
                {
                    gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(physicsX.MovementAdjustmentX(gameTime, moving, faceRight), Y_CHANGE));
                }
                else
                {
                    ((Mario)gameObject).XMoveState = new PlayerMoveLeft(gameObject, true, false, crouching, physicsX);
                }
            }
            else
            {
                if (!faceRight)
                {
                    ((Mario)gameObject).XMoveState = new PlayerNoMoveLeft(gameObject, false, false, crouching, physicsX);
                }
                else
                {
                    ((Mario)gameObject).XMoveState = new PlayerNoMoveRight(gameObject, false, true, crouching, physicsX);
                }
            }
            this.moving = false;
            this.crouching = false;
        }
    }

    class EnemyMoveRight : ItemEnemyMoveX
    {

        public EnemyMoveRight(IGameObject targetObj, bool moving, bool faceRight, IPhysicsX? physicsX = null, double maxVelocity = 1.0)
        {
            gameObject = targetObj;
            this.moving = moving;
            this.faceRight = faceRight;
            if (physicsX == null)
            {
                this.physicsX = new EnemyPhysicsX(maxVelocity);
            }
            else
            {
                this.physicsX = physicsX;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.moving)
            {
                bool foundEdge = true;
                int posX = (int)gameObject.Location.X / Game1.BLOCK_WIDTH;
                posX++;

                int yExtraPixels = (int)gameObject.Location.Y % Game1.BLOCK_WIDTH;
                if (gameObject.Location.Y < 0) yExtraPixels *= -1;
                int posY = ((int)gameObject.Location.Y + yExtraPixels) / Game1.BLOCK_WIDTH;
                posY += 1;

                foreach (var block in GameObjectManager.Instance.StructureList[posX])
                {
                    Vector2 loc = block.Location;
                    if (((int)loc.X / Game1.BLOCK_WIDTH) == posX && ((int)loc.Y / Game1.BLOCK_WIDTH) == posY)
                    {
                        foundEdge = false;
                        break;
                    }
                }

                if (foundEdge)
                {
                    gameObject.CollideX(true, true, new Rectangle(0, 0, 0, 0));
                }



                if (faceRight)
                {
                    gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(physicsX.MovementAdjustmentX(gameTime, moving, faceRight), Y_CHANGE));
                }
                else
                {
                    ((IEnemy)gameObject).XMoveState = new EnemyMoveLeft(gameObject, true, false, physicsX);
                }
            }
            else
            {
                if (!faceRight)
                {
                    ((IEnemy)gameObject).XMoveState = new EnemyNoMoveLeft(gameObject, false, false, physicsX);
                }
                else
                {
                    ((IEnemy)gameObject).XMoveState = new EnemyNoMoveRight(gameObject, false, true, physicsX);
                }
            }
        }
    }

    class ItemMoveRight : ItemEnemyMoveX
    {

        public ItemMoveRight(IGameObject targetObj, bool moving, bool faceRight, IPhysicsX? physicsX = null)
        {
            gameObject = targetObj;
            this.moving = moving;
            this.faceRight = faceRight;
            if (physicsX == null)
            {
                this.physicsX = new ItemPhysicsX();
            }
            else
            {
                this.physicsX = physicsX;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.moving)
            {
                if (faceRight)
                {
                    gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(physicsX.MovementAdjustmentX(gameTime, moving, faceRight), Y_CHANGE));
                }
                else
                {
                    ((Item)gameObject).XMoveState = new ItemMoveLeft(gameObject, false, faceRight, physicsX);
                }
            }
            else
            {
                if (!faceRight)
                {
                    ((Item)gameObject).XMoveState = new ItemNoMoveLeft(gameObject, false, false, physicsX);
                }
                else
                {
                    ((Item)gameObject).XMoveState = new ItemNoMoveRight(gameObject, false, true, physicsX);
                }
            }
        }
    }
}
