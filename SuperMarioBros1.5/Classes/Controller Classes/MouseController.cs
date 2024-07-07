using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint5
{
    class MouseController : IMouseController, IUpdateable
    {
        private Game1 game;
        private Dictionary<Rectangle, ICommand> leftMousePressBindings, rightMousePressBindings;
        private int screenWidth, screenHeight;
        private MouseState previousMouseState;
        private GameState gs;

        public MouseController(Game1 game, GameState gs)
        {
            this.game = game;
            this.gs = gs;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            gs.AddMouse(this);
            ResetCommandBindings();                       

        }

        public void RegisterPressCommand(Rectangle position, ICommand command, bool leftMouse)
        {
            if (leftMouse)
            {
                leftMousePressBindings.Add(position, command);
            } else
            {
                rightMousePressBindings.Add(position, command);
            }
            
        }

        private bool RightHasBeenPressed()
        {
            return Mouse.GetState().RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }

        private bool LeftHasBeenPressed()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }

        public void ResetCommandBindings()
        {
            leftMousePressBindings = new Dictionary<Rectangle, ICommand>();
            rightMousePressBindings = new Dictionary<Rectangle, ICommand>();

            // Register necessary commands.
            RegisterPressCommand(new Rectangle(0, 0, screenWidth, screenHeight), new NextLevelCommand(gs), false);
            RegisterPressCommand(new Rectangle(0, 0, screenWidth, screenHeight), new NextLevelCommand(gs), true);
        }


        public void Update(GameTime gt)
        {
            // Get mouse state.
            MouseState state = Mouse.GetState();
            var mousePoint = new Point(state.X, state.Y);

            // If left button is clicked, find the quadrant clicked and execute that command.
            foreach (var binding in leftMousePressBindings)
            {
                if (LeftHasBeenPressed() && binding.Key.Contains(mousePoint))
                {
                    binding.Value.Execute();
                }
            }

            // If right button is clicked, find the quadrant clicked and execute that command.
            foreach (var binding in rightMousePressBindings)
            {
                if (RightHasBeenPressed() && binding.Key.Contains(mousePoint))
                {
                    binding.Value.Execute();
                }
            }

            previousMouseState = state;

        }
    }
}
