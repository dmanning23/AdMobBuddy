# AdMobBuddy
A super easy to use library for adding AdMob ads to Xamarin.Android, Xamarin.iOS, and MonoGame apps.

Step one, add the nuget package located at:
https://www.nuget.org/packages/AdMobBuddy/

Step two (Android), initialize the library in your Activity class:
```
IAdManager ads = new AdMobAdapter(this, "your AdMob app ID", "your AdMob interstitial ad unit ID", "your Admob rewarded video ad unit ID"
```

Step two (iOS), initialize the library:
(in MonoGame, add this in your UIApplicationDelegate class)
```
//the first param is a UIViewController
IAdManager ads = new AdMobAdapter(game.Services.GetService<UIViewController>(), "your AdMob app ID", "your AdMob interstitial ad unit ID", "your Admob rewarded video ad unit ID"
```

Step three (interstitial), display an ad:
```
ads.DisplayInterstitialAd();
```

Step four (rewarded video), display an ad and listen for the reward:
```
ads.OnVideoReward += VideoReward;
ads.DisplayRewardedVideoAd();
```

Step five: profit
