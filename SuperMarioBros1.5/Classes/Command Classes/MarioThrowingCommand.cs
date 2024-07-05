namespace Sprint5
{

    public class MarioThrowingCommand : ICommand
    {
        IPlayer mario;
        GameState gs;
        public MarioThrowingCommand(IPlayer mario, GameState gs)
        {
            this.mario = mario;
            this.gs = gs;
        }
        public void Execute()
        {
            mario.Fireball(gs.GameTime);
        }
    }
}
