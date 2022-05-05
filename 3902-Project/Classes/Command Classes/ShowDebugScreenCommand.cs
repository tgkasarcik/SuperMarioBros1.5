using System;
using System.Collections.Generic;
using System.Text;

namespace Sprint5
{
	public class ShowDebugScreenCommand : ICommand
	{
		public ShowDebugScreenCommand()
		{

		}

		public void Execute()
		{
			ScreenManager.Instance.ToggleDrawing(ScreenType.DebugScreen);
		}
	}
}
