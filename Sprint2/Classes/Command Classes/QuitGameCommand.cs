namespace Sprint5
{
    public class QuitGameCommand : ICommand
    {
        private Game1 game;
        public QuitGameCommand(Game1 game)
        {
            this.game = game;
        }
        public void Execute()
        {
            game.Exit();
        }
    }
}
