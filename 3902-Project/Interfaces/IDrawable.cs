using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
    public interface IDrawable
    {

        /*
         * <c> Vector2 </c> object to hold <c> this </c>'s on screen position. 
         */
        public Vector2 Location { get; set; }

        /*
         * Draw <c> this </c> to the specified <c> SpriteBatch </c> at <c> this </c>'s location.
         * <requires>
         *      spritebatch.Begin() must be called before this method is invoked.
         * </requires>
         */
        void Draw(SpriteBatch spritebatch);

        /*
         * Reports whether <c> this </c> should be drawn to the screen or not. 
         */
        bool IsDrawn();
    }
}