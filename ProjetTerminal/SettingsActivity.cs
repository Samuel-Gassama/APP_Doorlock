
/**
 * Settings page 
 * @file SettingsActivity.cs
 * @author 
 *     - Samuel Gassama
 * @version 1.6 : 12/05/2023
 */

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ProjetTerminal;

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

            Button backButton = FindViewById<Button>(Resource.Id.backButton);
            backButton.Click += (sender, args) =>
            {

                var intent = new Intent(this, typeof(MQTTInfoActivity));
                intent.PutExtra("CurrentLanguage", currentLanguage);
                Finish(); // Finish the current activity to go back to the previous activity
                StartActivity(intent);

            };
        }



        // Méthode pour basculer la langue de l'application

        private void ToggleAppLanguage()
        {
            string newLanguage = currentLanguage == "en" ? "fr" : "en";
            SetAppLanguage(newLanguage);
            currentLanguage = newLanguage;
        }

        // Méthode pour définir la langue de l'application

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

            // Vérifier si la langue par défaut a été modifiée

            if (!Java.Util.Locale.Default.Language.Equals(currentLanguage))
            {
                Recreate();
            }
        }
    }
}
