using Android.Gms.Ads;

namespace TapGreen
{
    public class AdListenerCustom : AdListener
    {
        public delegate void AdLoadedEvent();
        public delegate void AdClosedEvent();
        public delegate void AdOpenedEvent();

        public event AdLoadedEvent AdLoaded;
        public event AdClosedEvent AdClosed;
        public event AdOpenedEvent AdOpened;

        public override void OnAdLoaded()
        {
            AdLoaded?.Invoke();
            base.OnAdLoaded();
        }

        public override void OnAdClosed()
        {
            AdClosed?.Invoke();
            base.OnAdClosed();
        }
        public override void OnAdOpened()
        {
            AdOpened?.Invoke();
            base.OnAdOpened();
        }
    }
}