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
        private const string LanguagePreferenceKey = "Language";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);

            languageButton = FindViewById<Button>(Resource.Id.languageButton);
            languageButton.Click += LanguageButton_Click;
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            string currentLanguage = GetLanguage();
            string newLanguage = currentLanguage == "en" ? "fr" : "en";
            SetLanguage(newLanguage);
        }

        private void SetLanguage(string language)
        {
            // Update the locale configuration
            UpdateLocale(language);

            // Refresh the activity
            Finish();
            StartActivity(Intent);
        }

        private void UpdateLocale(string language)
        {
            // Set the new language configuration
            var config = new Android.Content.Res.Configuration(BaseContext.Resources.Configuration);
            config.Locale = new Java.Util.Locale(language);
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);
        }


        private string GetLanguage()
        {
            return Intent.GetStringExtra("Language") ?? "en";
        }
    }
}
