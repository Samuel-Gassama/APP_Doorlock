using System;
using Android.Content;

namespace ProjetTerminal
{
    public class SharedPreferencesHelper
    {
        private const string SharedPreferencesName = "AppPreferences";
        private const string LanguageKey = "Language";
        private Context context;

        public SharedPreferencesHelper(Context context)
        {
            this.context = context;
        }

        public void SaveLanguage(string language)
        {
            ISharedPreferences prefs = context.GetSharedPreferences(SharedPreferencesName, FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(LanguageKey, language);
            editor.Apply();
        }

        public string GetLanguage()
        {
            ISharedPreferences prefs = context.GetSharedPreferences(SharedPreferencesName, FileCreationMode.Private);
            return prefs.GetString(LanguageKey, "en");
        }
    }

}

