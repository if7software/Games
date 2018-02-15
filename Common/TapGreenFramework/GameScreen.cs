using TapGreenFramework.Interfaces;
using TapGreenFramework.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace TapGreenFramework
{
	public abstract class GameScreen : IGameScreen
	{
		private bool _isPopup;
		private bool _isExiting;
		private bool _otherScreenHasFocus;

		private GestureType _enabledInput;
		private ScreenManager _screenManager;
		private GameScreenState _screenState; 

		public GameScreenState ScreenState
		{
			get { return _screenState; }
			set { _screenState = value; }
		}

		public ScreenManager ScreenManager
		{
			get { return _screenManager; }
			set { _screenManager = value; }
		}

		public GestureType EnabledInput
		{
			get { return _enabledInput; }
			protected set { _enabledInput = value; }
		}

		public bool IsExiting
		{
			get { return _isExiting; }
			protected set { _isExiting = value; }
		}

		public bool IsPopup
		{
			get { return _isPopup; }
			protected set { _isPopup = value; }
		}

		public GameScreen()
		{
			_isExiting = false;

			_screenState = GameScreenState.Hidden;
		}

		public void Initialize()
		{
		}

		public virtual void HandleInput(InputState input) { }

		public virtual void UpdatePresence() { }

		public virtual void CloseScreen()
		{
			ScreenManager.RemoveScreen(this);
		}

		public virtual void LoadContent() { }

		public virtual void UnloadContent() { }

		public void Dispose() { }

		public virtual void Back(GameTime gameTime) { }

		public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			ScreenManager.Screens.Last().Back(gameTime);

			_otherScreenHasFocus = otherScreenHasFocus;

			if (_isExiting)
			{
				ScreenManager.RemoveScreen(this);

				_isExiting = false;
			}
			else if (coveredByOtherScreen)
			{
				_screenState = GameScreenState.Hidden;
			}
			else
			{
				_screenState = GameScreenState.Active;
			}
		}

		public virtual void Draw(GameTime gameTime) { }

		public T Load<T>(string assetName)
		{
			return ScreenManager.Game.Content.Load<T>(assetName);
		}
	}
}