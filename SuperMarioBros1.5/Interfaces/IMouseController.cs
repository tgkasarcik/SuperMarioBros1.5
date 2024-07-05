using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    public interface IMouseController
    {
        /*
         * Registers a command for a specific input. If leftMouse is true, it is a command for a left mouse click,
         * if false, it's a command for a right mouse click.
         */
        void RegisterPressCommand(Rectangle position, ICommand command, bool leftMouse);

        /* 
         * Check for user input and react accordingly for different inputs.
         */
        void Update(GameTime gt);

        void ResetCommandBindings();
    }
}
