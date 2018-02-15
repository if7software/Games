using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapGreenFramework.Utilities;
using System;

namespace TapGreenFramework
{
	public class MenuEntry
	{
		public event EventHandler Selected;

		private int _scaleCount;
		private bool _isEnd;

		public Color TextColor { get; set; }

		public float Scale { get; set; }
		public float Rotation { get; set; }

		public string Texture { get; set; }
		public string SpriteFont { get; set; }

		public string Text { get; set; }

        public MenuEntry()
        {
            _scaleCount = 0;
            _isEnd = false;

            Scale = 1f;
            Rotation = 0f;
            TextColor = Color.White;
        }

        public MenuEntry(string text)
            : this()
        {
            Text = text;
        }

        public void Update(GameTime gameTime, MenuScreen screen)
		{
			if (_isEnd)
			{
				_isEnd = false;
				Selected?.Invoke(this, new System.EventArgs());
			}
		}

		public void Draw(GameTime gameTime, MenuScreen screen)
		{
			ScreenManager screenManager = screen.ScreenManager;
			SpriteBatch spriteBatch = screenManager.SpriteBatch;
			ExTexture2D exTexture = screenManager.GetTexture2D(Texture);
			SpriteFont spriteFont = screenManager.GetSpriteFont(SpriteFont);

			spriteBatch.Begin();

			if (_scaleCount == 0)
			{
				spriteBatch.Draw(
					exTexture.Texture,
					exTexture.Destination,
					Color.White
				);
			}
			else if (_scaleCount > 0)
			{
				float addX = (exTexture.Destination.Width * .1f) / 2;
				float addY = (exTexture.Destination.Height * .1f) / 2;

				spriteBatch.Draw(exTexture.Texture, new Vector2(exTexture.Destination.X + addX, exTexture.Destination.Y + addY), null, Color.White, 0f, Vector2.Zero, new Vector2(.9f, .9f), SpriteEffects.None, 0f);

				if (_scaleCount == 1)
					_isEnd = true;

				_scaleCount--;
			}

            if (!string.IsNullOrWhiteSpace(Text))
            {
                spriteBatch.DrawString(
                    spriteFont, Text, Position.GetTextPosition(exTexture.Destination, spriteFont, Text),
                    TextColor, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }

			spriteBatch.End();
		}

		protected internal void OnSelectEntry()
		{
			_scaleCount = 3;
		}


	}
}