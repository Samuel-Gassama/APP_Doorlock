# APP_Doorlock

Si la connexion à l'API n'est pas possible et que l'application affiche "Failed to add log: Cleartext HTTP traffic to 10.0.2.2 not permitted"

Rendez-vous dans le fichier "manifest.xml" à la ligne <application>, ajoutez "android:usedClearTraffic="true"" et sauvegarder le fichier. 
Puis relancez l'application !


La ligne devrait ressembler à ça : 

  <application android:usesCleartextTraffic="True" android:name="android.app.Application" android:allowBackup="true" android:appComponentFactory="androidx.core.app.CoreComponentFactory" android:debuggable="true" android:extractNativeLibs="true" android:icon="@mipmap/ic_launcher" android:label="@string/app_name" android:roundIcon="@mipmap/ic_launcher_round" android:supportsRtl="true" android:theme="@style/AppTheme">
