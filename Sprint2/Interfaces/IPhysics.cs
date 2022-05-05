using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    //NOTE: Blocks are 16 pixels wide, so a single movement update should not be more than 8 pixels at a time
    public interface IPhysicsX
    {
        /*
         * Will return the amount to change an object's X position by, on screen, based on <> movingRight <> and gameTime
         *      if !moving, then any velocity that object may have had is reduced towards 0 over time
         *      return = positive iff movingRight (Will move position right on screen)
         *      return = negative iff !movingRight (Will move position left on screen)
         */
        int MovementAdjustmentX(GameTime gameTime, bool moving, bool faceRight);


        /*
        * This will be called when a collision in the X axis occurs and will adjust velocity/acceleration accordingly
        *       Will add or subtract the X overlap between the collider object and the collided object from the <> rectangle <> X value
        *       returns colliderXLoc - pixelOverlap if faceRight
        *       returns colliderXLoc + pixelOverlap if !faceRight
        */
        int CollisionAdjustmentX(bool faceRight, Rectangle rectangle);
    }

    public interface IPhysicsY
    {

        /*
         * Will return the amount to change an object's Y position by, on screen, based on <> falling <>
         *      if falling, then any negative velocity that object may have had is accelerated towards a positive value over time
         *      return = positive iff falling (Will lower position on screen)
         *      return = negative iff !falling (Will heighten position on screen)
         */
        int MovementAdjustmentY(GameTime gameTime, bool jumping);

        /*
        * This will be called when a collision in the Y axis occurs and will adjust velocity/acceleration accordingly
        *       Will add or subtract the Y overlap between the collider object and the collided object from <> rectangle <> Y value
        *       returns colliderYLoc - pixelOverlap if falling
        *       returns colliderYLoc + pixelOverlap if !falling
        */
        int CollisionAdjustmentY(Rectangle rectangle);

        /*
         * Sets the value of y movement changes to some value, all definitions of this function should set newVal to a constant so that the value can be reset if needed
         * by calling the function with no parameters
         */
        void AdjustYMovementVal(double newVal);
    }
}
