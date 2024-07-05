using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace Sprint5
{
    public abstract class PlayerHealth : IPlayerHealth
    {
        //Dictionary of state names to Sprite factory key parts
        protected Dictionary<string, string> spriteKeys = new Dictionary<string, string>() { { "BigMario", "BigMario" }, { "FireMario", "FireMario" }, { "SmallMario", "SmallMario" }, { "PlayerDead", "DeadMario" } };

        // The max number of seconds the star power lasts
        protected static double MAX_STAR_TIME = 10;
        // States whether player has the star power
        protected bool hasStar;
        // Tracks the pickup start of the star power
        protected double starStart;
        //Holds object that should be the parent of the state
        protected IPlayer marioObj;

        public virtual List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();
            keys.Add(spriteKeys[this.GetType().Name]);
            return keys;
        }

        public bool IsDead()
        {
            return this.GetType().Name.Contains("Dead");
        }

        public bool HasStar()
        {
            return hasStar;
        }

        //Most common functionality of PickupPower is doing nothing
        public virtual void PickupPower( PowerType power, GameTime gameTime)
        {
            return;
        }

        //All uses of TakeDamage are different from one another
        public abstract void TakeDamage();

        //Most common functionality of ThrowFireball is returning a fireball object after being called
        public virtual IGameObject ThrowFireBall(bool faceRight, Vector2 Location, SpriteFactory spriteFactory, GameTime gameTime)
        {
            return new Fireball(faceRight, Location, spriteFactory, gameTime);
        }

        //Most common occurence of method is going to state that Player can't throw
        public virtual bool CanThrow(GameTime gameTime)
        {
            return false;
        }

        //Most common functionality of Update (Right now at least) is doing nothing
        public virtual void Update(GameTime gametime)
        {
            if (hasStar) {
                if (gametime.TotalGameTime.TotalSeconds - starStart >= MAX_STAR_TIME)
                {
                    hasStar = false;
                    MediaPlayer.Play(GameState.Instance.BackgroundSong);
                }
            }
        }
    }
}
