# AdMobBuddy
A super easy to use library for adding AdMob ads to Xamarin.Android, Xamarin.iOS, and MonoGame apps.

## Step one:
Add the nuget package located at:
https://www.nuget.org/packages/AdMobBuddy/

## Step two (Android):
Initialize the library in your Activity class:
```
//first param is an Activity
IAdManager ads = new AdMobAdapter(this, "your AdMob app ID", "your AdMob interstitial ad unit ID", "your Admob rewarded video ad unit ID"
```

## Step two (iOS):
Initialize the library:
(in MonoGame, add this in your UIApplicationDelegate class)
```
//the first param is a UIViewController
IAdManager ads = new AdMobAdapter(game.Services.GetService<UIViewController>(), "your AdMob interstitial ad unit ID", "your Admob rewarded video ad unit ID"
```
Add your AdMob app ID to the info.plist file:
```
<key>GADApplicationIdentifier</key>
<string>your AdMob app ID</string>
```

## Step three (interstitial):
Display an ad:
```
ads.DisplayInterstitialAd();
```

## Step four (rewarded video):
Display an ad and listen for the reward:
```
ads.OnVideoReward += VideoReward;
ads.DisplayRewardedVideoAd();
```

## Step five:
profit

If you still have any questions, there is a full-blown functioning app example at: https://github.com/dmanning23/RevMobBuddySample
