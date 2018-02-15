using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using TapGreenFramework.Utilities;

namespace TapGreenFramework
{
	public abstract class MenuScreen : GameScreen
	{
		private IList<MenuEntry> _menuEntries;
		protected IList<MenuEntry> MenuEntries
		{
			get { return _menuEntries; }
		}

		public MenuScreen()
		{
			EnabledInput = GestureType.Tap;

			_menuEntries = new List<MenuEntry>();
		}

		protected virtual void OnSelectEntry(int entryIndex)
		{
			_menuEntries[entryIndex].OnSelectEntry();
		}

		public override void HandleInput(InputState input)
		{
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    Point tapLocation = new Point((int)gesture.Position.X, (int)gesture.Position.Y);

                    for (int i = 0; i < _menuEntries.Count; i++)
                    {
                        MenuEntry menuEntry = _menuEntries[i];

						ExTexture2D exTexture = ScreenManager.GetTexture2D(menuEntry.Texture);

                        if (exTexture.Destination.Contains(tapLocation))
                            OnSelectEntry(i);
                    }
                }
            }
		}
	}
}