using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class GameObjectCollideRightCommand : ICommand
    {
        private CollisionObject collider, collided;
        private bool isSolid = false;
        private CollisionRectangle intersection;
        public GameObjectCollideRightCommand(CollisionObject collider, CollisionObject collided, CollisionRectangle intersection, GameState gameState)
        {
            this.intersection = intersection;
            this.collider = collider;
            this.collided = collided;
        }
        public void Execute()
        {
            if (collided.GameObject.GetType() == typeof(Block) || collided.GameObject.GetType() == typeof(Pipe) || collided.GameObject.GetType() == typeof(Flag)) isSolid = true;
            collider.GameObject.CollideX(true, isSolid, intersection.Intersection); // Collides in the x directions, param is isUP
            collided.GameObject.CollideX(false, isSolid, intersection.Intersection);

        }
    }
}