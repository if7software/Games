using HaveFunFramework.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace HaveFunFramework
{
	public abstract class GameScreen
	{
		private bool _otherScreenHasFocus;

		private bool _isPopup;
		public bool IsPopup
		{
			get { return _isPopup; }
			protected set { _isPopup = value; }
		}

		private ScreenState _screenState;
		public ScreenState ScreenState
		{
			get { return _screenState; }
			protected set { _screenState = value; }
		}

		private TimeSpan _transitionOnTime;
		public TimeSpan TransitionOnTime
		{
			get { return _transitionOnTime; }
			protected set { _transitionOnTime = value; }
		}

		private TimeSpan _transitionOffTime;
		public TimeSpan TransitionOffTime
		{
			get { return _transitionOffTime; }
			protected set { _transitionOffTime = value; }
		}

		private float _transitionPosition;
		public float TransitionPosition
		{
			get { return _transitionPosition; }
			protected set { _transitionPosition = value; }
		}

		private bool _isExiting;
		public bool IsExiting
		{
			get { return _isExiting; }
			protected internal set { _isExiting = value; }
		}

		private GestureType _enabledGestures;
		public GestureType EnabledGestures
		{
			get { return _enabledGestures; }
			protected set
			{
				_enabledGestures = value;

				if (ScreenState == ScreenState.Active)
				{
					TouchPanel.EnabledGestures = value;
				}
			}
		}

		private ScreenManager _screenManager;
		public ScreenManager ScreenManager
		{
			get { return _screenManager; }
			internal set { _screenManager = value; }
		}

		public float TransitionAlpha
		{
			get { return 1f - TransitionPosition; }
		}

		public bool IsActive
		{
			get { return !_otherScreenHasFocus && (_screenState == ScreenState.TransitionOn || _screenState == ScreenState.Active); }
		}

		public GameScreen()
		{
			Initialize();
		}

		private void Initialize()
		{
			_isPopup = false;
			_isExiting = false;
			_transitionPosition = 1;
			_transitionOnTime = TimeSpan.Zero;
			_transitionOffTime = TimeSpan.Zero;
			_enabledGestures = GestureType.None;
			_screenState = ScreenState.TransitionOn;
		}

		public virtual void LoadContent() { }

		public virtual void UnloadContent() { }

		public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			_otherScreenHasFocus = otherScreenHasFocus;

			if (_isExiting)
			{
				_screenState = ScreenState.TransitionOff;

				if (!UpdateTransition(gameTime, _transitionOffTime, 1))
					ScreenManager.RemoveScreen(this);
			}
			else if (coveredByOtherScreen)
			{
				if (UpdateTransition(gameTime, _transitionOffTime, 1))
					_screenState = ScreenState.TransitionOff;
				else
					_screenState = ScreenState.Hidden;
			}
			else
			{
				if (UpdateTransition(gameTime, _transitionOnTime, -1))
					_screenState = ScreenState.TransitionOn;
				else
					_screenState = ScreenState.Active;
			}
		}

		bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
		{
			float transitionDelta;

			if (time == TimeSpan.Zero)
				transitionDelta = 1;
			else
				transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

			_transitionPosition += transitionDelta * direction;

			if (((direction < 0) && (_transitionPosition <= 0)) || ((direction > 0) && (_transitionPosition >= 1)))
			{
				_transitionPosition = MathHelper.Clamp(_transitionPosition, 0, 1);
				return false;
			}

			return true;
		}

		public virtual void HandleInput(InputState input) { }

		public virtual void Draw(GameTime gameTime) { }
	}
}
