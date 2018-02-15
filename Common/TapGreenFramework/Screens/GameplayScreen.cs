using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using TapGreenFramework.Games;

namespace TapGreenFramework.Screens
{
	public class GameplayScreen : GameScreen
	{
		private bool _returnAfterPause;

		private TimeSpan _timePause;

		private TimeoutGame _trafficGame;

		public GameplayScreen()
		{
			EnabledInput = GestureType.Tap;

			_returnAfterPause = false;
			_timePause = new TimeSpan();
		}

		public override void LoadContent()
		{
            _trafficGame = new TimeoutGame(ScreenManager);

			if (_trafficGame != null)
			{
				_trafficGame.LoadContent();
				_trafficGame.GameClosed += new EventHandler(GameClosedEventArgs);
			}

			base.LoadContent();
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		public override void Back(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				PauseCurrentGame(gameTime);
		}

		public override void HandleInput(InputState input)
		{
			foreach (GestureSample gesture in input.Gestures)
			{
				if (gesture.GestureType == GestureType.Tap)
				{
					_trafficGame.HandleInput(new Point((int)gesture.Position.X, (int)gesture.Position.Y));
				}
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			if (_returnAfterPause)
			{
				TimeSpan time = gameTime.TotalGameTime - _timePause;
				_trafficGame.AddTimeAfterPause(time);
				_returnAfterPause = false;
			}

			if (_trafficGame != null)
			{
				_trafficGame.Update(gameTime);
			}

			base.Update(gameTime, otherScreenHasFocus, false);
		}

		internal void ReturnAfterPause()
		{
			_returnAfterPause = true;
			_trafficGame.IsPaused = false;
		}

		public override void CloseScreen()
		{
			if (_trafficGame != null)
			{
				_trafficGame.GameClosed -= new EventHandler(GameClosedEventArgs);
			}

			_trafficGame = null;

			base.CloseScreen();
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			if (_trafficGame != null)
			{
				_trafficGame.Draw(gameTime);
			}
		}

		private void GameClosedEventArgs(object sender, System.EventArgs e)
		{
			_trafficGame.IsPaused = true;
            var percentScore = (double)sender;

			ScreenManager.AddScreen(new FinishScreen(percentScore));
		}

		private void PauseCurrentGame(GameTime gameTime)
		{
			_trafficGame.IsPaused = true;
			_timePause = gameTime.TotalGameTime;

			ScreenManager.AddScreen(new PauseScreen());
		}
	}
}