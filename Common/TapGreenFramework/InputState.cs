using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace TapGreenFramework
{
	public class InputState
	{
		private TouchCollection _touchState;

		private readonly List<GestureSample> _gestures = new List<GestureSample>();

		private GameTime _gameTime;
		public GameTime GameTime
		{
			get { return _gameTime; }
		}

		public List<GestureSample> Gestures
		{
			get { return _gestures; }
		}

		public void Update(GameTime gameTime)
		{
			_touchState = TouchPanel.GetState();
			_gameTime = gameTime;

			_gestures.Clear();

			while (TouchPanel.IsGestureAvailable)
				_gestures.Add(TouchPanel.ReadGesture());
		}
	}
}