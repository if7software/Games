using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapGreenFramework.Utilities;
using System.Collections.Generic;

namespace TapGreenFramework
{
	public partial class ScreenManager
	{
		private Dictionary<string, SpriteFont> _fontDict;
		private Dictionary<string, ExTexture2D> _textureDict;

		private void DataInitialize()
		{
			_fontDict = new Dictionary<string, SpriteFont>();
			_textureDict = new Dictionary<string, ExTexture2D>();
		}

		public SpriteFont GetSpriteFont(string key)
		{
			SpriteFont font = null;

			if (string.IsNullOrWhiteSpace(key) || !_fontDict.TryGetValue(key, out font))
				return null;

			return font;
		}

		public ExTexture2D GetTexture2D(string key)
		{
			ExTexture2D texture = null;

			if (!_textureDict.TryGetValue(key, out texture))
				return null;

			return texture;
		}

		private void AddSpriteFont(string key, SpriteFont spriteFont)
		{
			if (!_fontDict.ContainsKey(key))
			{
				_fontDict[key] = spriteFont;
			}
		}

		private void AddTexture2D(string key, Texture2D texture)
		{
			if (!_textureDict.ContainsKey(key))
			{
				_textureDict[key] = new ExTexture2D(texture, new Rectangle(0, 0, texture.Width, texture.Height));
			}
		}

		private void AddTexture2D(string key, ExTexture2D texture)
		{
			if (!_textureDict.ContainsKey(key))
			{
				_textureDict[key] = texture;
			}
		}

		private void AddTexture2D(string key, Color color)
		{
			if (!_textureDict.ContainsKey(key))
			{
				int width = GraphicsDevice.Viewport.Width;
				int height = GraphicsDevice.Viewport.Height;

				Texture2D texture = new Texture2D(GraphicsDevice, width, height);
				StaticTexture2D.FillTexture2D(ref texture, width, height, color);

				_textureDict[key] = new ExTexture2D(texture, new Rectangle(0, 0, width, height));
			}
		}

		private void AddTexture2D(string key, Rectangle destination, Color color)
		{
			if (!_textureDict.ContainsKey(key))
			{
				int width = destination.Width;
				int height = destination.Height;

				Texture2D texture = new Texture2D(GraphicsDevice, width, height);
				StaticTexture2D.FillTexture2D(ref texture, width, height, color);

				_textureDict[key] = new ExTexture2D(texture, destination);
			}
		}

		private void AddTexture2D(string key, Rectangle destination, Color color, int borderThickness, int borderRadius, int borderShadow)
		{
			if (!_textureDict.ContainsKey(key))
			{
				int width = destination.Width;
				int height = destination.Height;

				Texture2D texture = new Texture2D(GraphicsDevice, width, height);
				StaticTexture2D.FillTexture2DWithRoundCorners(ref texture, width, height, color, borderThickness, borderRadius, borderShadow);

				_textureDict[key] = new ExTexture2D(texture, destination);
			}
		}
	}
}
