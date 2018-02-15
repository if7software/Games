using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using HaveFunFramework.Enums;

namespace HaveFunFramework
{
    public class ScreenManager : DrawableGameComponent
	{
		bool _isInitialized;

		public InputState _input = new InputState();

		private List<GameScreen> _screens = new List<GameScreen>();
		private List<GameScreen> _screensToUpdate = new List<GameScreen>();

		private SpriteBatch _spriteBatch;
		public SpriteBatch SpriteBatch
		{
			get { return _spriteBatch; }
		}

        public GameScreen[] Screens
        {
            get { return _screens.ToArray(); }
        }

        public Rectangle SafeArea
		{
			get { return Game.GraphicsDevice.Viewport.TitleSafeArea; }
		}

		public ScreenManager(Game game)
			: base(game)
		{
            TouchPanel.EnabledGestures = GestureType.None;
		}

		public override void Initialize()
		{
			base.Initialize();

			_isInitialized = true;
		}

		protected override void LoadContent()
		{
			ContentManager content = Game.Content;

			_spriteBatch = new SpriteBatch(GraphicsDevice);

			foreach (GameScreen screen in _screens)
			{
				screen.LoadContent();
			}
		}

		protected override void UnloadContent()
		{
			foreach (GameScreen screen in _screens)
			{
				screen.UnloadContent();
			}
		}

		public override void Update(GameTime gameTime)
		{
			_input.Update();

			_screensToUpdate.Clear();

			foreach (GameScreen screen in _screens)
				_screensToUpdate.Add(screen);

			bool otherScreenHasFocus = !Game.IsActive;
			bool coveredByOtherScreen = false;

			while (_screensToUpdate.Count > 0)
			{
				GameScreen screen = _screensToUpdate[_screensToUpdate.Count - 1];

				_screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

				screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

				if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
				{
					if (!otherScreenHasFocus)
					{
						screen.HandleInput(_input);

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
				if (screen.ScreenState == ScreenState.Hidden)
					continue;

				screen.Draw(gameTime);
			}
		}

		public void AddScreen(GameScreen screen)
		{
			screen.ScreenManager = this;
			screen.IsExiting = false;

			if (_isInitialized)
				screen.LoadContent();

			_screens.Add(screen);

            TouchPanel.EnabledGestures = screen.EnabledGestures;
		}

		public void RemoveScreen(GameScreen screen)
		{
			if (_isInitialized)
				screen.UnloadContent();

			_screens.Remove(screen);
			_screensToUpdate.Remove(screen);

            if (_screens.Count > 0)
            {
                TouchPanel.EnabledGestures = _screens[_screens.Count - 1].EnabledGestures;
            }
		}

		public void FadeBackBufferToBlack(float alpha)
		{
			Viewport viewport = GraphicsDevice.Viewport;

			_spriteBatch.Begin();

			//_spriteBatch.Draw(_blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * alpha);

			_spriteBatch.End();
		}
	}
}
