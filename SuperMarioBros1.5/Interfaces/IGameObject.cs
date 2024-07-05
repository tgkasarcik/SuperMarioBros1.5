using Microsoft.Xna.Framework;
/*
 * Author: Tommy Kasarcik
 */
namespace Sprint5
{
    public interface IGameObject : IUpdateable, IDrawable
    {

        bool destroyObject { get; set; }

        /*
          * Changes/updates object after it has collided on the X axis, <c> faceRight </c> determines what direction the object was facing when it collided
          *      faceRight = collided with object to the right of it
          *      !faceRight = collided with object to the left of it
          */
        void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap);

        /*
         * Changes/updates object after it has collided on the Y axis, <c> above </c> determines what direction the object was moving when it collided
         *      above = collided with object above it
         *      !above = collided with object below it
         */
        void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap);


        /*
         * Return the hit box of <c> this </c> represented as a <c> Rectangle </c> object.
         */
        Rectangle GetHitBox();

        /*
         * Sets object to be destroyed in the next GameObjectManager update call
         */
        void DestroyObject();
    }
}