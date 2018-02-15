using System;
using TapGreenFramework.GameEngine;
using TapGreenFramework.GameEngine.Enums;

namespace TapGreenFramework.Games
{
    public class StrengthGame : TapGreenEngine
    {
        public StrengthGame(ScreenManager screenManager)
            : base(screenManager)
        {
        }

        protected override void AfterPause(TimeSpan time)
        {
        }

        protected override void SetScore(int points, bool? plusOrMinus)
        {
        }

        protected override void TapButton(BlockType type)
        {
        }
    }
}
