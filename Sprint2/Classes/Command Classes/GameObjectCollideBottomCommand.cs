using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class GameObjectCollideBottomCommand : ICommand
    {
        private CollisionObject collider, collided;
        private bool isSolid = false;
        private CollisionRectangle intersection;
        public GameObjectCollideBottomCommand(CollisionObject collider, CollisionObject collided, CollisionRectangle intersection, GameState gameState)
        {
            this.intersection = intersection;
            this.collider = collider;
            this.collided = collided;
        }
        public void Execute()
        {
            if (collided.GameObject.GetType() == typeof(Block) || collided.GameObject.GetType() == typeof(Pipe) || collided.GameObject.GetType() == typeof(Flag)) isSolid = true;
            // Collides in the Y directions, param is isUp
            collider.GameObject.CollideY(false, isSolid, intersection.Intersection); 
        }
    }
}