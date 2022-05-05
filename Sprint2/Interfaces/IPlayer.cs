using Microsoft.Xna.Framework;
using System;

namespace Sprint5
{
    public interface IPlayer : IGameObject
    {
        public enum AnimationType
        {
            None,
            Flag,
            DownPipe,
            UpPipe,
            LeftPipe,
            RightPipe
        }

        public enum CardinalDir
        {
            Left,
            Right,
            Down,
            Up
        }

        IPlayerHealth HState { get; set; }
        IVulnerability VState { get; set; }

        IXMove XMoveState { get; set; }

        IYMove YMoveState { get; set; }

        /*
         * Operate process for <c> this </c> taking damage
         */
        void TakeDamage(GameTime gameTime);
        /*
         * Call for change in <c> this </c> needing a specific animation
         */
        void AnimationLock(bool lockTruth);
        /*
         * Operate process for <c> this </c> moving
         */
        void Move(bool faceRight);
        /*
         * Operate process for <c> this </c> jumping
         */
        void Jump();
        /*
         * Operate process for <c> this </c> bouncing
         */
        void Bounce();
        /*
         * Report if <c> this </c> is able to throw a fireball
         */
        void Fireball(GameTime gameTime);
        /*
         * Operate process for picking up a power such as a Star, Mushroom, or Fireflower
         */
        void PickupPower(PowerType power, GameTime gameTime);

        /*
         * Function to call for placing blocks by player
         */
        void PlaceBlock(CardinalDir dir);
    }
}
