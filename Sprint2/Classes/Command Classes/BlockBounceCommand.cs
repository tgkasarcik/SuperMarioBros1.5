namespace Sprint5
{
    public class BlockBounceCommand : ICommand
    {
        private CollisionObject collided, collider;
        private GameState gameState;
        public BlockBounceCommand(CollisionObject collider, CollisionObject collided, GameState gameState)
        {
            this.collider = collider;
            this.collided = collided;
            this.gameState = gameState;
        }
        public void Execute()
        {
            if(((Block)collided.GameObject).Type == BlockType.BounceBlock)
            {
                ((Mario)collider.GameObject).Bounce(5);
            }
        }
    }
}