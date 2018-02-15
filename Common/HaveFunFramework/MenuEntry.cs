using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HaveFunFramework
{
	public class MenuEntry
	{
		private float _selectionFade;

		public float Scale { get; set; }
		public float Rotation { get; set; }

		private string _text;
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		private Rectangle _destination;
		public Rectangle Destination
		{
			get { return _destination; }
			set { _destination = value; }
		}

		public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
		{
            isSelected = false;

			float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

			if (isSelected)
				_selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
			else
				_selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
		}

		public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
		{
			Color textColor = isSelected ? Color.White : Color.Black;
			Color tintColor = isSelected ? Color.White : Color.Gray;

            isSelected = false;
            tintColor = Color.White;
            textColor = Color.Black;

			ScreenManager screenManager = screen.ScreenManager;
			SpriteBatch spriteBatch = screenManager.SpriteBatch;
			//SpriteFont font = screenManager.Font;

			//spriteBatch.Draw(screenManager.ButtonBackground, _destination, tintColor);

			//spriteBatch.DrawString(screenManager.Font, _text, getTextPosition(screen),
			//	textColor, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0);
		}

		public virtual int GetHeight(MenuScreen screen)
		{
            return 0; // screen.ScreenManager.Font.LineSpacing;
		}

		public virtual int GetWidth(MenuScreen screen)
		{
            return 0; // (int)screen.ScreenManager.Font.MeasureString(Text).X;
		}

		private Vector2 getTextPosition(MenuScreen screen)
		{
			Vector2 textPosition = Vector2.Zero;

			if (Scale == 1f)
			{
				textPosition = new Vector2(_destination.X + _destination.Width / 2 - GetWidth(screen) / 2, _destination.Y);
			}
			else
			{
				textPosition = new Vector2(_destination.X + (_destination.Width / 2 - ((GetWidth(screen) / 2) * Scale)), _destination.Y + (GetHeight(screen) - GetHeight(screen) * Scale) / 2);
			}

			return textPosition;
		}
	}
}
