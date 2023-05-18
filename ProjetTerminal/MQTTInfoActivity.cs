/**
 * Receiving the MQTT Data info 
 * @file MQTTInfoActivity.cs
 * @author 
 *     - Samuel Gassama
 * @version 1.6 : 12/05/2023
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
using ProjetTerminal;
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

            // Configuration du client MQTT
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
                // Réception d'un message MQTT

                string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                JObject jObject = JObject.Parse(payload);
                string cardId = jObject["cardId"].ToString();
                string name = jObject["name"].ToString();
                string door = jObject["door"].ToString();

                // Ajouter la clé scannée à la liste
                var scannedKey = new JavaDictionary<string, object> {
                {"ID", cardId},
                {"Statut", name},
                {"Porte", door},
                {"Date et Heure", DateTime.Now.ToString()}
            };
                scannedKeys.Add(scannedKey);

                // Mettre à jour la liste des clés scannées dans la vue
                RunOnUiThread(() =>
                {
                    var adapter = new SimpleAdapter(this, scannedKeys, Resource.Layout.mqtt_list_item, new[] { "ID", "Statut", "Porte", "Date et Heure" }, new[] {
        Resource.Id.textView1, Resource.Id.textView2, Resource.Id.textView3, Resource.Id.textView4
    });
                    ListView mqttInfoListView = FindViewById<ListView>(Resource.Id.mqttInfoListView);
                    mqttInfoListView.Adapter = adapter;
                });

            });



            Button settingsButton = FindViewById<Button>(Resource.Id.settingsButton);
            settingsButton.Click += (sender, args) =>
            {
                var intent = new Intent(this, typeof(SettingsActivity));
                intent.PutExtra("CurrentLanguage", currentLanguage);
                StartActivity(intent);
            };

            // Se connecter au  MQTT
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
