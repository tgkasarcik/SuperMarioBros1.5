using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5
{

    public enum BackgroundType
	{
        Cloud1,
        Cloud2,
        Cloud3,
        Bush1,
        Bush2,
        Bush3,
        Hill1,
        Hill2,
        Castle,
        FlagPennant,
        Water,
        Lava,
        Fire,
        PalmTree1,
        PalmTree2,
        PalmTree3,
        PalmTree4,
        PineTree1,
        PineTree2,
        PineTree3,
        WinterTree1,
        WinterTree2,
        OSULogo
    }

    public class BackgroundObject : IGameObject
    {
        private ISprite backgroundObjectSprite;
        private SpriteFactory spriteFactory;
        public Vector2 Location { get; set; }
        public bool destroyObject { get; set; }

        private Vector2 initialLocation;

        public BackgroundObject(Vector2 location, SpriteFactory SpriteFactory, BackgroundType type)
        {
            Location = location;
            destroyObject = false;
            initialLocation = location;
            spriteFactory = SpriteFactory;
            backgroundObjectSprite = spriteFactory.GetSprite(type.ToString());
            backgroundObjectSprite.Location = location;
        }

        public Rectangle GetHitBox()
        {
            return backgroundObjectSprite.GetHitBox();
        }

        public void Reset(Vector2 location, GameTime gameTime)
        {
            backgroundObjectSprite.Location = initialLocation;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            backgroundObjectSprite.Draw(spritebatch);
        }
        public bool IsDrawn()
        {
            return backgroundObjectSprite.IsDrawn();
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            //Does not collide with other objects
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            //Does not collide with other objects
        }
        public void Update(GameTime gametime)
        {
            backgroundObjectSprite.Location = Location;
            backgroundObjectSprite.Update(gametime);
        }

        public void DestroyObject()
        {
            destroyObject = true;
        }
    }
}
