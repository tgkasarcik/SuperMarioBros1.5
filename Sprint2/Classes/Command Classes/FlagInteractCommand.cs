namespace Sprint5
{
    public class FlagInteractCommand : ICommand
    {
        private CollisionObject collider, collided;
        private GameState gameState;
        public FlagInteractCommand(CollisionObject collider, CollisionObject collided, GameState gameState)
        {
            this.gameState = gameState;
            this.collider = collider;
            this.collided = collided;
        }
        public void Execute()
        {
            ((Flag)collided.GameObject).PlayerInteract(((Mario)collider.GameObject), gameState.GameTime);
        }
    }
}