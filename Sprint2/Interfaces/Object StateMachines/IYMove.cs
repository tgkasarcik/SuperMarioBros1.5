using Microsoft.Xna.Framework;

namespace Sprint5
{
    public interface IYMove : IUpdateable, ISpriteKey
    {

        /*
         * Function to set object to Jumping
         */
        int CollisionAdjustment(bool isUp, Rectangle collisionOverlap);
        void Jump();

        /*
         * Called when the object bounces rather than jumps
         */
        void Bounce();
        void Bounce(int maxHeight);
    }
}
