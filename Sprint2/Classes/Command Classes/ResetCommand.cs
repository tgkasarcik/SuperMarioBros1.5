using System.Diagnostics;

namespace Sprint5
{
    public class ResetCommand : ICommand
    {
        GameObjectManager gom;
        public ResetCommand(GameObjectManager gom)
        {
            this.gom = gom;
        }
        public void Execute()
        {
            if (AnimationEngine.Instance.FinishedAnimating()) {
                if (((Mario)GameObjectManager.Instance.PlayerList[0]).gameEnded)
                {
                    GameState.Instance.NextLevel();
                    GameState.Instance.WorldState = WorldState.Playing;
                    ScreenManager.Instance.ToggleDrawing(ScreenType.GameOver);
                    ((Mario)GameObjectManager.Instance.PlayerList[0]).HState = new SmallMario((Mario)GameObjectManager.Instance.PlayerList[0]);
                    ((Mario)GameObjectManager.Instance.PlayerList[0]).AnimationLock(false);
                    ((Mario)GameObjectManager.Instance.PlayerList[0]).gameEnded = false;
                    HUD.Instance.GameOverReset();
                } else
                {
                    if (!((Mario)GameObjectManager.Instance.PlayerList[0]).HState.IsDead())
                    {
                        GameState.Instance.GoToLevel(GameState.Instance.GetLevel());
                        HUD.Instance.DeathReset();
                        ((Mario)GameObjectManager.Instance.PlayerList[0]).HState = new SmallMario((Mario)GameObjectManager.Instance.PlayerList[0]);
                        //ScreenManager.Instance.ToggleDrawing(ScreenType.LevelLoadScreen);
                        GameState.Instance.WorldState = WorldState.PreLevel;
                    }
                }
            }
        }
    }
}
