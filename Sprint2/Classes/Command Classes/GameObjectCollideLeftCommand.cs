using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class GameObjectCollideLeftCommand : ICommand
    {
        private CollisionObject collider, collided;
        private bool isSolid = false;
        private CollisionRectangle intersection;
        public GameObjectCollideLeftCommand(CollisionObject collider, CollisionObject collided, CollisionRectangle intersection, GameState gameState)
        {
            this.intersection = intersection;
            this.collider = collider;
            this.collided = collided;
        }
        public void Execute()
        {
            if (collided.GameObject.GetType() == typeof(Block) || collided.GameObject.GetType() == typeof(Pipe) || collided.GameObject.GetType() == typeof(Flag)) isSolid = true;
     
            collider.GameObject.CollideX(false, isSolid, intersection.Intersection); // Collides in the x directions, param is isUP
            collided.GameObject.CollideX(true, isSolid, intersection.Intersection);
        }
    }
}