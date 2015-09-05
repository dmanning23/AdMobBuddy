## Google Play Services

This component contains libraries for all of the Google Play Services API's.  While you are free to install them all through this component, the recommended approach is to install only the NuGet packages for the API's you are using in your app.  Here is a list of all the individual libraries which make up Google Play Services:

 * Ads
 * Analytics
 * App Indexing
 * App Invites
 * App State
 * Base (all libraries depend and require this)
 * Cast
 * Drive
 * Fitness
 * Games
 * Gcm
 * Identity
 * Location
 * Maps
 * Nearby
 * Panorama
 * Plus
 * Safety Net
 * Wallet
 * Wearable 


## SDK Version

It is a good practice to set your Xamarin.Android app's ***Target Framework*** to *Use latest installed platform (X.Y)* and your ***Target Android Version*** to *Automatic - use target framework version*.  

Even though you may compile your app targeting the latest API level, you can still set your ***Minimum Android version*** all the way down to *2.3 (API Level 10)* to run on older devices.  This means you will be responsible for adding appropriate runtime checks to ensure your app is not using API's that are not available on a given device which may be running an older version of Android.


## Testing

To test your app using Google Play Services, you must use either:

 * A compatible Android device that includes Google Play Store
 * The **Android emulator** with an AVD that runs the **Google APIs** platform

Ideally, you should develop and test your app on a variety of devices, including both phones and tablets.


## Google Play Developer Console Setup

Some of the API's you may want to use require some setup within the [Google Play Developer Console](https://code.google.com/apis/console/).

For your app, you should create a new *Project*.  Under the *APIs & auth -> APIs* section you can enable various API's to use (such as Places, Maps, GCM, etc).

### Create an Android API Key

For some API's, such as Maps, you will need to create a *Public API access* key for your Android app.

In the *Google Play Developer Console* go to the *APIs & auth -> Credentials* section and click *Create new Key* in the *Public API access* section.  Choose *Android key*.

Next, you need to find the SHA1 hash for both your *debug.keystore* and the *.keystore* you will be signing your app's release builds with.  You can visit our documentation on [Finding your Keystore's MD5 or SHA1 Signature](http://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/MD5_SHA1/) to learn how to do this.

Once you have your SHA1 hashes, you can enter them in the Google Play Developer Console, one per line, followed by `;com.your.app.package.name`.  Your entries should look something like this:

```
45:B5:E4:6F:36:AD:0A:98:94:B4:02:66:2B:12:17:F2:56:26:A0:E0;com.your.app.package.name
56:C6:F5:7A:47:BE:1B:09:05:C5:13:77:3C:23:28:A3:67:37:B1:F1;com.your.app.package.name
```

After you click *create* You will have a new Key.  The *API key* value is what you will use in your Android app, according to the directions of the API you are working with.

### Create a Client ID

For some Google Play Services API's (such as App Invites), you will need to create a *Client ID* to make them work.  

In the *Google Play Developer Console* go to *APIs & auth -> Credentials* section and click *Create new Client ID* in the *OAuth* section.  

Choose *Installed application* and then pick *Android*.  Enter your package name, and the SHA1 of your app (you will need to create a client ID for both your debug.keystore and release .keystore.

Click *Create Client ID* to complete the step.


## Samples

There are a number of samples included in the component which will help you better understand how to use API's for the individual Google Play Services libraries.  


## Documentation

For more information, guides, and java samples you should visit the [Google Play Services Documentation](https://developers.google.com/android/)
