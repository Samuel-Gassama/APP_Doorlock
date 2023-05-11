/**
 * Receiving the MQTT Data info 
 * @file MQTTInfoActivity.cs
 * @author 
 *     - Samuel Gassama
 * @version 1.5 : 28/04/2023
 */


using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Exceptions;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace ProjetTerminal
{
    [Activity(Label = "MQTTInfoActivity", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]

    public class MQTTInfoActivity : Activity
    {
        IManagedMqttClient client;
        private List<IDictionary<string, object>> scannedKeys = new List<IDictionary<string, object>>();
        private string currentLanguage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listeEntrees);

            currentLanguage = Java.Util.Locale.Default.Language;

            // Configure the MQTT client
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("172.16.5.100", 1883)
                .WithCredentials("mams", "mams")
                .Build();

            client = new MqttFactory().CreateManagedMqttClient();
            client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(args =>
            {
                Console.WriteLine("Connected to MQTT broker");
                client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("door/lock").Build());
            });
            client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(args =>
            {
                Console.WriteLine("Disconnected from MQTT broker: " + args.Exception.Message);
            });
            client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(args =>
            {
                string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                JObject jObject = JObject.Parse(payload);
                string cardId = jObject["cardId"].ToString();
                string name = jObject["name"].ToString();
                string door = jObject["door"].ToString();

                // Add the scanned key to the list
                var scannedKey = new JavaDictionary<string, object> {
                {"ID", cardId},
                {"Statut", name},
                {"Porte", door},
                {"Date et Heure", DateTime.Now.ToString()}
            };
                scannedKeys.Add(scannedKey);

                // Update the list view with all the scanned keys
                RunOnUiThread(() =>
                {
                    var adapter = new SimpleAdapter(this, scannedKeys, Resource.Layout.listeEntrees, new[] { "ID", "Statut", "Porte", "Date et Heure" }, new[] {
            Resource.Id.textView1, Resource.Id.textView2, Resource.Id.textView3, Resource.Id.textView4
        });
                    ListView mqttInfoListView = FindViewById<ListView>(Resource.Id.mqttInfoListView);
                    mqttInfoListView.Adapter = adapter;
                });
            });



            // Add a button to go to the SettingsActivity
            Button settingsButton = FindViewById<Button>(Resource.Id.settingsButton);
            settingsButton.Click += (sender, args) =>
            {
                var intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
            };

            // Connect to the MQTT broker
            client.StartAsync(
                new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(options)
                    .Build());

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