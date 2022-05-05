using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5
{
    public interface IItem : IGameObject
    {
        IObjHealth HState { get; set; }

        // Called when the item is picked up by a player character
        void Collect(GameTime gameTime);
    }
}
