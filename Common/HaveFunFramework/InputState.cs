using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace HaveFunFramework
{
    public class InputState
	{
		private TouchCollection _touchState;

		private readonly List<GestureSample> _gestures = new List<GestureSample>();

        public List<GestureSample> Gestures
        {
            get { return _gestures; }
        }

        public void Update()
		{
			_touchState = TouchPanel.GetState();

			_gestures.Clear();

            while (TouchPanel.IsGestureAvailable)
            {
				_gestures.Add(TouchPanel.ReadGesture());
            }
		}
	}
}
