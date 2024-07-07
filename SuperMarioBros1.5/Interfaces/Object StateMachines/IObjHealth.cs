namespace Sprint5
{
    public interface IObjHealth : IUpdateable, ISpriteKey
    {
        /*
         * Function used to change from a higher health/power state to a lower one such as SmallMario or BigMario (Will call IVulnerability TakeDamage function)
         */
        void TakeDamage();

        /*
         * Returns truth as to whether the object is dead or not
         */
        bool IsDead();
    }
}
