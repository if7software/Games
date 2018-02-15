using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using TapGreenFramework.Enums;
using TapGreenFramework.Utilities;
using TapGreenFramework.EventArgs;

namespace TapGreenFramework
{
	public partial class ScreenManager : DrawableGameComponent
	{
        private const int BUTTON_HEIGHT = 150;
        private const int BUTTON_PAUSE = 100;

        private bool _isInitialized;

		private InputState _inputState;
		private SpriteBatch _spriteBatch;

		private List<GameScreen> _screens;
		private List<GameScreen> _screenToUpdate;

		public event EventHandler DisplayAdvertising;
		public event EventHandler CloseApplication;

        public event EventHandler<SendMessageToSocialMediaEventArgs> ShareMessageInSocialMedia;
        public event EventHandler<SendPostToSocialMediaEventArgs> SharePostInSocialMedia;

        public Action<string, double> OnSetScore;
		public Func<string, double> OnGetScore;

        private int _blockInLine;
        public int BlockInLine
        {
            get { return _blockInLine; }
        }

        public SpriteBatch SpriteBatch
		{
			get { return _spriteBatch; }
		}

		public GameScreen[] Screens
		{
			get { return _screens.ToArray(); }
		}

		public ScreenManager(Game game, int blockInLine)
			: base(game)
		{
			_isInitialized = false;
            _blockInLine = blockInLine;

			_inputState = new InputState();

			_screens = new List<GameScreen>();
			_screenToUpdate = new List<GameScreen>();

			DataInitialize();
		}

		public void CloseApplicationMethod()
		{
			CloseApplication?.Invoke(null, null);
		}

		public void DisplayAdvertisingMethod()
		{
			DisplayAdvertising?.Invoke(null, null);
		}

        public void ShareMessageInSocialMediaMethod(SendMessageToSocialMediaEventArgs e)
        {
            ShareMessageInSocialMedia?.Invoke(null, e);
        }

        public void SharePostInSocialMediaMethod(SendPostToSocialMediaEventArgs e)
        {
            SharePostInSocialMedia?.Invoke(null, e);
        }

        public void AddScreen(GameScreen screen)
		{
			screen.ScreenManager = this;

			if (_isInitialized)
				screen.LoadContent();

			_screens.Add(screen);

			TouchPanel.EnabledGestures = screen.EnabledInput;
		}

		public void RemoveScreen(GameScreen screen)
		{
			if (_isInitialized)
				screen.UnloadContent();

			screen.Dispose();
			_screens.Remove(screen);
			_screenToUpdate.Remove(screen);

			if (_screens.Any())
				TouchPanel.EnabledGestures = _screens.Last<GameScreen>().EnabledInput;
		}

		public override void Initialize()
		{
			base.Initialize();

			_isInitialized = true;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

            Rectangle pauseBackground = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, 90, 420);
            Rectangle gameOverBackground = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, 90, 850);

            Rectangle pauseButton = new Rectangle(GraphicsDevice.Viewport.Bounds.Width - (BUTTON_PAUSE + 20), 20, BUTTON_PAUSE, BUTTON_PAUSE);
            Rectangle playButton = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, BUTTON_HEIGHT, 90, 380, Position.Direction.BOTTOM);
            Rectangle exitButton = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, BUTTON_HEIGHT, 90, 190, Position.Direction.BOTTOM);
            Rectangle tryAgainButton = Position.GetRectangle(gameOverBackground, BUTTON_HEIGHT, 90, 40, Position.Direction.TOP);
            Rectangle nextButton = Position.GetRectangle(gameOverBackground, BUTTON_HEIGHT, 90, 190, Position.Direction.BOTTOM);
            Rectangle returnButton = Position.GetRectangle(pauseBackground, BUTTON_HEIGHT, 90, 40, Position.Direction.TOP);
            Rectangle quitButton = Position.GetRectangle(pauseBackground, BUTTON_HEIGHT, 90, 190, Position.Direction.BOTTOM);

            AddTexture2D(General.Textures.Pause, Game.Content.Load<Texture2D>("pause"));
            AddTexture2D(General.Textures.Plus, Game.Content.Load<Texture2D>("plus"));
            AddTexture2D(General.Textures.Minus, Game.Content.Load<Texture2D>("minus"));
            AddTexture2D(General.Textures.Cross, Game.Content.Load<Texture2D>("cross"));
            AddTexture2D(General.Textures.Symbol, Game.Content.Load<Texture2D>("symbol"));
            AddTexture2D(General.Textures.Finish, Game.Content.Load<Texture2D>("finish"));

            AddTexture2D(General.Textures.Facebook, Game.Content.Load<Texture2D>("facebook"));
            AddTexture2D(General.Textures.Twitter, Game.Content.Load<Texture2D>("twitter"));

            AddSpriteFont(General.Fonts.SegoePrint98, Game.Content.Load<SpriteFont>("SagoePrint98"));
            AddSpriteFont(General.Fonts.SegoePrint64, Game.Content.Load<SpriteFont>("SagoePrint64"));
            AddSpriteFont(General.Fonts.SegoePrint48, Game.Content.Load<SpriteFont>("SagoePrint48"));

            AddTexture2D(General.Textures.GameBackground, General.Colors.MAIN_BACKGROUND);
            AddTexture2D(General.Textures.OpacityBackground, General.Colors.OPACITY_BACKGROUND);
            AddTexture2D(General.Textures.PauseBackground, pauseBackground, General.Colors.MAIN_BACKGROUND, 0, 45, 0);
            AddTexture2D(General.Textures.GameOverBackground, gameOverBackground, General.Colors.MAIN_BACKGROUND, 0, 45, 0);

            AddTexture2D(General.Textures.PauseButton, pauseButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            AddTexture2D(General.Textures.PlayButton, playButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            AddTexture2D(General.Textures.ExitButton, exitButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            AddTexture2D(General.Textures.TryAgainButton, tryAgainButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            AddTexture2D(General.Textures.NextButton, nextButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            AddTexture2D(General.Textures.ReturnButton, returnButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            AddTexture2D(General.Textures.QuitButton, quitButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);

            foreach (GameScreen screen in _screens)
				screen.LoadContent();
		}

		protected override void UnloadContent()
		{
			foreach (GameScreen screen in _screens)
				screen.UnloadContent();
		}

		public override void Update(GameTime gameTime)
		{
			_inputState.Update(gameTime);

			_screenToUpdate.Clear();

			foreach (GameScreen screen in _screens)
				_screenToUpdate.Add(screen);

			bool otherScreenHasFocus = !Game.IsActive;
			bool coveredByOtherScreen = false;

			while (_screenToUpdate.Count > 0)
			{
				GameScreen screen = _screenToUpdate.Last<GameScreen>();

				_screenToUpdate.RemoveAt(_screenToUpdate.Count - 1);

				screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

				if (screen.ScreenState == GameScreenState.Active)
				{
					if (!otherScreenHasFocus)
					{
						screen.HandleInput(_inputState);
						screen.UpdatePresence();

						otherScreenHasFocus = true;
					}

					if (!screen.IsPopup)
						coveredByOtherScreen = true;
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (GameScreen screen in _screens)
			{
				if (screen.ScreenState == GameScreenState.Hidden)
					continue;

				screen.Draw(gameTime);
			}
		}

		public void SetScore(string key, double value)
		{
			OnSetScore(key, value);
		}

		public double GetScore(string key)
		{
			return OnGetScore(key);
		}
	}
}