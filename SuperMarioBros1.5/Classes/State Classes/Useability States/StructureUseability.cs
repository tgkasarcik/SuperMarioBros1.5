using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    public abstract class StructureUseability : IUseability
    {
        //Most common functionality of all functions is to do nothing

        public virtual void ChangeUseability(GameTime gameTime)
        {
            return;
        }

        //Dictionary of state names to Sprite factory key parts
        private Dictionary<string, string> spriteKeys = new Dictionary<string, string>() {
            { "BrokenBrick", "Brick" },
            { "DestroyedCoinBlock", "UsedBlock" },
            { "DestroyedQuestionBlock", "UsedBlock" },
            { "InteractiveBrick", "Brick" },
            { "InteractiveCoinBlock", "Brick" },
            { "InteractiveQuestionBlock", "Question" },
            { "InteractivePipe", "UpPipe" },
            { "InteractiveFlag", "Flag" },
            { "SolidBlock", "GroundBlock" },
            { "SolidPipe", "UpPipe" },
            { "UsedQuestionBlock", "Question" },
            { "UsedCoinBlock", "Brick" },
            { "UsedFlag", "Flag" }
        };

        public List<string> GetSpriteKey()
        {
            List<string> keys = new List<string>();
            keys.Add(spriteKeys[this.GetType().Name]);
            return keys;
        }

        public virtual void Update(GameTime gametime)
        {
            return;
        }

        /*
         * Most common occurence is that the object is not going to be interactive
         */
        public virtual bool IsInteractive()
        {
            return false;
        }
    }
}
