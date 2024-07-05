namespace Sprint5
{
    public class MarioJumpCommand : ICommand
    {
        private IPlayer mario;
        public MarioJumpCommand(IPlayer mario)
        {
            this.mario = mario;
        }
        public void Execute()
        {
            mario.Jump();
        }
    }
}
