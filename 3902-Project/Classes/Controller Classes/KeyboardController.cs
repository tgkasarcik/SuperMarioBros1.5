using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint5
{
    class KeyboardController : IKeyboardController, IUpdateable
    {
        private Game1 game;
        private Dictionary<Keys, ICommand> pressKeyBindings = new Dictionary<Keys, ICommand>();
        private Dictionary<Keys, ICommand> holdKeyBindings = new Dictionary<Keys, ICommand>();
        private KeyboardState previousKeyboardState;
        private GameObjectManager GOM;
        private GameState gameState;

        public KeyboardController(Game1 game, GameObjectManager GOM, GameState gameState)
        {
            this.game = game;
            this.GOM = GOM;
            this.gameState = gameState;
            gameState.AddKeyboard(this);
            ResetCommandBindings();
           
        }

        public void RegisterHoldCommand(Keys key, ICommand command)
        {
            holdKeyBindings.Add(key, command);
        }

        public void RegisterPressCommand(Keys key, ICommand command)
        {
            pressKeyBindings.Add(key, command);
        }

        private bool HasBeenPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        public void ResetCommandBindings()
        {
            pressKeyBindings = new Dictionary<Keys, ICommand>();
            holdKeyBindings = new Dictionary<Keys, ICommand>();

            RegisterHoldCommand(Keys.D, new MarioRightCommand((IPlayer)GOM.MasterList[0][0][0]));
            RegisterHoldCommand(Keys.A, new MarioLeftCommand((IPlayer)GOM.MasterList[0][0][0]));
            RegisterHoldCommand(Keys.S, new MarioCrouchCommand((IPlayer)GOM.MasterList[0][0][0]));

            RegisterPressCommand(Keys.N, new MarioThrowingCommand(((IPlayer)GOM.MasterList[0][0][0]), gameState));
            RegisterHoldCommand(Keys.W, new MarioJumpCommand((IPlayer)GOM.MasterList[0][0][0]));

            RegisterPressCommand(Keys.Down, new PlaceBlockCommand((IPlayer)GOM.MasterList[0][0][0], IPlayer.CardinalDir.Down));
            RegisterPressCommand(Keys.Up, new PlaceBlockCommand((IPlayer)GOM.MasterList[0][0][0], IPlayer.CardinalDir.Up));
            RegisterPressCommand(Keys.Left, new PlaceBlockCommand((IPlayer)GOM.MasterList[0][0][0], IPlayer.CardinalDir.Left));
            RegisterPressCommand(Keys.Right, new PlaceBlockCommand((IPlayer)GOM.MasterList[0][0][0], IPlayer.CardinalDir.Right));

            RegisterPressCommand(Keys.Q, new QuitGameCommand(game));
            RegisterPressCommand(Keys.P, new PauseCommand());
            RegisterPressCommand(Keys.R, new ResetCommand(GOM));

            RegisterPressCommand(Keys.D1, new UseItemCommand((IPlayer)GOM.MasterList[0][0][0], gameState, PowerType.FireFlower));
            RegisterPressCommand(Keys.D2, new UseItemCommand((IPlayer)GOM.MasterList[0][0][0], gameState, PowerType.RedMushroom));
            RegisterPressCommand(Keys.D3, new UseItemCommand((IPlayer)GOM.MasterList[0][0][0], gameState, PowerType.Star));
            RegisterPressCommand(Keys.F3, new ShowDebugScreenCommand());
        }

        public void Update(GameTime gametime)
        {
            // Get keyboard state
            KeyboardState state = Keyboard.GetState();

            // Execute any commands where the key is held.
            foreach (var elt in holdKeyBindings)
            {
                if (state.IsKeyDown(elt.Key))
                {
                    elt.Value.Execute();
                }
            }

            // Execute commands where key is pressed.
            foreach (var elt in pressKeyBindings)
            {
                if (HasBeenPressed(elt.Key))
                {
                    elt.Value.Execute();
                }
            }

            // Set the current state to the previous keyboard state when done.
            previousKeyboardState = state;
        }
    }
}
