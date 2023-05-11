using System;
using System.Reflection.Emit;
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

        // Méthode pour basculer la langue de l'application
        private void ToggleAppLanguage()
        {
            // Déterminer la nouvelle langue en fonction de la langue actuelle
            string newLanguage = currentLanguage == "en" ? "fr" : "en";

            SetAppLanguage(newLanguage);
            currentLanguage = newLanguage;
        }

        // Méthode pour définir la langue de l'application
        private void SetAppLanguage(string languageCode)
        {
            var locale = new Java.Util.Locale(languageCode);

            // Définir la langue par défaut de l'application sur la nouvelle locale
            Java.Util.Locale.Default = locale;

            // Créer une nouvelle configuration avec la nouvelle locale
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
