namespace Sprint5
{
    public class MarioEnterRightPipeCommand : ICommand
    {

        private CollisionObject collider, collided;
        private GameState gameState;
        public MarioEnterRightPipeCommand(CollisionObject collider, CollisionObject collided, GameState gameState)
        {
            this.collider = collider;
            this.collided = collided;
            this.gameState = gameState;
        }
        public void Execute()
        {
            if (((Pipe)collided.GameObject).pipeType == PipeType.RightPipe)
            {
                ((Pipe)collided.GameObject).PlayerInteract(((Mario)collider.GameObject), gameState.GameTime);
            }
        }
    }
}