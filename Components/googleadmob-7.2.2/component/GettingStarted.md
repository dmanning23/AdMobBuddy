Creating a banner ad unit and loading the request:

```csharp
using GoogleAdMobAds;
...

const string AdmobID = "<Get your ID at google.com/ads/admob>";

GADBannerView adView;
bool viewOnScreen = false;

public override void ViewDidLoad ()
{
	base.ViewDidLoad ();

	adView = new GADBannerView (size: GADAdSizeCons.Banner, origin: new CGPoint (0, 0)) {
		AdUnitID = AdmobID,
		RootViewController = this
	};

	adView.DidReceiveAd += (sender, args) => {
		if (!viewOnScreen) View.AddSubview (adView);
		viewOnScreen = true;
	};

	adView.LoadRequest (GADRequest.Request);
}
```
