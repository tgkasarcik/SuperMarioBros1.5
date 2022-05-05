using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint5
{
    class PlayerDead : PlayerHealth
    {
        //Holds object that should be the parent of the state
        private IPlayer marioObj;

        public PlayerDead(IPlayer targetMario)
        {
            marioObj = targetMario;
        }

        /*
         * Mario can't take more damage after dying
         */
        public override void TakeDamage()                                
        {
            return;
        }
    }

    class EnemyDead : IObjHealth
    {
        //Dictionary of state names to Sprite factory key parts
        private Dictionary<string, string> spriteKeys = new Dictionary<string, string>() { { "Koopa", "Koopa" }, { "Goomba", "GoombaDead" }, { "Shell", "ShellGreen" } };
        //Holds object that should be the parent of the state
        private IEnemy enemyObj;                               

        public EnemyDead(IEnemy targetEnemy)
        {
            enemyObj = targetEnemy;
        }

        public List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();
            keys.Add(spriteKeys[enemyObj.GetType().Name]);
            return keys;
        }

        /*
         * Enemy can't take more damage after dying
         */
        public void TakeDamage()                                
        {
            return;
        }

        public bool IsDead()
        {
            return true;
        }

        /*
         * Doesn't move after death
         */
        public void Update(GameTime gameTime)                   
        {
            return;
        }
    }

        class ItemDead : IObjHealth
        {
            //Holds object that should be the parent of the state
            private IItem itemObj;                               

            public ItemDead(IItem targetItem)
            {
                itemObj = targetItem;
            }

        public List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();
            keys.Add(itemObj.GetType().Name);
            return keys;
        }

        /*
        * Item can't take more damage after "death"
        */
        public void TakeDamage()                                
        {
            return;
        }

        public bool IsDead()
        {
            return true;
        }

        /*
        * Doesn't move after "death"
        */
        public void Update(GameTime gameTime)                   
            {
                return;
            }
        }
}
