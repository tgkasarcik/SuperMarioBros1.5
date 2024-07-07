using Microsoft.Xna.Framework;

namespace Sprint5
{
    public class GameObjectCollideTopCommand : ICommand
    {
        private CollisionObject collider, collided;
        private bool isSolid = false;
        private CollisionRectangle intersection;
        public GameObjectCollideTopCommand(CollisionObject collider, CollisionObject collided, CollisionRectangle intersection, GameState gameState)
        {
            this.intersection = intersection;
            this.collider = collider;
            this.collided = collided;
        }
        public void Execute()
        {
            if (collided.GameObject.GetType() == typeof(Block) || collided.GameObject.GetType() == typeof(Pipe) || collided.GameObject.GetType() == typeof(Flag)) isSolid = true;
            // Collides in the x directions, param is isUP
            collider.GameObject.CollideY(true, isSolid, intersection.Intersection); 
        }
    }
}