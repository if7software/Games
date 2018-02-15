using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapGreenFramework;
using TapGreenFramework.Utilities;
using TapGreenFramework.Screens;
using System;
using Android.OS;

namespace TapGreen
{
    public class MonoGame : Game
    {
        private const int BUTTON_HEIGHT = 150;
        private const int BUTTON_PAUSE = 100;

        private GraphicsDeviceManager _graphics;
        private ScreenManager _screenManager;
        private MainActivity _mainActivity;

        public MonoGame(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _screenManager = new ScreenManager(this, CommonVariables.BlockInLine);
            _screenManager.AddScreen(new BackgroundScreen());
            _screenManager.AddScreen(new MainMenuScreen());
            _screenManager.ShowAds += ShowAdsEvent;
            _screenManager.CloseApplication += CloseApplicationEvent;
            _screenManager.OnGetScore = delegate (string key) { return SharedPreferences.GetStorageIntValue(CommonVariables.GameName, key); };
            _screenManager.OnSetScore = delegate (string key, int value) { SharedPreferences.SetStorageIntValue(CommonVariables.GameName, key, value); };

            Components.Add(_screenManager);

            _graphics.IsFullScreen = true;
            //graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Rectangle pauseBackground = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, 90, 420);
            Rectangle gameOverBackground = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, 90, 850);

            Rectangle pauseButton = new Rectangle(GraphicsDevice.Viewport.Bounds.Width - (BUTTON_PAUSE + 20), 20, BUTTON_PAUSE, BUTTON_PAUSE);
            Rectangle playButton = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, BUTTON_HEIGHT, 90, 380, Position.Direction.BOTTOM);
            Rectangle exitButton = Position.GetRectangle(GraphicsDevice.Viewport.Bounds, BUTTON_HEIGHT, 90, 190, Position.Direction.BOTTOM);
            Rectangle tryAgainButton = Position.GetRectangle(gameOverBackground, BUTTON_HEIGHT, 90, 40, Position.Direction.TOP);
            Rectangle nextButton = Position.GetRectangle(gameOverBackground, BUTTON_HEIGHT, 90, 190, Position.Direction.BOTTOM);
            Rectangle returnButton = Position.GetRectangle(pauseBackground, BUTTON_HEIGHT, 90, 40, Position.Direction.TOP);
            Rectangle quitButton = Position.GetRectangle(pauseBackground, BUTTON_HEIGHT, 90, 190, Position.Direction.BOTTOM);

            _screenManager.AddSpriteFont(General.Fonts.SegoePrint98, Content.Load<SpriteFont>("SagoePrint98"));
            _screenManager.AddSpriteFont(General.Fonts.SegoePrint64, Content.Load<SpriteFont>("SagoePrint64"));
            _screenManager.AddSpriteFont(General.Fonts.SegoePrint48, Content.Load<SpriteFont>("SagoePrint48"));

            _screenManager.AddTexture2D(General.Textures.GameBackground, General.Colors.MAIN_BACKGROUND);
            _screenManager.AddTexture2D(General.Textures.OpacityBackground, General.Colors.OPACITY_BACKGROUND);
            _screenManager.AddTexture2D(General.Textures.PauseBackground, pauseBackground, General.Colors.MAIN_BACKGROUND, 0, 45, 0);
            _screenManager.AddTexture2D(General.Textures.GameOverBackground, gameOverBackground, General.Colors.MAIN_BACKGROUND, 0, 45, 0);

            _screenManager.AddTexture2D(General.Textures.PauseButton, pauseButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            _screenManager.AddTexture2D(General.Textures.PlayButton, playButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            _screenManager.AddTexture2D(General.Textures.ExitButton, exitButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            _screenManager.AddTexture2D(General.Textures.TryAgainButton, tryAgainButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            _screenManager.AddTexture2D(General.Textures.NextButton, nextButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            _screenManager.AddTexture2D(General.Textures.ReturnButton, returnButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);
            _screenManager.AddTexture2D(General.Textures.QuitButton, quitButton, General.Colors.BUTTON_BACKGROUND, 0, 30, 0);

            _screenManager.AddTexture2D(General.Textures.Pause, Content.Load<Texture2D>("pause"));
            _screenManager.AddTexture2D(General.Textures.Plus, Content.Load<Texture2D>("plus"));
            _screenManager.AddTexture2D(General.Textures.Minus, Content.Load<Texture2D>("minus"));
            _screenManager.AddTexture2D(General.Textures.Cross, Content.Load<Texture2D>("cross"));
            _screenManager.AddTexture2D(General.Textures.Symbol, Content.Load<Texture2D>("symbol"));
            _screenManager.AddTexture2D(General.Textures.Finish, Content.Load<Texture2D>("finish"));
        }

        private void ShowAdsEvent(object sender, EventArgs eventArgs)
        {
            _mainActivity.CallInterstitial();
        }

        private void CloseApplicationEvent(object sender, EventArgs e)
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}
