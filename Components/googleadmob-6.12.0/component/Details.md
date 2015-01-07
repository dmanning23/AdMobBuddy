Quickly monetize your app with Google AdMob, one of the world's largest mobile
advertising platforms. This SDK features:

* Simplified APIs
* Access to the latest HTML5 ad units from AdMob

With this component, developers can easily incorporate Google AdMob ads into their mobile
apps. Mobile-friendly text and image banners are available, along with rich, full-screen
web apps known as interstitials.  An ever-growing set of "calls-to-action" are supported
in response to user-generated events, including redirection to the App Store, iTunes,
mapping applications, videos, and the dialer. Ads can be targeted by location and
demographic data.

Here's a quick example:

```csharp
using GoogleAdMobAds;
...

const string AdmobID = "<Get your ID at google.com/ads/admob>";

GADBannerView adView;
bool viewOnScreen = false;

public override void ViewDidLoad ()
{
	base.ViewDidLoad ();

	adView = new GADBannerView (size: GADAdSizeCons.Banner, origin: new PointF (0, 0)) {
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
