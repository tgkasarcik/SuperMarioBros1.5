using Microsoft.Xna.Framework;

namespace Sprint5
{
    public interface IXMove : IUpdateable, ISpriteKey
    {

        /*
         * Will change the facing and moving states of an object, the only outside info it needs is if the left or right key is being pressed (represented by the faceRight parameter)
         * Will change object state to Moving no matter what if called
         */
        int CollisionAdjustment(bool isRight, Rectangle collisionOverlap);
        void Movement(bool faceRight);
        //Returns whether the object is facing right or not
        bool FaceRight();
        //Returns whether object is moving or not
        bool Moving();
        //Flips the value of moving stored in the state
        void SetMoving(bool move);
    }
}
