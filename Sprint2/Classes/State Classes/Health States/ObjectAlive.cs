using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint5
{
    class EnemyAlive : IObjHealth
    {
        //Dictionary of state names to Sprite factory key parts
        private Dictionary<string, string> spriteKeys = new Dictionary<string, string>() { { "Koopa", "Koopa" }, { "Goomba", "GoombaMove" }, { "Shell", "ShellGreen" } };
        private IEnemy enemyObj;
        public EnemyAlive(IEnemy targetEnemy)
        {
            this.enemyObj = targetEnemy;
        }

        public List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();
            keys.Add(spriteKeys[enemyObj.GetType().Name]);
            return keys;
        }

        public void TakeDamage()
        {
            enemyObj.HState = new EnemyDead(enemyObj);
        }

        public bool IsDead()
        {
            return false;
        }

        public void Update(GameTime gameTime)
        {
            return;
        }
    }

    class ItemAlive : IObjHealth
    {
        private IItem itemObj;
        public ItemAlive(IItem targetItem)
        {
            this.itemObj = targetItem;
        }

        public List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();
            keys.Add(itemObj.GetType().Name);
            return keys;
        }

        public void TakeDamage()
        {
            itemObj.HState = new ItemDead(itemObj);
        }

        public bool IsDead()
        {
            return false;
        }

        public void Update(GameTime gameTime)
        {
            return;
        }
    }
}
