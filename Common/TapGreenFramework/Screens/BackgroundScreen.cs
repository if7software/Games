using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TapGreenFramework.Screens
{
	public class BackgroundScreen : GameScreen
	{
		private SpriteBatch SprintBatch
		{
			get { return ScreenManager.SpriteBatch; }
		}

		public BackgroundScreen()
		{
			
		}

		public override void LoadContent()
		{
			base.LoadContent();
		}

		public override void HandleInput(InputState input)
		{
			base.HandleInput(input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);
		}

		public override void Draw(GameTime gameTime)
		{
			SprintBatch.Begin();

			SprintBatch.Draw(ScreenManager.GetTexture2D(General.Textures.GameBackground).Texture, Vector2.Zero, Color.White);

			SprintBatch.End();
		}
	}
}