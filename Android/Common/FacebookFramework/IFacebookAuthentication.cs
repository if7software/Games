using System;
using FacebookFramework.Authentication;

namespace FacebookFramework
{
	public interface IFacebookAuthentication
	{
		void OnAuthenticationCompleted(FacebookOAuthToken token);
		void OnAuthenticationFailed(string message, Exception exception);
		void OnAuthenticationCanceled();
	}
}