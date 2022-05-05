using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
	/*
	 * Interface for creating various game screens
	 */
	public interface IScreen
	{
		/*
		 * Public Members
		 */

		/*
		 * <c> RenderTarget2D </c> object to represent <c> this </c>.
		 */
		RenderTarget2D RenderTarget { get; }

		/*
		 * <c> ScreenType </c> of <c> this </c>.
		 */
		ScreenType Type { get; }

		/*
		 * Draw the correct <c> ISprites </c> to <c> this </c>.
		 */
		void DrawScreen(SpriteBatch spriteBatch);

		/*
		 * Enable or disable drawing for <c> this </c>.
		 */
		//void ToggleDrawing();
	}
}
