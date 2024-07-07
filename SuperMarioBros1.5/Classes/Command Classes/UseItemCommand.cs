using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    class UseItemCommand : ICommand
    {
        private IPlayer mario;
        private GameState gs;
        private PowerType power;
        public UseItemCommand(IPlayer mario, GameState gs, PowerType power)
        {
            this.mario = mario;
            this.gs = gs;
            this.power = power;

        }
        public void Execute()
        {
            mario.PickupPower(power, gs.GameTime);
        }
    }
}

