using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
    public interface ISpriteKey
    {
        /*
         * Returns a string corresponding to the SpriteFactory's required string names for different Sprites
         *      return part or whole of sprite factory key
         */
        List<string> GetSpriteKey();
    }
}
