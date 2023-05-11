using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ProjetTerminal
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        private Button languageButton;
        private string currentLanguage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings);

            currentLanguage = Java.Util.Locale.Default.Language;

            languageButton = FindViewById<Button>(Resource.Id.languageButton);
            languageButton.Click += (sender, args) =>
            {
                ToggleAppLanguage();
            };
        }

        private void ToggleAppLanguage()
        {
            string newLanguage = currentLanguage == "en" ? "fr" : "en";
            SetAppLanguage(newLanguage);
            currentLanguage = newLanguage;
        }

        private void SetAppLanguage(string languageCode)
        {
            var locale = new Java.Util.Locale(languageCode);
            Java.Util.Locale.Default = locale;

            var config = new Android.Content.Res.Configuration { Locale = locale };
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);
            Recreate();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!Java.Util.Locale.Default.Language.Equals(currentLanguage))
            {
                Recreate();
            }
        }
    }
}
