using Microsoft.Xna.Framework;

namespace Sprint5
{
    public interface IUseability : IUpdateable, ISpriteKey
    {
        /*
         * Function used to change the useability state of object (states include Solid, Interactive, Used, Broken, and Destroyed)
         * and will also perform any action that is necessary for a block to have
         */
        void ChangeUseability(GameTime gameTime);

        /*
         * Returns a boolean value based on the state of the object to determine if the object is interactive
         */
        bool IsInteractive();
    }
}
