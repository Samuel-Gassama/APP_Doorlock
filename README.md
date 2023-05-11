# Projet Terminal

Ce projet est une application Android qui utilise MQTT pour recevoir des informations à partir d'un broker MQTT et afficher les données reçues.

## Fonctionnalités

- Connexion à un broker MQTT et réception des données
- Affichage des données reçues dans une liste
- Paramètres pour changer la langue de l'application

## Captures d'écran

Insérez ici des captures d'écran de votre application pour illustrer son apparence.

## Installation

1. Clonez ce dépôt sur votre machine locale.
2. Ouvrez le projet dans votre IDE Android.
3. Construisez et exécutez l'application sur un appareil ou un émulateur Android.

## Configuration MQTT

Pour configurer la connexion à votre broker MQTT, modifiez les paramètres suivants dans la classe `MQTTInfoActivity` :

```csharp
// Configure the MQTT client
var options = new MqttClientOptionsBuilder()
    .WithTcpServer("adresse_broker", port)
    .WithCredentials("nom_utilisateur", "mot_de_passe")
    .Build();
```

##Fix

Si la connexion à l'API n'est pas possible et que l'application affiche "Failed to add log: Cleartext HTTP traffic to 10.0.2.2 not permitted"

Rendez-vous dans le fichier "manifest.xml" à la ligne <application>, ajoutez "android:usedClearTraffic="true"" et sauvegarder le fichier. 
Puis relancez l'application !


La ligne devrait ressembler à ça : 

  <application android:usesCleartextTraffic="True" android:name="android.app.Application" android:allowBackup="true" android:appComponentFactory="androidx.core.app.CoreComponentFactory" android:debuggable="true" android:extractNativeLibs="true" android:icon="@mipmap/ic_launcher" android:label="@string/app_name" android:roundIcon="@mipmap/ic_launcher_round" android:supportsRtl="true" android:theme="@style/AppTheme">
