namespace Sprint5
{
    public class MarioEnterTopPipeCommand : ICommand
    {

        private CollisionObject collider, collided;
        private GameState gameState;
        public MarioEnterTopPipeCommand(CollisionObject collider, CollisionObject collided, GameState gameState)
        {
            this.collider = collider;
            this.collided = collided;
            this.gameState = gameState;
        }
        public void Execute()
        {
            if (((Pipe)collided.GameObject).pipeType == PipeType.UpPipe && ((PlayerMoveX)((Mario)collider.GameObject).XMoveState).IsCrouching())
            {
                ((Pipe)collided.GameObject).PlayerInteract(((Mario)collider.GameObject), gameState.GameTime);
            }
        }
    }
}