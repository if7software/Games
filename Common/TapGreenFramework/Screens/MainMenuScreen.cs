using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TapGreenFramework.Utilities;

namespace TapGreenFramework.Screens
{
	public class MainMenuScreen : MenuScreen
	{
		private string _textPercent;
		private string _textEfficiency;

		public MainMenuScreen()
		{

		}

		private void StartGameMenuEntrySelected(object sender, System.EventArgs e)
		{
			foreach (GameScreen screen in ScreenManager.Screens)
				screen.CloseScreen();

			ScreenManager.AddScreen(new BackgroundScreen());
			ScreenManager.AddScreen(new GameplayScreen());
		}

		private void OnCancel(object sender, System.EventArgs e)
		{
			ScreenManager.CloseApplicationMethod();
		}

		public override void LoadContent()
		{
			MenuEntry startGameMenuEntry = new MenuEntry("Play")
			{
				SpriteFont = General.Fonts.SegoePrint64,
				Texture = General.Textures.PlayButton
			};

			MenuEntry exitMenuEntry = new MenuEntry("Exit")
			{
				SpriteFont = General.Fonts.SegoePrint64,
				Texture = General.Textures.ExitButton
			};

			startGameMenuEntry.Selected += StartGameMenuEntrySelected;
			exitMenuEntry.Selected += OnCancel;

			MenuEntries.Add(startGameMenuEntry);
			MenuEntries.Add(exitMenuEntry);

			double bestPercent = ScreenManager.GetScore(General.ScoreKeys.PercentScore);

			_textEfficiency = "Best efficiency:";
			_textPercent = string.Format("{0:N1}%", bestPercent);

			base.LoadContent();
		}

		public override void Back(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				ScreenManager.CloseApplicationMethod();
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			foreach (var menuEntry in MenuEntries)
				menuEntry.Update(gameTime, this);

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice graphics = ScreenManager.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

			spriteBatch.Begin();

			SpriteFont segoePrint98 = ScreenManager.GetSpriteFont(General.Fonts.SegoePrint98);
			SpriteFont segoePrint64 = ScreenManager.GetSpriteFont(General.Fonts.SegoePrint64);
			SpriteFont segoePrint48 = ScreenManager.GetSpriteFont(General.Fonts.SegoePrint48);

			spriteBatch.DrawString(
				segoePrint48, _textEfficiency, Position.GetTextPosition(ScreenManager.GraphicsDevice.Viewport.Bounds, segoePrint48, _textEfficiency, 30),
				General.Colors.SCORE_FONT, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(
				segoePrint98, _textPercent, Position.GetTextPosition(ScreenManager.GraphicsDevice.Viewport.Bounds, segoePrint98, _textPercent, 90),
				General.Colors.SCORE_FONT, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.End();

			foreach (var menuEntry in MenuEntries)
				menuEntry.Draw(gameTime, this);
		}
	}
}