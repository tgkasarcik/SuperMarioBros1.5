using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Sprint5
{
    class BigMario : PlayerHealth
    {

        public BigMario(IPlayer targetMario, bool hasStar, double starStart)
        {
            marioObj = targetMario;
            this.hasStar = hasStar;
            this.starStart = starStart;
        }
        
        /*
         * Mario's health state is modified accordingly based on the powerup he picks up, can aquire FireFlower or Star to change state
         */
        public override void PickupPower(PowerType power, GameTime gameTime)                                           
        {
            SoundFactory.Instance.GetSoundEffect("CollectPowerUp").Play();
            switch (power)
            {
                case PowerType.OneUpMushroom:
                    //GameState.Instance.Lives++; TODO: Add lives counter somewhere
                    break;
                case PowerType.FireFlower:
                    marioObj.HState = new FireMario(marioObj, false, 0);
                    break;
                case PowerType.Star:
                    hasStar = true;
                    starStart = gameTime.TotalGameTime.TotalSeconds;
                    MediaPlayer.Play(SoundFactory.Instance.GetSong("Star"));
                    break;
            }          
        }

        /*
         * If Mario takes damage in BigMario state, he will end up in the SmallMario state
         */
        public override void TakeDamage()                                                      
        {
            if (!hasStar) {
                SoundFactory.Instance.GetSoundEffect("PipeSound").Play();
                marioObj.HState = new SmallMario(marioObj);
            }
        }
    }
}
