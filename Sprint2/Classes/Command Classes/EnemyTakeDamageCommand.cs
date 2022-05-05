using System.Diagnostics;

namespace Sprint5
{
    public class EnemyTakeDamageCommand : ICommand
    {
        private CollisionObject enemy;
        public EnemyTakeDamageCommand(CollisionObject enemy, GameState gameState)
        {
            this.enemy = enemy;
        }
        public void Execute()
        {
            ((IEnemy)enemy.GameObject).TakeDamage();
        }
    }
}