using Microsoft.Xna.Framework;

namespace Sprint5
{
    class SolidBlock : StructureUseability
    {
        private IStructure blockObj;

        public SolidBlock(IStructure targetBlock)
        {
            this.blockObj = targetBlock;
        }

        public override void Update(GameTime gameTime)       
        {
            return;
        }
    }

    class SolidPipe : StructureUseability
    {
        private IStructure pipeObj;

        public SolidPipe(IStructure targetPipe)
        {
            this.pipeObj = targetPipe;
        }

        public override void Update(GameTime gameTime)       
        {
            return;
        }
    }
}
