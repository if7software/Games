using Microsoft.Xna.Framework;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Gms.Ads;

namespace TapGreen
{
    [Activity(Label = CommonVariables.GameName,
        MainLauncher = true,
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class MainActivity : AndroidGameActivity
    {
        private static InterstitialAd _adInterstitial;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new MonoGame(this);

            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }

        public void CallInterstitial()
        {
            if (_adInterstitial != null)
                return;

            AdListenerCustom intlistener = new AdListenerCustom();
            intlistener.AdLoaded += () => { if (_adInterstitial.IsLoaded) _adInterstitial.Show(); };
            intlistener.AdClosed += () => { _adInterstitial = null; };

            _adInterstitial = AdWrapper.ConstructFullPageAd(this, CommonVariables.BannerId);
            _adInterstitial.AdListener = intlistener;
            _adInterstitial.CustomBuild();
        }
    }
}

