using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
    public interface ISprite : IDrawable, IUpdateable
    {
        /*
         * Public Members
         */
        public Texture2D Texture { get; set; }

        /*
         * Return the hit box of <c> this </c>.
         */
        Rectangle GetHitBox();

        /*
         * Toggle Star Overlay for <c> this </c>.
         */
        void ToggleStar();

        /*
         * Toggle Damaged Overlay for <c> this </c>.
         */
        void ToggleDamaged();

    }
}
