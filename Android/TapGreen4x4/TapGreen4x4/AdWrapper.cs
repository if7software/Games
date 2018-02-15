using Android.Content;
using Android.Gms.Ads;

namespace TapGreen
{
    internal static class AdWrapper
    {
        public static InterstitialAd ConstructFullPageAd(Context context, string UnitId)
        {
            var ad = new InterstitialAd(context);
            ad.AdUnitId = UnitId;
            return ad;
        }

        public static AdView ConstructStandardBanner(Context context, AdSize adSize, string UnitId)
        {
            var ad = new AdView(context);
            ad.AdSize = adSize;
            ad.AdUnitId = UnitId;
            return ad;
        }

        public static InterstitialAd CustomBuild(this InterstitialAd ad)
        {
            var requestbuilder = new AdRequest.Builder()
                .AddTestDevice(AdRequest.DeviceIdEmulator)
                .Build();
            ad.LoadAd(requestbuilder);
            return ad;
        }

        public static AdView CustomBuild(this AdView ad)
        {
            var requestbuilder = new AdRequest.Builder()
                .AddTestDevice(AdRequest.DeviceIdEmulator)
                .Build();
            ad.LoadAd(requestbuilder);
            return ad;
        }

        public static InterstitialAd CustomBuild(this InterstitialAd ad, string testDeviceID)
        {
            var requestbuilder = new AdRequest.Builder()
                .AddTestDevice(AdRequest.DeviceIdEmulator)
                .AddTestDevice(testDeviceID)
                .Build();
            ad.LoadAd(requestbuilder);
            return ad;
        }

        public static AdView CustomBuild(this AdView ad, string testDeviceID)
        {
            var requestbuilder = new AdRequest.Builder()
                .AddTestDevice(AdRequest.DeviceIdEmulator)
                .AddTestDevice(testDeviceID)
                .Build();
            ad.LoadAd(requestbuilder);
            return ad;
        }
    }
}