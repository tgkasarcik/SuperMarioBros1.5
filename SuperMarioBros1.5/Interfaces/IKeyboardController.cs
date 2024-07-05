using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint5
{
    public interface IKeyboardController
    {
        /*
         * Registers a command for a specific input. This method will register commands that should only execute 
         * once per key/mouse press and release.
         */
        void RegisterPressCommand(Keys key, ICommand command);


        /*
        * Registers a command for a specific input. This method will register commands that continue to execute if key/mouse is down.
        */
        void RegisterHoldCommand(Keys key, ICommand command);

        /* 
         * Check for user input and react accordingly for different inputs.
         */
        void Update(GameTime gt);

        void ResetCommandBindings();
    }
}