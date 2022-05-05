using Microsoft.Xna.Framework;

namespace Sprint5
{
    public interface IProjectile
    {
        /*
         * Changes state of projectile to "Moving"
         */
        void Move();
        /*
         * Get the rectangle for the projectile and return the position
         */
        Rectangle GetPosition();
    }
}
