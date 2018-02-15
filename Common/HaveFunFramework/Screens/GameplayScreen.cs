using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace HaveFunFramework.Screens
{
	public class GameplayScreen : GameScreen
	{
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            EnabledGestures = GestureType.Flick;
        }

        public override void HandleInput(InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Flick)
                {
                    var x1 = gesture.Position.X;
                    var y1 = gesture.Position.Y;

                    var x2 = gesture.Position2.X;
                    var y2 = gesture.Position2.Y;

                    var deltaX1 = gesture.Delta.X;
                    var deltaY1 = gesture.Delta.Y;

                    var deltaX2 = gesture.Delta.X;
                    var deltaY2 = gesture.Delta.Y;

                    System.Diagnostics.Debug.WriteLine($"{ x1 }, { y1 } / { x2 }, { y2 } / { deltaX1 }, { deltaY1 } / { deltaX2 }, { deltaY2 }");
                }
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
