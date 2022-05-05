using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class InteractiveBrick : StructureUseability
    {
        private IStructure brickObj;

        public InteractiveBrick(IStructure targetBrick)
        {
            this.brickObj = targetBrick;
        }

        public override void ChangeUseability(GameTime gameTime)
        {
            brickObj.UState = new BrokenBrick(brickObj, gameTime);
            SoundFactory.Instance.GetSoundEffect("BreakBlock").Play();
            HUD.Instance.Getblock();
        }

        public override void Update(GameTime gameTime)                   
        {
            return;
        }

        public override bool IsInteractive()
        {
            return true;
        }
    }

    public class InteractiveCoinBlock : StructureUseability
    {
        private IStructure coinBlockObj;
        private int useCount;

        public InteractiveCoinBlock(IStructure targetCoinBlock, int useTimes)
        {
            this.coinBlockObj = targetCoinBlock;
            this.useCount = useTimes;
        }

        public override void ChangeUseability(GameTime gameTime)
        {
            SoundFactory.Instance.GetSoundEffect("CollectCoin").Play();
            coinBlockObj.UState = new UsedCoinBlock(coinBlockObj, this.useCount, gameTime);
        }

        public override void Update(GameTime gameTime)                   
        {
            return;
        }
        public override bool IsInteractive()
        {
            return true;
        }
    }

    public class InteractiveQuestionBlock : StructureUseability
    {
        private IStructure qBlockObj;

        private ItemType type;

        public InteractiveQuestionBlock(IStructure targetQBlock, ItemType type)
        {
            this.qBlockObj = targetQBlock;
            this.type = type;
        }

        public override void ChangeUseability(GameTime gameTime)
        {
            qBlockObj.UState = new UsedQuestionBlock(qBlockObj, this.type, gameTime);
            GameObjectManager.Instance.ItemList.Add(new Item(this.type, qBlockObj.Location, true, gameTime));
            SoundFactory.Instance.GetSoundEffect("PowerUpAppears").Play();
        }

        public override void Update(GameTime gameTime)                   
        {
            return;
        }
        public override bool IsInteractive()
        {
            return true;
        }
    }

    public class InteractivePipe : StructureUseability
    {
        private IStructure pipeObj;

        public InteractivePipe(IStructure targetPipe)
        {
            this.pipeObj = targetPipe;
        }

        public override void ChangeUseability(GameTime gameTime)
        {
            pipeObj.UState = new SolidPipe(pipeObj);
        }

        public override void Update(GameTime gameTime)                   
        {
            return;
        }
        public override bool IsInteractive()
        {
            return true;
        }
    }

    public class InteractiveFlag : StructureUseability
    {
        private IStructure flagObj;

        public InteractiveFlag(IStructure targetFlag)
        {
            this.flagObj = targetFlag;
        }

        public override void ChangeUseability(GameTime gameTime)
        {
            flagObj.UState = new UsedFlag(flagObj, gameTime);
        }

        public override void Update(GameTime gameTime)                   
        {
            return;
        }
        public override bool IsInteractive()
        {
            return true;
        }
    }
}
