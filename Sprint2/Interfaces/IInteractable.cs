using Microsoft.Xna.Framework;

namespace Sprint5
{
    public interface IInteractable
	{

		/*
		 * Object that is interacted with by player does what it needs and then affects the player who interacted based on what the object's interaction affect is
		 *		This affect will often being to tell the interacting player to animate a certain way
		 */
		void PlayerInteract(Mario player, GameTime gameTime);
	}
}
