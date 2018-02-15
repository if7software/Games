using Android.App;
using Android.Content;

namespace TapGreen
{
    public static class SharedPreferences
    {
        public static int GetStorageIntValue(string appName, string key)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences(appName, FileCreationMode.Private);
            return pref.GetInt(key, 0);
        }

        public static void SetStorageIntValue(string appName, string key, int value)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences(appName, FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            edit.PutInt(key, value);
            edit.Apply();
        }
    }
}