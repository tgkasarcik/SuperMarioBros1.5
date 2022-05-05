namespace Sprint5
{
    public class BounceCommand : ICommand
    {
        private CollisionObject collider;
        public BounceCommand(CollisionObject collider)
        {
            this.collider = collider;
        }
        public void Execute()
        {
            ((IPlayer)collider.GameObject).Bounce();
        }
    }
}