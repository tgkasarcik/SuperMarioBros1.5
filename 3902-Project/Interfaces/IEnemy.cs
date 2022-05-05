using Microsoft.Xna.Framework;


namespace Sprint5
{
    public interface IEnemy : IGameObject
    {
        IObjHealth HState { get; set; }
        IXMove XMoveState { get; set; }

        IYMove YMoveState { get; set; }

        /*Let the enemy move*/
        void Move(bool faceRight);
        /*
         * Let the enemy take damage
         */
        void TakeDamage();
    }
}
