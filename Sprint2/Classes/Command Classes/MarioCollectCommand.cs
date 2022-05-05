using System.Diagnostics;

namespace Sprint5
{
    internal class MarioCollectCommand : ICommand
    {
        private CollisionObject collider, collided;
        private GameState gameState;
        public MarioCollectCommand(CollisionObject collider, CollisionObject collided, GameState gameState)
        {
            this.collider = collider;
            this.collided = collided;
            this.gameState = gameState;
        }

        public void Execute()
        {
            ((Item)collided.GameObject).Collect(gameState.GameTime);
            if(((Item)collided.GameObject).type != ItemType.Coin)
            {
                PowerType power = PowerType.Star;
                if (((Item)collided.GameObject).type == ItemType.FireFlower)
                {
                    power = PowerType.FireFlower;
                }
                if (((Item)collided.GameObject).type == ItemType.RedMushroom)
                {
                    power = PowerType.RedMushroom;
                }
                if (((Item)collided.GameObject).type == ItemType.OneUpMushroom)
                {
                    power = PowerType.OneUpMushroom;
                }

                ((Mario)collider.GameObject).PickupPower(power, gameState.GameTime);
            }
        }
    }
}