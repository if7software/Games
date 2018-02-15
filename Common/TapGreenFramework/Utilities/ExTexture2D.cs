using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TapGreenFramework.Utilities
{
	public class ExTexture2D
	{
		private Texture2D _texture;
		public Texture2D Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}

		private Rectangle _destination;
		public Rectangle Destination
		{
			get { return _destination; }
			set { _destination = value; }
		}

		public Vector2 Position
		{
			get { return new Vector2(_destination.X, _destination.Y); }
		}

		public ExTexture2D(Texture2D texture, Rectangle destination)
		{
			_texture = texture;
			_destination = destination;
		}
	}
}
