The Google Mobile Ads SDK is the latest generation in Google mobile advertising, featuring refined ad formats and streamlined APIs for access to mobile ad networks and advertising solutions. The SDK enables mobile app developers to maximize their monetization in native mobile apps.

Required Android API Levels
===========================

We recommend setting your app's *Target Framework* and *Target Android version* to **Android 5.0 (API Level 21)** or higher in your app project settings.

This Google Play Service SDK's requires a *Target Framework* of at least Android 4.1 (API Level 16) to compile.

You may still set a lower *Minimum Android version* (as low as Android 2.3 - API Level 9) so your app will run on older versions of Android, however you must ensure you do not use any API's at runtime that are not available on the version of Android your app is running on.


Android Manifest 
================

Some Google Play Services APIs require specific metadata, attributes, permissions or features to be declared in your *AndroidManifest.xml* file.

These can be added manually to the *AndroidManifest.xml* file, or merged in through the use of assembly level attributes.


The Google Mobile Ads SDK requires the *Internet* and *Access Network State* permissions to work correctly.  You can add these with the following assembly level attributes:

```csharp
[assembly: UsesPermission (Android.Manifest.Permission.Internet)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessNetworkState)]
```

You must also declare an activity that exists in the SDK by manually adding the following element to your *AndroidManifest.xml* file, inside of the `<application>` `</application>` tags:

```xml
<activity android:name="com.google.android.gms.ads.AdActivity"
            android:configChanges="keyboard|keyboardHidden|orientation|screenLayout|uiMode|screenSize|smallestScreenSize"
            android:theme="@android:style/Theme.Translucent" />
```





Samples
=======

You can find a Sample Application within each Google Play Services component.  The sample will demonstrate the necessary configuration and some basic API usages.



The AdMobSample in this component demonstrates how to use various ad types from code and xml layouts.




Learn More
==========

You can learn more about the various Google Play Services SDKs & APIs by visiting the official [Google APIs for Android][3] documentation


You can learn more about the Google Play Services Ad by visiting the official [AdMob for Android](https://developers.google.com/admob/android/quick-start) documentation.



[1]: https://console.developers.google.com/ "Google Developers Console"
[2]: https://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/MD5_SHA1/ "Finding your SHA-1 Fingerprints"
[3]: https://developers.google.com/android/ "Google APIs for Android"

