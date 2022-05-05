namespace Sprint5
{
    public class MarioEnterLeftPipeCommand : ICommand
    {

        private CollisionObject collider, collided;
        private GameState gameState;
        public MarioEnterLeftPipeCommand(CollisionObject collider, CollisionObject collided, GameState gameState)
        {
            this.collider = collider;
            this.collided = collided;
            this.gameState = gameState;
        }
        public void Execute()
        {
            if (((Pipe)collided.GameObject).pipeType == PipeType.LeftPipe)
            {
                ((Pipe)collided.GameObject).PlayerInteract(((Mario)collider.GameObject),gameState.GameTime);
            }
        }
    }
}