using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapGreenFramework;
using TapGreenFramework.Utilities;
using TapGreenFramework.GameEngine;

namespace TapGreenFramework.Screens
{
	public class FinishScreen : GameScreen
	{
		private double _percent;
		private bool _startScreenBool;
		private TimeSpan _startScreenTime;
		private Rectangle _finishTextDest;
		private ExTexture2D _exTexture;

        private double _percentScore;

		private SpriteBatch SprintBatch
		{
			get { return ScreenManager.SpriteBatch; }
		}

		public FinishScreen(double percentScore)
		{
            _percentScore = percentScore;

			_percent = 0;
			_startScreenBool = false;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			int height = (int)(ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Height * 0.2);
			int x = ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Width / 2;
			int y = (ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Height - height) / 2;

			_finishTextDest = new Rectangle(x, y, 0, height);

			_exTexture = ScreenManager.GetTexture2D(General.Textures.Finish);
		}

		public override void HandleInput(InputState input)
		{
			base.HandleInput(input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			if (!_startScreenBool)
			{
				_startScreenTime = gameTime.TotalGameTime;
				_startScreenBool = true;
			}

			TimeSpan timeSpan = gameTime.TotalGameTime - _startScreenTime;

			if (timeSpan >= TimeSpan.Zero && timeSpan < new TimeSpan(0, 0, 0, 0, 500))
			{
			}
			else if (timeSpan >= new TimeSpan(0, 0, 0, 0, 500) && timeSpan < new TimeSpan(0, 0, 0, 1, 500))
			{
				int milisecounds = (timeSpan - new TimeSpan(0, 0, 0, 0, 500)).Milliseconds;
				_percent = milisecounds / 10;

				_finishTextDest.Width = (int)(ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Width * (_percent / 100));
				_finishTextDest.X = (ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Width - _finishTextDest.Width) / 2;
			}
			else if (timeSpan >= new TimeSpan(0, 0, 0, 1, 500) && timeSpan < new TimeSpan(0, 0, 0, 2))
			{
			}
			else
			{
				ScreenManager.DisplayAdvertisingMethod();

				ScreenManager.RemoveScreen(this);
				ScreenManager.AddScreen(new GameOverScreen(_percentScore));
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime)
		{
			SprintBatch.Begin();

			SprintBatch.Draw(_exTexture.Texture, _finishTextDest, Color.White);

			SprintBatch.End();
		}
	}
}