using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
    public interface IText : IDrawable
    {
        /*
         * Public Members
         */
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color TextColor { get; set; }

    }
}
