using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TapGreenFramework.Utilities
{
	public static class Position
	{
		public enum Direction
		{
			TOP,
			BOTTOM
		}

		public static Vector2 GetTextPosition(Rectangle rect, SpriteFont spriteFont, string text)
		{
			return GetTextPosition(rect, GetTextWidth(spriteFont, text), GetTextHeight(spriteFont), 1f);
		}

		public static Vector2 GetTextPosition(Rectangle rect, int screenWidth, int screenHeight, float scale)
		{
			Vector2 textPosition = Vector2.Zero;

			if (scale == 1f)
				textPosition = new Vector2(rect.X + rect.Width / 2 - screenWidth / 2, rect.Y);
			else
				textPosition = new Vector2(rect.X + (rect.Width / 2 - ((screenWidth / 2) * scale)), rect.Y + (screenHeight - screenHeight * scale) / 2);

			return textPosition;
		}

		public static Vector2 GetTextPosition(Rectangle rect, SpriteFont spriteFont, string text, int top)
		{
			return GetTextPosition(rect, GetTextWidth(spriteFont, text), GetTextHeight(spriteFont), top, 1f);
		}

		public static Vector2 GetTextPosition(Rectangle rect, int screenWidth, int screenHeight, int top, float scale)
		{
			Vector2 textPosition = Vector2.Zero;

			if (scale == 1f)
				textPosition = new Vector2(rect.X + rect.Width / 2 - screenWidth / 2, rect.Y + top);
			else
				textPosition = new Vector2(rect.X + (rect.Width / 2 - ((screenWidth / 2) * scale)), rect.Y + top + (screenHeight - screenHeight * scale) / 2);

			return textPosition;
		}

		public static int GetTextHeight(MenuScreen screen, string spriteFont)
		{
			return screen.ScreenManager.GetSpriteFont(spriteFont).LineSpacing;
		}

		public static int GetTextWidth(MenuScreen screen, string text, string spriteFont)
		{
			return (int)screen.ScreenManager.GetSpriteFont(spriteFont).MeasureString(text).X;
		}

		public static int GetTextHeight(SpriteFont spriteFont)
		{
			return spriteFont.LineSpacing;
		}

		public static int GetTextWidth(SpriteFont spriteFont, string text)
		{
			return (int)spriteFont.MeasureString(text).X;
		}

		public static Rectangle GetRectangle(Rectangle screen, int widthPercent, int height)
		{
			int w = widthPercent != 0 ? (int)(screen.Width * ((double)widthPercent / 100)) + screen.X : screen.Width;
			int h = height;
			int x = (screen.Width - w) / 2;
			int y = (screen.Height - h) / 2;

			return new Rectangle(x, y, w, h);
		}

		public static Rectangle GetRectangle(Rectangle panel, int height, int widthPercent, int value, Direction direct)
		{
			int w = widthPercent != 0 ? (int)(panel.Width * ((double)widthPercent / 100)) : panel.Width;
			int h = height;
			int x = ((panel.Width - w) / 2) + panel.X;
			int y = 0;

			switch (direct)
			{
				case Direction.TOP:
					y = value + panel.Y;
					break;
				case Direction.BOTTOM:
					y = (panel.Height - value) + panel.Y;
					break;
			}

			return new Rectangle(x, y, w, h);
		}
	}
}
