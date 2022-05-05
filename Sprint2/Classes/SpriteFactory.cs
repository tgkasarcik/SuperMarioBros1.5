
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
    /*
     * Singleton class used for creating all <c> ISprite </c> objects.
     */
    public class SpriteFactory
    {
        /*
         * Private Dictionary to hold parameters for all ISprites
         */
        private Dictionary<string, object[]> spriteDict;

        /*
         * Private Dictionary to hold all Spritesheets
         */
        private Dictionary<string, Texture2D> spritesheetDict;

        /*
         * String constant to represent a Sprite with a MissingTexture
         */
        private static readonly string MISSING_TEXTURE = "MissingTexture";

        /*
         * ConstructorInfo object used to create new ISprite instances
         */
        private ConstructorInfo constructor;

        /*
         * Private Instance of <c> this </c>.
         */
        private static SpriteFactory instance = new SpriteFactory();

        /*
         * Public Instance of <c> this </c>.
         */
        public static SpriteFactory Instance
        {
            get
            {
                return instance;
            }
        }

        /*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
        private SpriteFactory() 
        {
            spriteDict = new Dictionary<string, object[]>();
            spritesheetDict = new Dictionary<string, Texture2D>();

            // Initialize ConstructorInfo Object
            Type spriteType = typeof(Sprite);
            Type[] types = new Type[11];

            types[0] = typeof(Texture2D);

            types[1] = typeof(int);
            types[2] = typeof(int);
            types[3] = typeof(int);
            types[4] = typeof(int);
            types[5] = typeof(int);
            types[6] = typeof(int);

            types[7] = typeof(SpriteEffects);
            types[8] = typeof(Color);
            types[9] = typeof(SpriteOverlay);
            types[10] = typeof(float);

            constructor = spriteType.GetConstructor(types);
        }

        /*
         * Register a new Spritesheet Texture with <c> this </c>.
         */
        public void RegisterTexture(ContentManager content, string name, string path)
		{
            // If the specified name is not already in the dictionary, add it
            if (!spritesheetDict.ContainsKey(name))
			{
                spritesheetDict.Add(name, content.Load<Texture2D>(path));
			}
            // Otherwise, do not make a new entry
		}

        /*
         * Register a new Sprite with <c> this </c>.
         */
        public void RegisterSprite(string name, string textureName, int x, int y, int width, int height, int frames, int msPerFrame, SpriteEffects effect, Color color, SpriteOverlay overlay, float depth)
		{
            // To allow for different sprites for different levels, add a new entry or replace an existing one
            // If the specified name is already in the dictionary, remove it
            if (spriteDict.ContainsKey(name))
            {
                spriteDict.Remove(name);
            }

            // Add a new entry
            object[] param = new object[]
               {
                    spritesheetDict[textureName],
                    x,
                    y,
                    width,
                    height,
                    frames,
                    msPerFrame,
                    effect,
                    color,
                    overlay,
                    depth
               };

            spriteDict.Add(name, param);
        }

        /*
         * Return a new <c> ISprite </c> object based on paramaters for entityName, direction, and action.
         * If the input set of parameters does not match a sprite in the dictionary, return a new sprite
         * with a missing texture.
         * 
         * <param> entityName
         *      Name of the desired object
         *      Options: SmallMario, BigMario, FireMario, Koopa
         * </param>
         * <param> direction
         *      Facing direction of the desied sprite
         *      Options: Left, Right
         * </param>
         * <param> action
         *      Action of the desired sprite
         *      Options: Still, Move, Jump, Crouch, Throw
         * </param>
         */
        public ISprite GetSprite(String entityName, String direction, String action)
        {
            String key = String.Concat(entityName, direction, action);

            return GetSprite(key);
        }

        /*
         * Return a new <c> ISprite </c> based on a single name paramater.
         * If the input name does not match a sprite in the dictionary, return a new sprite
         * with a missing texture.
         */
        public ISprite GetSprite(String key)
        {
            ISprite ret;

            // If the specified key is in the dictionary, return a new sprite with it
            if (spriteDict.ContainsKey(key))
            {
                ret = (ISprite)constructor.Invoke(spriteDict[key]);
            }
            // Otherwise, return a new MissingTexture Sprite
            else
            {
                ret = (ISprite)constructor.Invoke(spriteDict[MISSING_TEXTURE]);
            }

            return ret;
        }
    }
}
