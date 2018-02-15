using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TapGreenFramework;
using TapGreenFramework.Utilities;
using System;
using System.Collections.Generic;

namespace TapGreenFramework.Screens
{
	public class PauseScreen : MenuScreen
	{
		private SpriteBatch SprintBatch
		{
			get { return ScreenManager.SpriteBatch; }
		}

		private void ReturnGameMenuEntrySelected(object sender, System.EventArgs e)
		{
			SkipToGameplayScreen();
		}

		private void SkipToGameplayScreen()
		{
			List<GameScreen> toCloseed = new List<GameScreen>();
			GameplayScreen gameplay = null;

			foreach (GameScreen screen in ScreenManager.Screens)
			{
				if (screen is GameplayScreen)
				{
					gameplay = (GameplayScreen)screen;
				}
				else if (screen is BackgroundScreen)
				{

				}
				else
				{
					toCloseed.Add(screen);
				}
			}

			foreach (GameScreen screen in toCloseed)
				screen.CloseScreen();

			gameplay.ReturnAfterPause();
		}

		private void QuitGameMenuEntrySelected(object sender, System.EventArgs e)
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
			MenuEntry returnGameMenuEntry = new MenuEntry("Return")
			{
				SpriteFont = General.Fonts.SegoePrint64,
				Texture = General.Textures.ReturnButton
			};

			MenuEntry quitGameMenuEntry = new MenuEntry("Quit")
			{
				SpriteFont = General.Fonts.SegoePrint64,
				Texture = General.Textures.QuitButton
			};

			returnGameMenuEntry.Selected += ReturnGameMenuEntrySelected;
			quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

			MenuEntries.Add(returnGameMenuEntry);
			MenuEntries.Add(quitGameMenuEntry);

			base.LoadContent();
		}

		public override void Back(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				SkipToGameplayScreen();
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
			ExTexture2D exTexture = ScreenManager.GetTexture2D(General.Textures.PauseBackground);

			SprintBatch.Begin();

			SprintBatch.Draw(ScreenManager.GetTexture2D(General.Textures.OpacityBackground).Texture, Vector2.Zero, Color.White);
			SprintBatch.Draw(exTexture.Texture, exTexture.Position, Color.White);

			SprintBatch.End();

			foreach (var menuEntry in MenuEntries)
				menuEntry.Draw(gameTime, this);
		}
	}
}