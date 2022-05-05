using System.Diagnostics;

namespace Sprint5
{
    public class PlaceBlockCommand : ICommand
    {
        IPlayer.CardinalDir direction;
        IPlayer mario;
        public PlaceBlockCommand(IPlayer mario, IPlayer.CardinalDir direction)
        {
            this.direction = direction;
            this.mario = mario;
        }
        public void Execute()
        {
            mario.PlaceBlock(direction);
        }
    }
}