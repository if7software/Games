using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TapGreenFramework
{
	public static class General
	{
        public enum SocialMediaEnum
        {
            FACEBOOK,
            TWITTER
        }

        public static List<Tuple<int, int>> GetOffsetBound(int blockInLine)
        {
            switch (blockInLine)
            {
                case 3:
                    return OffsetBound3x3;
                case 4:
                    return OffsetBound4x4;
                default:
                    throw new ArgumentException("Invalid number block in line");
            }
        }

        public static readonly List<Tuple<int, int>> OffsetBound3x3 = new List<Tuple<int, int>>
		{
			new Tuple<int, int>(900, 1400),
			new Tuple<int, int>(800, 1300),
			new Tuple<int, int>(700, 1200),
			new Tuple<int, int>(600, 1100),
			new Tuple<int, int>(500, 1000),
			new Tuple<int, int>(400, 900),
			new Tuple<int, int>(300, 800),
			new Tuple<int, int>(200, 700),
			new Tuple<int, int>(100, 600),
			new Tuple<int, int>(0 ,500)
		};

		public static readonly List<Tuple<int, int>> OffsetBound4x4 = new List<Tuple<int, int>>
		{
			new Tuple<int, int>(900, 1400),
			new Tuple<int, int>(800, 1280),
			new Tuple<int, int>(700, 1160),
			new Tuple<int, int>(600, 1040),
			new Tuple<int, int>(500, 920),
			new Tuple<int, int>(400, 800),
			new Tuple<int, int>(300, 680),
			new Tuple<int, int>(200, 560),
			new Tuple<int, int>(100, 440),
			new Tuple<int, int>(0 ,320)
		};

		public static class Textures
		{
            //Png icons
            public static readonly string Cross = "cross";
            public static readonly string Symbol = "symbol";
            public static readonly string Plus = "plus";
            public static readonly string Minus = "minus";
            public static readonly string Pause = "pause";
            public static readonly string Finish = "finish";

            //Backgrounds
            public static readonly string GameBackground = "gameBackground";
			public static readonly string OpacityBackground = "opacityBackground";
			public static readonly string GameOverBackground = "gameOverBackground";
			public static readonly string PauseBackground = "pauseBackground";

            //Buttons
            public static readonly string PauseButton = "pauseButton";
            public static readonly string PlayButton = "playButton";
            public static readonly string ExitButton = "exitButton";
            public static readonly string TryAgainButton = "tryAgainButton";
            public static readonly string NextButton = "nextButton";
            public static readonly string ReturnButton = "returnButton";
            public static readonly string QuitButton = "quitButton";

            //Social media
            public static readonly string Facebook = "facebook";
            public static readonly string Twitter = "twitter";
		}

		public static class Fonts
		{
			public static readonly string SegoePrint48 = "SegoePrint48";
			public static readonly string SegoePrint64 = "SegoePrint64";
			public static readonly string SegoePrint98 = "SegoePrint98";
		}

		public static class TimePeriod
		{
			public static readonly int PERIOD_0 = 0;
			public static readonly int PERIOD_1 = 5;
			public static readonly int PERIOD_2 = 10;
			public static readonly int PERIOD_3 = 15;
			public static readonly int PERIOD_4 = 20;
			public static readonly int PERIOD_5 = 25;
			public static readonly int PERIOD_6 = 30;
			public static readonly int PERIOD_7 = 35;
			public static readonly int PERIOD_8 = 40;
			public static readonly int PERIOD_9 = 45;
		}

		public static class Colors
		{
			public static readonly Color BUTTON_BACKGROUND = new Color(140, 168, 135);
			public static readonly Color OPACITY_BACKGROUND = new Color(0, 0, 0, 128);
			public static readonly Color MAIN_BACKGROUND = new Color(200, 210, 146);
			public static readonly Color BLOCK_RED = new Color(255, 160, 122);
			public static readonly Color BLOCK_GREEN = new Color(144, 215, 144);
			//public static readonly Color BLOCK_EMPTY = new Color(143, 188, 143);
			public static readonly Color BLOCK_EMPTY = new Color(140, 168, 135);
			public static readonly Color SCORE_FONT = new Color(153, 153, 66);
		}

		public static class ScoreKeys
		{
			public static readonly string PointScore = "pointScore";
			public static readonly string PercentScore = "percentScore";
		}
	}
}