using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TapGreenFramework.Utilities;
using TapGreenFramework.GameEngine;
using System;
using TapGreenFramework.EventArgs;

namespace TapGreenFramework.Screens
{
	public class GameOverScreen : MenuScreen
	{
		private string _textEfficiency;
		private string _textPercent;

        private double _percentScore;

		private SpriteBatch SprintBatch
		{
			get { return ScreenManager.SpriteBatch; }
		}

		public GameOverScreen(double percentScore)
		{
            _percentScore = percentScore;
		}

		private void NewGameMenuEntrySelected(object sender, System.EventArgs e)
		{
			foreach (GameScreen screen in ScreenManager.Screens)
				screen.CloseScreen();

			ScreenManager.AddScreen(new BackgroundScreen());
			ScreenManager.AddScreen(new GameplayScreen());
		}

		private void QuitGameMenuEntrySelected(object sender, System.EventArgs e)
		{
			SkipToMainMenuScreen();
		}

        private void TwitterMenuEntrySelected(object sender, System.EventArgs e)
        {
            ScreenManager.ShareMessageInSocialMediaMethod(
                new SendMessageToSocialMediaEventArgs
                {
                    Message = $"Kocham Ciem",
                    SocialMedia = General.SocialMediaEnum.TWITTER
                });
        }

        private void FacebookMenuEntrySelected(object sender, System.EventArgs e)
        {
            ScreenManager.ShareMessageInSocialMediaMethod(
                new SendMessageToSocialMediaEventArgs
                {
                    Message = $"Kocham Ciem",
                    SocialMedia = General.SocialMediaEnum.FACEBOOK
                });
        }

        private void SkipToMainMenuScreen()
		{
			for (int i = 0; i < ScreenManager.Game.Components.Count; i++)
			{
				if (!(ScreenManager.Game.Components[i] is ScreenManager))
				{
					if (ScreenManager.Game.Components[i] is DrawableGameComponent)
					{
						(ScreenManager.Game.Components[i] as IDisposable).Dispose();
						i--;
					}
					else
					{
						ScreenManager.Game.Components.RemoveAt(i);
						i--;
					}
				}
			}

			foreach (GameScreen screen in ScreenManager.Screens)
				screen.CloseScreen();

			ScreenManager.AddScreen(new BackgroundScreen());
			ScreenManager.AddScreen(new MainMenuScreen());
		}

		public override void LoadContent()
		{
			MenuEntry newGameMenuEntry = new MenuEntry("Try again")
			{
				SpriteFont = General.Fonts.SegoePrint64,
				Texture = General.Textures.TryAgainButton
			};

			MenuEntry quitGameMenuEntry = new MenuEntry("Next")
			{
				SpriteFont = General.Fonts.SegoePrint64,
				Texture = General.Textures.NextButton
			};

            MenuEntry facebookMenuEntry = new MenuEntry()
            {
                Texture = General.Textures.Facebook
            };

            MenuEntry twitterMenuEntry = new MenuEntry()
            {
                Texture = General.Textures.Twitter
            };

            newGameMenuEntry.Selected += NewGameMenuEntrySelected;
			quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            facebookMenuEntry.Selected += FacebookMenuEntrySelected;
            twitterMenuEntry.Selected += TwitterMenuEntrySelected;

            MenuEntries.Add(newGameMenuEntry);
			MenuEntries.Add(quitGameMenuEntry);
            MenuEntries.Add(facebookMenuEntry);
            MenuEntries.Add(twitterMenuEntry);

			double bestPercent = ScreenManager.GetScore(General.ScoreKeys.PercentScore);

			if (_percentScore > bestPercent)
			{
				bestPercent = _percentScore;
				ScreenManager.SetScore(General.ScoreKeys.PercentScore, _percentScore);
			}

			_textEfficiency = "Efficiency:";
			_textPercent = string.Format("{0:N1}%", _percentScore);

			base.LoadContent();
		}

        public override void Back(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				SkipToMainMenuScreen();
		}

		public override void HandleInput(InputState input)
		{
			base.HandleInput(input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			foreach (var menuEntry in MenuEntries)
				menuEntry.Update(gameTime, this);
		}

		public override void Draw(GameTime gameTime)
		{
			ExTexture2D exTexture = ScreenManager.GetTexture2D(General.Textures.GameOverBackground);

			SprintBatch.Begin();

			SprintBatch.Draw(ScreenManager.GetTexture2D(General.Textures.OpacityBackground).Texture, Vector2.Zero, Color.White);
			SprintBatch.Draw(exTexture.Texture, exTexture.Position, Color.White);

			SpriteFont segoePrint48 = ScreenManager.GetSpriteFont(General.Fonts.SegoePrint48);
			SpriteFont segoePrint64 = ScreenManager.GetSpriteFont(General.Fonts.SegoePrint64);

			SprintBatch.DrawString(
				segoePrint48, _textEfficiency, Position.GetTextPosition(exTexture.Destination, segoePrint48, _textEfficiency, 220),
				General.Colors.SCORE_FONT, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			SprintBatch.DrawString(
				segoePrint64, _textPercent, Position.GetTextPosition(exTexture.Destination, segoePrint64, _textPercent, 300),
				General.Colors.SCORE_FONT, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			SprintBatch.End();

			foreach (var menuEntry in MenuEntries)
				menuEntry.Draw(gameTime, this);
		}
	}
}