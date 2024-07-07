namespace Sprint5
{
    public class MarioRightCommand : ICommand
    {
        private IPlayer mario;
        public MarioRightCommand(IPlayer mario)
        {
            this.mario = mario;
        }
        public void Execute()
        {
            mario.Move(true);
        }
    }
}
