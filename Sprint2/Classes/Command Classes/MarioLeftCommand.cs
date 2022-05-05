namespace Sprint5
{
    public class MarioLeftCommand : ICommand
    {
        private IPlayer mario;
        public MarioLeftCommand(IPlayer mario)
        {
            this.mario = mario;
        }
        public void Execute()
        {
            mario.Move(false);
        }
    }
}
