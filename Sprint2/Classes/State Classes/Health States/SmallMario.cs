using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Sprint5
{
    
    public class SmallMario : PlayerHealth
    {
        // The speed modifier for Mario's movement
        public static int MOVE_MOD = 1;                                                   

        public SmallMario(IPlayer targetMario)
        {
            marioObj = targetMario;
        }

        /*
         * Mario's health state is modified accordingly based on the powerup he picks up
         */
        public override void PickupPower( PowerType power, GameTime gameTime)                                           
        {
            SoundFactory.Instance.GetSoundEffect("CollectPowerUp").Play();
            switch (power)
            {
                case PowerType.FireFlower:
                    marioObj.HState = new FireMario(marioObj, false, 0);
                    marioObj.Location = Vector2.Add(marioObj.Location, new Vector2(0, -16));
                    break;
                case PowerType.RedMushroom:
                    marioObj.HState = new BigMario(marioObj, false, 0);
                    marioObj.Location = Vector2.Add(marioObj.Location, new Vector2(0, -16));
                    break;
                case PowerType.OneUpMushroom:
                    HUD.Instance.AddLives();
                    break;
                case PowerType.Star:
                    marioObj.HState = new BigMario(marioObj, true, gameTime.TotalGameTime.TotalSeconds);
                    marioObj.Location = Vector2.Add(marioObj.Location, new Vector2(0, -16));
                    MediaPlayer.Play(SoundFactory.Instance.GetSong("Star"));
                    break;
            }          
        }

        /*
         * If Mario takes damage in SmallMario state, he will end up in the Dead state
         */
        public override void TakeDamage()                                                        
        {
            SoundFactory.Instance.GetSoundEffect("MarioDies").Play();
            MediaPlayer.Pause();
            marioObj.HState = new PlayerDead(marioObj);
            HUD.Instance.MinusLives();
            marioObj.AnimationLock(true);
        }
    }
}
