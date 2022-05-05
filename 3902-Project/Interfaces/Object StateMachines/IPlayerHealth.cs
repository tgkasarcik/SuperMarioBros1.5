using Microsoft.Xna.Framework;

namespace Sprint5
{
    public interface IPlayerHealth : IObjHealth
    {
        /*
         * Function used to change from a lower health/power state to a higher one such as BigMario, FireMario, and StarMario
         */
        void PickupPower(PowerType power, GameTime gameTime);

        /*
         * Function used to return a a Fireball when called
         */
        IGameObject ThrowFireBall(bool faceRight, Vector2 Location, SpriteFactory spriteFactory, GameTime gameTime);

        /*
         * Returns truth value of whether Player cna throw an object or not
         */
        bool CanThrow(GameTime gameTime);

        /*
         * Returns the truth value for if the player has the star power active
         *      Returns true if hasStar, false otherwise
         */
        bool HasStar();
    }
}
