using HaveFunFramework.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HaveFunFramework
{
	public abstract class MenuScreen : GameScreen
	{
		private const int menuEntryPadding = 35;

		private int selectedEntry = 0;

		private List<MenuEntry> menuEntries = new List<MenuEntry>();

		private Rectangle bounds;
		//private string menuTitle;

		public override void LoadContent()
		{
			bounds = ScreenManager.SafeArea;

			base.LoadContent();
		}

		protected virtual void UpdateMenuEntryLocations()
		{
			float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

			Vector2 position = new Vector2(0f, ScreenManager.Game.Window.ClientBounds.Height / 2 - (menuEntries[0].GetHeight(this) + (menuEntryPadding * 2) * menuEntries.Count));

			for (int i = 0; i < menuEntries.Count; i++)
			{
				MenuEntry menuEntry = menuEntries[i];

				position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

				if (ScreenState == ScreenState.TransitionOn)
					position.X -= transitionOffset * 256;
				else
					position.X += transitionOffset * 512;

				position.Y += menuEntry.GetHeight(this) + (menuEntryPadding * 2);
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			for (int i = 0; i < menuEntries.Count; i++)
			{
				bool isSelected = IsActive && (i == selectedEntry);
				UpdateMenuEntryDestination();
				menuEntries[i].Update(this, isSelected, gameTime);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice graphics = ScreenManager.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			//SpriteFont font = ScreenManager.Font;

			spriteBatch.Begin();

			for (int i = 0; i < menuEntries.Count; i++)
			{
				MenuEntry menuEntry = menuEntries[i];

				bool isSelected = IsActive && (i == selectedEntry);

				menuEntry.Draw(this, isSelected, gameTime);
			}

			float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

			Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 375);
			//Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
			Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
			//float titleScale = 1.25f;

			titlePosition.Y -= transitionOffset * 100;

			//spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

			spriteBatch.End();
		}

		public void UpdateMenuEntryDestination()
		{
			Rectangle bounds = ScreenManager.SafeArea;
			//Rectangle textureSize = ScreenManager.ButtonBackground.Bounds;

			int xStep = bounds.Width / (menuEntries.Count + 2);
			int maxWidth = 0;

			for (int i = 0; i < menuEntries.Count; i++)
			{
				int width = menuEntries[i].GetWidth(this);
				if (width > maxWidth)
				{
					maxWidth = width;
				}
			}

			maxWidth += 20;

			for (int i = 0; i < menuEntries.Count; i++)
			{
				//menuEntries[i].Destination = new Rectangle(bounds.Left + (xStep - textureSize.Width) / 2 + (i + 1) * xStep, bounds.Bottom - textureSize.Height * 2, maxWidth, 50);
			}
		}
	}
}
