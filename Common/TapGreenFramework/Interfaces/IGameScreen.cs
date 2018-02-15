using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace TapGreenFramework.Interfaces
{
	public interface IGameScreen
	{
		bool IsPopup { get; }
		GestureType EnabledInput { get; }

		void Initialize();

		void HandleInput(InputState input);
		void UpdatePresence();
		void CloseScreen();

		void LoadContent();
		void UnloadContent();

		void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen);
		void Draw(GameTime gameTime);
	}
}