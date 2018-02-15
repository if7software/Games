using Microsoft.Xna.Framework;
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Content.PM;
using TapGreenFramework;
using FacebookFramework;
using FacebookFramework.Services;
using FacebookFramework.Authentication;

namespace TapGreen
{
	[Activity(Label = CommonVariables.GameName,
			  MainLauncher = true,
			  Icon = "@drawable/icon",
			  Theme = "@style/Theme.Splash",
			  AlwaysRetainTaskState = true,
			  LaunchMode = LaunchMode.SingleInstance,
			  ScreenOrientation = ScreenOrientation.Portrait,
			  ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]

	public class MainActivity : AndroidGameActivity, IFacebookAuthentication
	{
		private const string ClientId = "125928271436861";
		private const string Scope = "email";

		private FacebookAuthentication _auth;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			var g = new MonoGame(this);

			SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
		
		public void FacebookAuthentication()
		{
			_auth = new FacebookAuthentication(ClientId, Scope, this);
			var auth = _auth.Authenticator;
			var intent = auth.GetUI(this);
			StartActivity(intent);
		}

		public void OnAuthenticationCompleted(FacebookOAuthToken token)
		{
			var facebookService = new FacebookService();
			var email = facebookService.GetEmailAsync(token.AccessToken);

			if (!string.IsNullOrWhiteSpace(email))
			{
				new AlertDialog.Builder(this)
					.SetTitle("Success!!!")
					.SetMessage($"It work's. Email: { email }")
					.Show();
			}
			else
			{
				new AlertDialog.Builder(this)
					.SetTitle("Authentication completed")
					.SetMessage("Email is still empty")
					.Show();
			}
		}

		public void OnAuthenticationFailed(string message, Exception exception)
		{
			new AlertDialog.Builder(this)
			   .SetTitle(message)
			   .SetMessage(exception?.ToString())
			   .Show();
		}

		public void OnAuthenticationCanceled()
		{
			new AlertDialog.Builder(this)
			   .SetTitle("Authentication canceled")
			   .SetMessage("You didn't completed the authentication process")
			   .Show();
		}

		public void ShareMessageInSocialMedia(string subject, string message, General.SocialMediaEnum socialMedia)
        {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraSubject, subject);
            intent.PutExtra(Intent.ExtraText, message);

            foreach (ResolveInfo app in PackageManager.QueryIntentActivities(intent, 0))
            {
                string activityInfoName = app.ActivityInfo.Name;
                string packageName = app.ActivityInfo.ApplicationInfo.PackageName;

                if (socialMedia == General.SocialMediaEnum.FACEBOOK
                    && activityInfoName.Contains("facebook"))
                {
					CreateSocialMediaIntent(ref intent, packageName, activityInfoName);
					break;
                }
                else if (socialMedia == General.SocialMediaEnum.TWITTER
                    && activityInfoName.Equals("com.twitter.android.PostActivity"))
                {
					CreateSocialMediaIntent(ref intent, packageName, activityInfoName);
                    break;
                }
            }
        }

		public void CreateSocialMediaIntent(ref Intent intent, string packageName, string activityInfoName)
		{
			ComponentName name = new ComponentName(packageName, activityInfoName);
			intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);
			intent.AddCategory(Intent.CategoryLauncher);
			intent.SetComponent(name);
			StartActivity(intent);
		}
	}
}

