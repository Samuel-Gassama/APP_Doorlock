﻿using System;
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);

            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
            EditText emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            EditText passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);

            loginButton.Click += async (sender, e) =>
            {
                string email = emailEditText.Text;
                string password = passwordEditText.Text;
                await LoginUser(email, password);
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async Task LoginUser(string email, string password)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var requestContent = new StringContent(JsonConvert.SerializeObject(new { email, password }), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("http://10.0.2.2:8000/api/login", requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

                        var intent = new Intent(this, typeof(DashboardActivity));
                        intent.PutExtra("user_id", result["user_id"]);
                        intent.PutExtra("email", result["email"]);
                        StartActivity(intent);
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Invalid email or password", ToastLength.Short).Show();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }

    }
}
