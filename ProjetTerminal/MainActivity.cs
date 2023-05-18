/**
 * Page de connexion
 * @file MQTTInfoActivity.cs
 * @author 
 *     - Samuel Gassama
 * @version 1.5 : 12/05/2023
 */

using System;
using System.Net.Http;
using Android.Content;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetTerminal
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private string currentLanguage;
        private Button languageButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);

            currentLanguage = Java.Util.Locale.Default.Language;

            // Récupérer les références des éléments de l'interface utilisateur
            EditText emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            EditText passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);

            // Définir l'événement de clic pour le bouton de connexion
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
            loginButton.Click += (sender, e) =>
            {
                // Créer une intention pour démarrer l'activité MQTTInfoActivity
                var intent = new Intent(this, typeof(MQTTInfoActivity));

                // Ajouter la langue actuelle en extra pour MQTTInfoActivity
                intent.PutExtra("CurrentLanguage", currentLanguage);

                // Démarrer l'activité MQTTInfoActivity
                StartActivity(intent);
            };

            // Ajouter un bouton pour accéder à SettingsActivity
            languageButton = FindViewById<Button>(Resource.Id.languageButton);
            languageButton.Click += (sender, args) =>
            {
                // Appeler la méthode ToggleAppLanguage lorsque le bouton de langue est cliqué
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

        private void SetAppLanguage(string languageCode)
        {
            // Créer un nouvel objet Locale avec le code de langue spécifié
            var locale = new Java.Util.Locale(languageCode);

            Java.Util.Locale.Default = locale;

            var config = new Android.Content.Res.Configuration { Locale = locale };

            // Mettre à jour les ressources de l'application avec la nouvelle configuration
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);

            // Recréer l'activité pour appliquer le changement de langue
            Recreate();
        }

        // Remplacer la méthode OnRequestPermissionsResult pour gérer les demandes d'autorisation
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        // Remplacer la méthode OnResume pour vérifier si la langue par défaut a été modifiée
        protected override void OnResume()
        {
            base.OnResume();

            // Vérifier si la langue par défaut a été modifiée
            if (!Java.Util.Locale.Default.Language.Equals(currentLanguage))
            {
                // Recréer l'activité pour appliquer le changement de langue
                Recreate();
            }
        }

        // Méthode pour gérer la connexion de l'utilisateur
        private async Task LoginUser(string email, string password)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var requestContent = new StringContent(JsonConvert.SerializeObject(new { email, password }), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("http://10.0.2.2:8000/api/controller/login", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

                        // Créer une intention pour démarrer l'activité MQTTInfoActivity
                        var intent = new Intent(this, typeof(MQTTInfoActivity));

                        // Ajouter les identifiants utilisateur en extra pour MQTTInfoActivity
                        intent.PutExtra("user_id", result["user_id"]);
                        intent.PutExtra("email", result["email"]);

                        // Démarrer l'activité MQTTInfoActivity
                        StartActivity(intent);
                    }
                    else
                    {
                        // Afficher un message toast en cas d'email ou de mot de passe non valide
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Invalid email or password", ToastLength.Short).Show();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Afficher les détails de l'exception dans la console
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }

    }
}
