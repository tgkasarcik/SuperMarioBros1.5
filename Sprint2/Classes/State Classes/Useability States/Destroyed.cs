using Microsoft.Xna.Framework;

namespace Sprint5
{
    public class DestroyedCoinBlock : StructureUseability
    {
        private IStructure coinBlockObj;

        public DestroyedCoinBlock(IStructure targetCoinBlock)
        {
            this.coinBlockObj = targetCoinBlock;
        }

        //Nothing can happen after it has been destroyed
    }

    public class DestroyedQuestionBlock : StructureUseability
    {
        private IStructure qBlockObj;

        public DestroyedQuestionBlock(IStructure targetQBlock)
        {
            this.qBlockObj = targetQBlock;
        }

        //Nothing can happen after it has been destroyed
    }
}
