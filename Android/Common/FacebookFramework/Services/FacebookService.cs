using FacebookFramework.DataContract;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace FacebookFramework.Services
{
	public class FacebookService
	{
		public string GetEmailAsync(string accessToken)
		{
			using (WebClient client = new WebClient())
			{
				var json = client.DownloadString($"https://graph.facebook.com/me?fields=email&access_token={ accessToken }");
				var responseJSON = JsonConvert.DeserializeObject<UserModel>(json);
				return responseJSON.Email;
			}
		}

		public string GetScorePushAsync(string accessToken)
		{
			using (WebClient client = new WebClient())
			{
				var json = client.UploadString($"https://graph.facebook.com/1699271273458561/scores", "score=10");
				var responseJSON = JsonConvert.DeserializeObject<UserModel>(json);
				return responseJSON.Email;
			}
		}

		public void GetGet()
		{
			WebRequest request = WebRequest.Create("http://some.url");
			request.Method = "POST";

			using (var stream = request.GetResponse().GetResponseStream())
			using (var reader = new StreamReader(stream))
			{
				var content = reader.ReadToEnd();
			}
		}
	}
}