using Microsoft.Xna.Framework.Media;

namespace Sprint5
{
    public class PauseCommand : ICommand
    {

        public PauseCommand()
        {
        }
        public void Execute()
        {
            // Call method to pause game.
            if (GameState.Instance.WorldState == WorldState.Paused)
            {
                GameState.Instance.WorldState = WorldState.Playing;
                MediaPlayer.Resume();

            }
            else if (GameState.Instance.WorldState == WorldState.Playing)
            {
                GameState.Instance.WorldState = WorldState.Paused;
                MediaPlayer.Pause();
            }
            SoundFactory.Instance.GetSoundEffect("PauseGame").Play();
        }
    }
}
