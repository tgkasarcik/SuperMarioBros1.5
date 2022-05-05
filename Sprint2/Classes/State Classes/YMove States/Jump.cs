using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint5
{
    public class PlayerJump : YMove
    {
        //The max amount of time the player can jump is for 3 seconds
        private static double MAX_JUMP_TIME = 0.5;
        //The minimum amount of time the player is jumping
        private static double MIN_JUMP_TIME = 0.15;
        //When did the jump start?
        private double jumpStart;

        public PlayerJump(IGameObject targetObj, GameTime gameTime, bool jump, bool bounce, IPhysicsY? physicsY = null)
        {
            hadGroundCollision = false;
            gameObject = targetObj;
            this.jump = jump;
            this.bounce = bounce;
            jumpStart = gameTime.TotalGameTime.TotalSeconds;

            List<string> keys = ((IPlayer)targetObj).HState.GetSpriteKey();
            if (jump && !bounce) {
                string sound;
                if (keys[0].Contains("Small"))
                {
                    sound = "SmallMarioJump";

                } else
                {
                    sound = "BigMarioJump";

                }
                SoundFactory.Instance.GetSoundEffect(sound).Play();
            } else
            {
                SoundFactory.Instance.GetSoundEffect("Stomp").Play();
            }

            if (physicsY == null)
            {
                this.physicsY = new RigidPhysicsY();
            }
            else
            {
                this.physicsY = physicsY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.jump && (MAX_JUMP_TIME.CompareTo(gameTime.TotalGameTime.TotalSeconds - jumpStart) > 0))
            {
                gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(X_CHANGE, physicsY.MovementAdjustmentY(gameTime, jump)));
            }
            else
            {
                ((Mario)gameObject).YMoveState = new PlayerNoJump(gameObject, false, bounce, physicsY);
            }
            if ((MIN_JUMP_TIME.CompareTo(gameTime.TotalGameTime.TotalSeconds - jumpStart) < 0)) {
                jump = false;
                bounce = false;
            }
        }
    }

    public class EnemyJump : YMove
    {
        //The max amount of time an enemy can jump is for 1 seconds
        private static double BASEJUMPTIME = 1;
        //When did the jump start?
        private double jumpStart;

        public EnemyJump(IGameObject targetObj, GameTime gameTime, bool jump, IPhysicsY? physicsY = null)
        {
            gameObject = targetObj;
            this.jump = jump;
            jumpStart = gameTime.ElapsedGameTime.TotalSeconds;
            if (physicsY == null)
            {
                this.physicsY = new RigidPhysicsY();
            }
            else
            {
                this.physicsY = physicsY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.jump && ((gameTime.ElapsedGameTime.TotalSeconds - jumpStart) < BASEJUMPTIME))
            {
                gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(X_CHANGE, physicsY.MovementAdjustmentY(gameTime, jump)));
            }
            else
            {
                ((IEnemy)gameObject).YMoveState = new EnemyNoJump(gameObject, jump, physicsY);
            }
            jump = false;
        }
    }

    public class ItemJump : YMove
    {
        //The max amount of time an item can jump is for 1 seconds
        private static double BASEJUMPTIME = 1;
        //When did the jump start?
        private double jumpStart;

        public ItemJump(IGameObject targetObj, GameTime gameTime, bool isBouncy, IPhysicsY? physicsY = null)
        {
            gameObject = targetObj;
            this.jump = isBouncy;
            jumpStart = gameTime.ElapsedGameTime.TotalSeconds;
            if (physicsY == null)
            {
                if (jump)
                {
                    this.physicsY = new BouncyPhysicsY();
                }
                else
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
            if (this.jump && ((gameTime.ElapsedGameTime.TotalSeconds - jumpStart) < BASEJUMPTIME))
            {
                gameObject.Location = Vector2.Add(gameObject.Location, new Vector2(X_CHANGE, physicsY.MovementAdjustmentY(gameTime, jump)));
            }
            else
            {
                jump = false;
                ((Item)gameObject).YMoveState = new EnemyNoJump(gameObject, jump, physicsY);
            }
        }
    }
}
