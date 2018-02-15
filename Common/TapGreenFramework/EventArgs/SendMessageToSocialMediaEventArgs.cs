namespace TapGreenFramework.EventArgs
{
    public class SendMessageToSocialMediaEventArgs
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public General.SocialMediaEnum SocialMedia { get; set; }
    }
}
