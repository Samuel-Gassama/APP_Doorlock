using System;
using System.Collections.Generic;
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

namespace ProjetTerminal
{
    [Activity(Label = "MQTTInfoActivity")]
    public class MQTTInfoActivity : Activity
    {
        IManagedMqttClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.listeEntrees);

            // Get the text view to display the MQTT message payload
            TextView mqttInfoTextView = FindViewById<TextView>(Resource.Id.mqttInfoTextView);

            // Configure the MQTT client
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("172.16.5.100", 1883)
                .WithCredentials("mams", "mams")
                .Build();

            client = new MqttFactory().CreateManagedMqttClient();
            client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(args =>
            {
                Console.WriteLine("Connected to MQTT broker");
                client.SubscribeAsync(new TopicFilterBuilder().WithTopic("mqtt_topic").Build());
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

                // Update the text view with the MQTT message payload
                RunOnUiThread(() =>
                {
                    mqttInfoTextView.Text = $"Card ID: {cardId}\nName: {name}\nDoor: {door}";
                });
            });

            // Connect to the MQTT broker
            client.StartAsync(
                new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(options)
                    .Build());
        }
    }
}
