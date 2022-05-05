using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Sprint5
{
    public class MarioTakeDamageCommand : ICommand
    {
        CollisionObject mario;
        CollisionObject collided;
        GameState gs;
        public MarioTakeDamageCommand(CollisionObject mario, CollisionObject collided, GameState gs)
        {
            this.mario = mario;
            this.collided = collided;
            this.gs = gs;
        }
        public void Execute()
        {
            if (collided.GameObject.GetType() == typeof(Shell))
            {
                if (!((IEnemy)collided.GameObject).XMoveState.Moving())
                {
                    ((IEnemy)collided.GameObject).Move(((IPlayer)mario.GameObject).XMoveState.FaceRight());
                    ((IEnemy)collided.GameObject).TakeDamage();
                }
                else
                {
                    ((IPlayer)mario.GameObject).TakeDamage(gs.GameTime);
                }
            } else if (((IPlayer)mario.GameObject).HState.HasStar())
            {
                ((IEnemy)collided.GameObject).TakeDamage();
            }
            else
            {
                ((IPlayer)mario.GameObject).TakeDamage(gs.GameTime);
            }
        }
    }
}
