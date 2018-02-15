using Microsoft.Xna.Framework;
using TapGreenFramework;
using TapGreenFramework.Screens;
using Android.OS;
using Android.Gms.Ads;

namespace TapGreen
{
	public class MonoGame : Game
	{
        private static InterstitialAd _adInterstitial;
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
            _screenManager.DisplayAdvertising += (sender, e) => { CallInterstitial(); };
            _screenManager.CloseApplication += (sender, e) => { Process.KillProcess(Process.MyPid()); }; ;
            _screenManager.SharePostInSocialMedia += (sender, e) => { _mainActivity.FacebookAuthentication(); };
            _screenManager.ShareMessageInSocialMedia += (sender, e) => { _mainActivity.ShareMessageInSocialMedia(e.Subject, e.Message, e.SocialMedia); };
            _screenManager.OnGetScore = delegate (string key) { return SharedPreferences.GetStorageDoubleValue(CommonVariables.GameName, key); };
            _screenManager.OnSetScore = delegate (string key, double value) { SharedPreferences.SetStorageDoubleValue(CommonVariables.GameName, key, value); };

            Components.Add(_screenManager);

            _graphics.IsFullScreen = true;
			//graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
		}

		protected override void Initialize()
		{
			base.Initialize();

			_graphics.ApplyChanges();
		}

        public void CallInterstitial()
        {
            if (_adInterstitial != null)
                return;

            AdListenerCustom intlistener = new AdListenerCustom();
            intlistener.AdLoaded += () => { if (_adInterstitial.IsLoaded) _adInterstitial.Show(); };
            intlistener.AdClosed += () => { _adInterstitial = null; };

            _adInterstitial = AdWrapper.ConstructFullPageAd(_mainActivity, CommonVariables.BannerId);
            _adInterstitial.AdListener = intlistener;
            _adInterstitial.CustomBuild();
        }
    }
}
