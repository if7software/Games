using FacebookFramework.Authentication;
using System;
using Xamarin.Auth;

namespace FacebookFramework
{
	public class FacebookAuthentication
	{
		private const string AuthorizeUrl = "https://www.facebook.com/dialog/oauth/";
		private const string RedirectUrl = "https://www.facebook.com/connect/login_success.html";

		private OAuth2Authenticator _auth;
		private IFacebookAuthentication _authInterface;

		public OAuth2Authenticator Authenticator
		{
			get { return _auth; }
		}

		public FacebookAuthentication(string clientId, string scope, IFacebookAuthentication authInterface)
		{
			_authInterface = authInterface;

			_auth = new OAuth2Authenticator(clientId, scope, new Uri(AuthorizeUrl), new Uri(RedirectUrl), null, false);

			_auth.Completed += OnAuthenticationCompleted;
			_auth.Error += OnAuthenticationFailed;
		}

		private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
		{
			_authInterface.OnAuthenticationFailed(e.Message, e.Exception);
		}

		private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				var token = new FacebookOAuthToken
				{
					AccessToken = e.Account.Properties["access_token"]
				};
				_authInterface.OnAuthenticationCompleted(token);
			}
			else
			{
				_authInterface.OnAuthenticationCanceled();
			}
		}
	}
}
