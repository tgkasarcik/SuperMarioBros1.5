using System.Diagnostics;

namespace Sprint5
{
    public class BlockBumpCommand : ICommand
    {
        private CollisionObject collided, collider;
        private GameState gameState;
        public BlockBumpCommand(CollisionObject collider, CollisionObject collided, GameState gameState)
        {
            this.collider = collider;
            this.collided = collided;
            this.gameState = gameState;
        }
        public void Execute()
        {
            ((Block)collided.GameObject).PlayerInteract(((Mario)collider.GameObject), gameState.GameTime);
            ((Block)collided.GameObject).CollideY(true, true, new Microsoft.Xna.Framework.Rectangle());
        }
    }
}