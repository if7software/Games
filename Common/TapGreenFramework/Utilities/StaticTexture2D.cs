using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TapGreenFramework.Utilities
{
	public static class StaticTexture2D
	{
		public static void FillTexture2D(ref Texture2D texture, int width, int height, Color color)
		{
			Color[] data = new Color[width * height];

			for (int i = 0; i < data.Length; ++i)
				data[i] = color;

			texture.SetData(data);
		}

		public static void FillTexture2DWithRoundCorners(ref Texture2D texture, int width, int height, Color color, int borderThickness, int borderRadius, int borderShadow)
		{
			texture.SetData(GetColorArray(width, height, color, borderThickness, borderRadius, borderShadow));
		}

		public static Color[] GetColorArray(int width, int height, Color color, int borderThickness, int borderRadius, int borderShadow)
		{
			List<Color> borderColors = new List<Color> { Color.White };

			float initialShadowIntensity = 0;
			float finalShadowIntensity = 0;

			Color[] colorData = new Color[width * height];

			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
				{
					colorData[x + width * y] = color;

					colorData[x + width * y] = ColorBorder(x, y, width, height, borderThickness, borderRadius, borderShadow, colorData[x + width * y], borderColors, initialShadowIntensity, finalShadowIntensity);
				}

			return colorData;
		}

		private static Color ColorBorder(int x, int y, int width, int height, int borderThickness, int borderRadius, int borderShadow, Color initialColor, List<Color> borderColors, float initialShadowIntensity, float finalShadowIntensity)
		{
			Rectangle internalRectangle = new Rectangle((borderThickness + borderRadius), (borderThickness + borderRadius), width - 2 * (borderThickness + borderRadius), height - 2 * (borderThickness + borderRadius));

			if (internalRectangle.Contains(x, y))
				return initialColor;

			Vector2 origin = Vector2.Zero;
			Vector2 point = new Vector2(x, y);

			if (x < borderThickness + borderRadius)
			{
				if (y < borderRadius + borderThickness)
					origin = new Vector2(borderRadius + borderThickness, borderRadius + borderThickness);
				else if (y > height - (borderRadius + borderThickness))
					origin = new Vector2(borderRadius + borderThickness, height - (borderRadius + borderThickness));
				else
					origin = new Vector2(borderRadius + borderThickness, y);
			}
			else if (x > width - (borderRadius + borderThickness))
			{
				if (y < borderRadius + borderThickness)
					origin = new Vector2(width - (borderRadius + borderThickness), borderRadius + borderThickness);
				else if (y > height - (borderRadius + borderThickness))
					origin = new Vector2(width - (borderRadius + borderThickness), height - (borderRadius + borderThickness));
				else
					origin = new Vector2(width - (borderRadius + borderThickness), y);
			}
			else
			{
				if (y < borderRadius + borderThickness)
					origin = new Vector2(x, borderRadius + borderThickness);
				else if (y > height - (borderRadius + borderThickness))
					origin = new Vector2(x, height - (borderRadius + borderThickness));
			}

			if (!origin.Equals(Vector2.Zero))
			{
				float distance = Vector2.Distance(point, origin);

				if (distance > borderRadius + borderThickness + 1)
				{
					return Color.Transparent;
				}
				else if (distance > borderRadius + 1)
				{
					if (borderColors.Count > 2)
					{
						float modNum = distance - borderRadius;

						if (modNum < borderThickness / 2)
						{
							return Color.Lerp(borderColors[2], borderColors[1], (float)((modNum) / (borderThickness / 2.0)));
						}
						else
						{
							return Color.Lerp(borderColors[1], borderColors[0], (float)((modNum - (borderThickness / 2.0)) / (borderThickness / 2.0)));
						}
					}

					if (borderColors.Count > 0)
						return borderColors[0];
				}
				else if (distance > borderRadius - borderShadow + 1)
				{
					float mod = (distance - (borderRadius - borderShadow)) / borderShadow;
					float shadowDiff = initialShadowIntensity - finalShadowIntensity;
					return DarkenColor(initialColor, ((shadowDiff * mod) + finalShadowIntensity));
				}
			}

			return initialColor;
		}

		private static Color DarkenColor(Color color, float shadowIntensity)
		{
			return Color.Lerp(color, Color.Black, shadowIntensity);
		}
	}
}
