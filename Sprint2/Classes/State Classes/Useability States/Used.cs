using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class UsedQuestionBlock : StructureUseability
    {
        //Defines how many seconds it takes from use start for the object to bounce up (Time to only bounce up)
        private static double BOUNCE_UP_TIME = 0.15;
        //Defines how many seconds it takes from use start for the object to fall back into original place (Time to bounce up and back down)
        private static double FALL_BACK_DOWN_TIME = 0.30;
        //Defines how far the object will bounce in the Y direction in one update
        private static int Y_BOUNCE_DISTANCE = 1;
        //Defines how far the object will bounce in the Y direction in one update
        private static int X_BOUNCE_DISTANCE = 0;

        private IStructure qBlockObj;
        private ItemType type;
        private double useStart;
        private bool bumpable;

        public UsedQuestionBlock(IStructure qBlock, ItemType type, GameTime gameTime)
        {
            this.qBlockObj = qBlock;
            this.type = type;
            this.useStart = gameTime.TotalGameTime.TotalSeconds;
            bumpable = false;
        }

        public override void ChangeUseability(GameTime gameTime)
        {
            
            if (bumpable)
			{
                qBlockObj.UState = new DestroyedQuestionBlock(this.qBlockObj);
            }
            
        }

        public override void Update(GameTime gameTime)                           
        {
            if ((gameTime.TotalGameTime.TotalSeconds - useStart) < BOUNCE_UP_TIME)
            {
                qBlockObj.Location = Vector2.Add(qBlockObj.Location, new Vector2(X_BOUNCE_DISTANCE, -Y_BOUNCE_DISTANCE));
            }
            else if ((gameTime.TotalGameTime.TotalSeconds - useStart) < FALL_BACK_DOWN_TIME)
            {
                qBlockObj.Location = Vector2.Add(qBlockObj.Location, new Vector2(X_BOUNCE_DISTANCE, Y_BOUNCE_DISTANCE));
            }
            else
            {
                qBlockObj.Location = Vector2.Add(qBlockObj.Location, new Vector2(X_BOUNCE_DISTANCE, -Y_BOUNCE_DISTANCE));
                bumpable = true;
                this.ChangeUseability(gameTime);
            }
        }
    }

    public class UsedCoinBlock : StructureUseability
    {
        //Defines how many seconds it takes from use start for the object to bounce up (Time to only bounce up)
        private static double BOUNCE_UP_TIME = 0.15;
        //Defines how many seconds it takes from use start for the object to fall back into original place (Time to bounce up and back down)
        private static double FALL_BACK_DOWN_TIME = 0.30;
        //Defines how far the object will bounce in the Y direction in one update
        private static int Y_BOUNCE_DISTANCE = 1;
        //Defines how far the object will bounce in the Y direction in one update
        private static int X_BOUNCE_DISTANCE = 0;
        //Defines how much the number of remaining uses are used per interaction
        private static int USE_DEDUCTION = 1;
        //Defines how many useCountRemaining there must be to be a fully used coin block
        private static int FULLY_USED = 0;                             

        private IStructure coinBlockObj;
        private double useStart;
        private int useCountRemaining;
        private bool bumpable;

        public UsedCoinBlock(IStructure coinBlock, int useCount, GameTime gameTime)
        {
            this.coinBlockObj = coinBlock;
            this.useStart = gameTime.TotalGameTime.TotalSeconds;
            this.useCountRemaining = useCount - USE_DEDUCTION;
            GameObjectManager.Instance.ItemList.Add(new Item(ItemType.CoinCollect, coinBlockObj.Location, true, gameTime));
            HUD.Instance.GetCoin();
            bumpable = false;
        }

        public override void ChangeUseability(GameTime gameTime)
        {
            if (bumpable) {
                if (this.useCountRemaining == FULLY_USED) {
                    coinBlockObj.UState = new DestroyedCoinBlock(this.coinBlockObj);
                } else
                {
                    coinBlockObj.UState = new InteractiveCoinBlock(this.coinBlockObj, this.useCountRemaining);
                }
            }
        }

        public override void Update(GameTime gameTime)                           
        {
            if ((gameTime.TotalGameTime.TotalSeconds - useStart) < BOUNCE_UP_TIME)
            {
                coinBlockObj.Location = Vector2.Add(coinBlockObj.Location, new Vector2(X_BOUNCE_DISTANCE, -Y_BOUNCE_DISTANCE));
            }
            else if ((gameTime.TotalGameTime.TotalSeconds - useStart) < FALL_BACK_DOWN_TIME)
            {
                coinBlockObj.Location = Vector2.Add(coinBlockObj.Location, new Vector2(X_BOUNCE_DISTANCE, Y_BOUNCE_DISTANCE));
            }
            else
            {
                coinBlockObj.Location = Vector2.Add(coinBlockObj.Location, new Vector2(X_BOUNCE_DISTANCE, -Y_BOUNCE_DISTANCE));
                bumpable = true;
                this.ChangeUseability(gameTime);
            }
        }
    }

    public class UsedFlag : StructureUseability
    {
        private IStructure flagObj;
        private double useStart;

        public UsedFlag(IStructure flagObj, GameTime gameTime)
        {
            this.flagObj = flagObj;
            this.useStart = gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void ChangeUseability(GameTime gameTime)                 
        {
            return;
        }

        public override void Update(GameTime gameTime)                           
        {
            return;
        }
    }
}
