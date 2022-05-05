using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class BrokenBrick : StructureUseability
    {
        //Defines how far the object will bounce in the Y direction in one update
        private static int Y_BOUNCE_DISTANCE = 2;
        //Defines how far the object will bounce in the Y direction in one update
        private static int X_BOUNCE_DISTANCE = 0;
        //Defines how many seconds it takes from break start for the object to bounce up
        private static double BOUNCE_TIME = 0.1;
        private IStructure brickObj;
        private double breakStart;

        public BrokenBrick(IStructure targetBrick, GameTime gameTime)
        {
            brickObj = targetBrick;
            breakStart = gameTime.TotalGameTime.TotalSeconds;
        }

        /*
         * After a brick gets broken, it runs a short particle effect and then is removed from the screen
         */
        public override void Update(GameTime gameTime)         
        {
            if (gameTime.TotalGameTime.TotalSeconds - breakStart < BOUNCE_TIME)
            {
                brickObj.Location = Vector2.Add(brickObj.Location, new Vector2(X_BOUNCE_DISTANCE, -Y_BOUNCE_DISTANCE));
            }
            else
            {
                brickObj.Broken(gameTime);
            }
        }
    }
}
