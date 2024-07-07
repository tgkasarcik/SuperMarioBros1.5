using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    class HUD : IDrawable, IUpdateable
    {
        private ISprite Sprite { get; set; }
        private int Score;
        private int Coin;
        private double TotalTime;
        private double TimeLeft;

        private double pauseTime = 0;

        private int Block;
        private int Lives;
        private int KillGoomba_Score = 100;
        private int KillKoopa_Score = 200;
        private int CoinScore = 200;
        private int ItemScore = 1000;

        private int TimeScore = 50;
        private int BlockScore = 50;

        private int newLevelTime = 120;

        private const int InitialScore = 0;
        private const int InitialTime = 360;
        private const int InitialLives = 3;
        private const int InitialBlock = 10;

        public Vector2 Location { get; set; }
        
        /*
         * Private Instance of <c> this </c>.
         */
        private static HUD instance = new HUD();

        /*
         * Public Instance of <c> this </c>.
         */
        public static HUD Instance
        {
            get
            {
                return instance;
            }
        }
        
        public HUD()
        {
            Score = InitialScore;
            Coin = InitialScore;
            TotalTime = InitialTime;
            TimeLeft = InitialTime;
            Lives = InitialLives;
            Block = InitialBlock;
        }
        
        /**
         * Update Time
         */
        public void Update(GameTime gametime)
        {
            if (GameState.Instance.WorldState == WorldState.Paused || GameState.Instance.WorldState == WorldState.PlayerInteraction)
            {

                pauseTime += gametime.ElapsedGameTime.TotalSeconds; //Buffers the time that has been lost

            }

            if ((AnimationEngine.Instance.FinishedAnimating())) 
            {

                TimeLeft = TotalTime - gametime.TotalGameTime.TotalSeconds + pauseTime;

            }

        }

        public void Draw(SpriteBatch spritebatch)
        {
            Sprite.Draw(spritebatch);
        }

        public bool IsDrawn()
        {
            return Sprite.IsDrawn();
        }

        /**
         * Update the number of coin
         */
        public void GetCoin()
        {
            Coin++;
            Score += CoinScore;
        }
        /**
         * Update the Score
         */
        public int KillGoomba()
        {
            Score += KillGoomba_Score;
            return KillGoomba_Score;
        }

        public int KillKoopa()
        {
            Score += KillKoopa_Score;
            return KillKoopa_Score;
        }

        public int GetItem()
        {
            Score += ItemScore;
            return ItemScore;
        }

        public void ReachFlag(int flagScore)
        {
            Score += flagScore;
        }
        /**
         * Update the Lives of Mario
         */
        public void AddLives()
        {
            Lives++;
        }

        public void MinusLives()
        {
            Lives--;
        }
        /**
         * Get block and place block
         */
        public int Getblock()
        {
            Block++;
            return Block;
        }
        public int Placeblock()
        {
            Block--;
            return Block;
        }
        public bool hasBlock()
        {
            if(Block > 0)
            {
                return true;
            }
            return false;
        }
        /**
         * Update the level
         */
        public void AddFloor()
        {
            TimeLeft += newLevelTime;
        }
        /**
         * Transfer The Time Left to the score before enter the next level
         * We should call the method for many times until time left is 0
         */
        public void Transfer()
        {

            if (TimeLeft > 0)
			{
                TimeLeft -= 1.0f;
                Score += TimeScore;
            }
            if(Block > 0)
            {
                Score += BlockScore;
                Block--;
            }
        }
        /**
         * Return the time, score, coinNumber and level for drawing
         * If need string, can use toString();
         */
        public string getValue(string category)
        {
            string value =null;
            switch (category)
            {
                case "Score":
                    value = Score.ToString();
                    break;
                case "Coin":
                    value = Coin.ToString();
                    break;
                case "Time":
                    value = ((int)TimeLeft).ToString();
                    break;
                case "Lives":
                    value = Lives.ToString();
                    break;
                case "Block":
                    value = Block.ToString();
                    break;
            }
            return value;
        }

        //Level gets reset after death
        public void DeathReset()
        {
            Score = InitialScore;
            Coin = InitialScore;
            //TotalTime = InitialTime;
            //TimeLeft = InitialTime;

            TotalTime = GameState.Instance.GameTime.TotalGameTime.TotalSeconds + InitialTime;
            TimeLeft = InitialTime;
            pauseTime = 0.0;
            Block = InitialBlock;
        }

        //Level gets reset after a Game Over
        public void GameOverReset()
        {
            Score = InitialScore;
            Coin = InitialScore;
            //TotalTime = InitialTime;
            //TimeLeft = InitialTime;

            TotalTime = GameState.Instance.GameTime.TotalGameTime.TotalSeconds + InitialTime;
            TimeLeft = InitialTime;
            pauseTime = 0.0;
            Block = InitialBlock;
            Lives = InitialLives;
        }
    }
}