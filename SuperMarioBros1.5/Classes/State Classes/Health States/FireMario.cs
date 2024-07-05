using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Sprint5
{
    class FireMario : PlayerHealth
    {
        //Mario can only throw 1 fireball per half second (500 milliseconds)
        private static double THROWINTERVAL = 500;
        //Mario takes 0.25 seconds to throw a fireball (250 milliseconds)
        private static double THROW_TIME = 250;
        //Time of the last fireball that was thrown in milliseconds
        private double lastThrow;
        // Tracks whether Mario is currently in the process of throwing
        private bool throwing;

        public FireMario(IPlayer targetMario, bool hasStar, double starStart)
        {
            marioObj = targetMario;
            this.hasStar = hasStar;
            this.starStart = starStart;
            lastThrow = -1;
            throwing = false;
        }

        public override List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();
            keys.Add(spriteKeys["FireMario"]);
            if (throwing) keys.Add("Throw");
            return keys;
        }

        /*
         * If Mario picks up a star, then he becomes invulnerable while still FireMario
         */
        public override void PickupPower(PowerType power, GameTime gameTime)
        {
            SoundFactory.Instance.GetSoundEffect("CollectPowerUp").Play();
            switch (power)
            {
                case PowerType.OneUpMushroom:
                    //GameState.Instance.Lives++; TODO: Add lives counter somewhere
                    break;
                case PowerType.Star:
                    hasStar = true;
                    starStart = gameTime.TotalGameTime.TotalSeconds;
                    MediaPlayer.Play(SoundFactory.Instance.GetSong("Star"));
                    break;
            }
        }

        /*
         * If Mario takes damage in FireMario state, he will end up in the BigMario state
         */
        public override void TakeDamage()                                                        
        {
            if (!hasStar)
            {
                SoundFactory.Instance.GetSoundEffect("PipeSound").Play();
                marioObj.HState = new BigMario(marioObj, hasStar, starStart);
            }
        }

        public override IGameObject ThrowFireBall(bool faceRight, Vector2 Location, SpriteFactory spriteFactory, GameTime gameTime)
        {
            lastThrow = gameTime.TotalGameTime.TotalMilliseconds;
            throwing = true;
            return new Fireball(faceRight, Location, spriteFactory, gameTime);
        }

        public override bool CanThrow(GameTime gameTime)
        {
            if ((THROWINTERVAL.CompareTo(gameTime.TotalGameTime.TotalMilliseconds - lastThrow) <= 0) || lastThrow == -1) return true;
            else return false;
        }

        public override void Update(GameTime gametime)
        {
            if (hasStar)
            {
                if (MAX_STAR_TIME.CompareTo(gametime.TotalGameTime.TotalSeconds - starStart) <= 0)
                {
                    hasStar = false;
                }
            }

            if (THROW_TIME.CompareTo(gametime.TotalGameTime.TotalMilliseconds - lastThrow) <= 0)
            {
                throwing = false;
            }
        }
    }
}
