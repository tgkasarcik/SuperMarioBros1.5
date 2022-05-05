using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sprint5
{
    public class NextLevelCommand : ICommand
    {
        private GameState gs;

        public NextLevelCommand(GameState gs)
        {
            this.gs = gs;
        }

        public void Execute()
        {
            if (!((Mario)GameObjectManager.Instance.PlayerList[0]).HState.IsDead() && AnimationEngine.Instance.FinishedAnimating())
            {
                gs.NextLevel();
                HUD.Instance.DeathReset();
                //ScreenManager.Instance.ToggleDrawing(ScreenType.LevelLoadScreen);
                GameState.Instance.WorldState = WorldState.PreLevel;
            }
        }
}
}
