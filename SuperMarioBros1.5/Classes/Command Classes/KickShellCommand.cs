namespace Sprint5
{
    public class KickShellCommand : ICommand
    {
        private CollisionObject collided;
        private bool kickRight;
        public KickShellCommand(CollisionObject collided, bool kickRight)
        {
            this.collided = collided;
            this.kickRight = kickRight;
        }
        public void Execute()
        {
            ((Shell)collided.GameObject).Kick(kickRight);
        }
    }
}