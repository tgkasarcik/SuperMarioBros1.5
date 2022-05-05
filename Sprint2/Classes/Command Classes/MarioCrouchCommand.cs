using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    public class MarioCrouchCommand : ICommand
    {
        private IPlayer mario;
        public MarioCrouchCommand(IPlayer mario)
        {
            this.mario = mario;
        }
        public void Execute()
        {
            ((Mario)mario).Crouch();
        }
    }
}

