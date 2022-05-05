using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Sprint5
{
    public class Flag : IStructure, IInteractable
    {
        private static int PENNANT_OFFSET_x = -13, PENNANT_OFFSET_Y = 8;

        private ISprite sprite { get; set; }
        public IUseability UState { get; set; }
        public Vector2 Location { get; set; }
        public bool destroyObject { get; set; }
        public BackgroundObject Pennant { get; set; }

        private static SpriteFactory spriteFactory;
        public Flag(Vector2 location, SpriteFactory spritefactory)
        {
            spriteFactory = spritefactory;
            Location = location;
            destroyObject = false;
            UState = new InteractiveFlag(this);

            // May need to change this if more levels are added
            sprite = spriteFactory.GetSprite("Flag");
            Pennant = new BackgroundObject(new Vector2(location.X + PENNANT_OFFSET_x, location.Y + PENNANT_OFFSET_Y), spritefactory, BackgroundType.FlagPennant);
            sprite.Location = Location;
        }

        public Rectangle GetHitBox()
        {
            return sprite.GetHitBox();
        }

        public void PlayerInteract(Mario player, GameTime gametime)
        {
            if (UState.GetType().Name == "InteractiveFlag") {
                AnimationEngine.Instance.GrabFlag(gametime, player, this);
                UState.ChangeUseability(gametime);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            sprite.Draw(spritebatch);
            Pennant.Draw(spritebatch);
        }

        public bool IsDrawn()
        {
            return sprite.IsDrawn();
        }

        public void CollideX(bool isRight, bool isSolid, Rectangle collisionOverlap)
        {
            //Not Implemented for Sprint 3
        }

        public void CollideY(bool isUp, bool isSolid, Rectangle collisionOverlap)
        {
            //Not Implemented for Sprint 3
        }
        public void Reset(Vector2 location, GameTime gameTime)
        {
            UState = new InteractiveFlag(this);
        }

        public void Update(GameTime gametime)
        {
            sprite.Location = Location;
            UState.Update(gametime);
            sprite.Update(gametime);
            Pennant.Update(gametime);
        }

        public void DestroyObject()
        {
            destroyObject = true;
        }

        public void Broken(GameTime gametime)
        {

        }
    }
}
