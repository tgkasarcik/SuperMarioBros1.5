using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
    /*
     * SpriteOverlay enum to control effects overlayed on top of sprites
     */
	public enum SpriteOverlay
	{
        Star,
        Damaged,
        None
	}
    
    public class Sprite : ISprite
    {
        
        // Public Members         
        public Texture2D Texture { get; set; }
        public Vector2 Location { get; set; } 

        // Private Members
        private readonly int startX;
        private readonly int startY;
        private readonly int width;
        private readonly int height;
        private readonly int numFrames;
        private int currentFrame;
        private readonly TimeSpan msPerFrame;
        private readonly TimeSpan msPerStarUpdate = TimeSpan.FromMilliseconds(35);
        private TimeSpan updateTimer;
        private TimeSpan starTimer;
        private readonly SpriteEffects effect;
        private Color color;
        private SpriteOverlay overlay;
        private List<Color> starColors;
        private readonly Color damagedColor = new Color(175, 150, 255, 215);
        private int colorIndex;
        private readonly float depth;

        /*
         * Constructor
         */
        public Sprite(Texture2D texture, int startX, int startY, int width, int height, int numFrames, int msPerFrame = 200, SpriteEffects effect = SpriteEffects.None, Color? color = null, SpriteOverlay overlay = SpriteOverlay.None, float depth = 1.0f)
		{
            Texture = texture;
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
            this.numFrames = numFrames;
            this.msPerFrame = TimeSpan.FromMilliseconds(msPerFrame);
            this.effect = effect;

            if (color != null)
			{
                this.color = (Color)color;
			} else
			{
                this.color = Color.White;
			}

            this.overlay = overlay;
            this.depth = depth;

            starColors = new List<Color>
                    {
                        Color.LightGreen,
                        Color.Orange,
                        Color.CadetBlue
                    };

            colorIndex = 0;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            int x = startX + (width * currentFrame);
            int y = startY;

            Rectangle sourceRectangle = new Rectangle(x, y, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, width, height);

            spritebatch.Draw(Texture, destinationRectangle, sourceRectangle, color, 0.0f, new Vector2(0, 0), effect, depth);
        }

        public void Update(GameTime time)
        {
            updateTimer += time.ElapsedGameTime;
            if (updateTimer > msPerFrame)
			{
                updateTimer = TimeSpan.FromMilliseconds(0);

				// Loop through all frames of animation
				currentFrame++;
				if (currentFrame == numFrames)
					currentFrame = 0;
			}

            // Change the color of Star Mario
            if (overlay == SpriteOverlay.Star)
			{
                starTimer += time.ElapsedGameTime;
                if (starTimer > msPerStarUpdate)
				{
                    starTimer = TimeSpan.FromMilliseconds(0);

                    color = starColors[colorIndex];

                    colorIndex++;
                    if (colorIndex == starColors.Count)
                        colorIndex = 0;
				}
			}

            if (overlay == SpriteOverlay.Damaged)
			{
                color = damagedColor;
            }
		}

        public Rectangle GetHitBox()
		{
            return new Rectangle((int)Location.X, (int)Location.Y, width, height);
		}

        public bool IsDrawn()
        {
            return true;
        }

        public void ToggleStar()
		{
            if (overlay == SpriteOverlay.Star)
			{
                overlay = SpriteOverlay.None;
			} else
			{
                overlay = SpriteOverlay.Star;
			}
		}

        public void ToggleDamaged()
		{
            if (overlay == SpriteOverlay.Damaged)
            {
                overlay = SpriteOverlay.None;
            }
            else
            {
                overlay = SpriteOverlay.Damaged;
            }
        }
    }
}
