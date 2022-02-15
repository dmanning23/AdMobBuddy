# AdMobBuddy
A super easy to use library for adding AdMob ads to Xamarin.Android, Xamarin.iOS, and MonoGame apps.

## Step one:
Add the nuget package located at:
https://www.nuget.org/packages/AdMobBuddy/

## Step two (Android):
Initialize the library in your Activity class:
```
//first param is an Activity
IAdManager ads = new AdMobAdapter(this, 
"your AdMob app ID", 
"your AdMob Banner Ad ID",
"your AdMob interstitial ad unit ID", 
"your Admob rewarded video ad unit ID"
```

Add your AdMob app ID to the .manifest file:
```
<application>
		<meta-data android:name="com.google.android.gms.ads.APPLICATION_ID" android:value="your AdMob app ID" />
</application>
```

## Step two (iOS):
Initialize the library:
(in MonoGame, add this in your UIApplicationDelegate class)
```
//the first param is a UIViewController
IAdManager ads = new AdMobAdapter(game.Services.GetService<UIViewController>(), 
"your AdMob Banner Ad ID",
"your AdMob interstitial ad unit ID", 
"your Admob rewarded video ad unit ID"
```
Add your AdMob app ID to the info.plist file:
```
<key>GADApplicationIdentifier</key>
<string>your AdMob app ID</string>
```

## Step three (banner):
Display a banner ad:
```
ads.DisplayBannerAd();
```

## Step four (interstitial):
Display an ad:
```
ads.DisplayInterstitialAd();
```

## Step five (rewarded video):
Display an ad and listen for the reward:
```
ads.OnVideoReward += VideoReward;
ads.DisplayRewardedVideoAd();
```

## Step six:
profit

If you still have any questions, there is a full-blown functioning app example at: https://github.com/dmanning23/AdMobBuddy/Sample
