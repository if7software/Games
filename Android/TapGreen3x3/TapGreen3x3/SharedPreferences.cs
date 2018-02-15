using Android.App;
using Android.Content;
using System;

namespace TapGreen
{
	public static class SharedPreferences
	{
		public static double GetStorageDoubleValue(string appName, string key)
		{
			ISharedPreferences pref = Application.Context.GetSharedPreferences(appName, FileCreationMode.Private);

            try
            {
                return Convert.ToDouble(pref.GetFloat(key, 0f));
            }
			catch
            {
                try
                {
                    return Convert.ToDouble(pref.GetInt(key, 0));
                }
                catch
                {
                    return 0.0;
                }
            }
		}

		public static void SetStorageDoubleValue(string appName, string key, double value)
		{
			ISharedPreferences pref = Application.Context.GetSharedPreferences(appName, FileCreationMode.Private);
			ISharedPreferencesEditor edit = pref.Edit();
			edit.PutFloat(key, (float)value);
			edit.Apply();
		}
	}
}
