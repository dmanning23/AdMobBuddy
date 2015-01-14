using AdMobBuddy.Forms.Plugin.Abstractions;
using System;
using GoogleAdMobAds;
using MonoTouch.UIKit;
using Xamarin.Forms;
using AdMobBuddy.Forms.Plugin.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdMobBuddy.Forms.Plugin.Abstractions.AdMobBuddyControl), typeof(AdMobBuddyRenderer))]
namespace AdMobBuddy.Forms.Plugin.iOS
{
	/// <summary>
	/// AdMobBuddy Renderer for iOS
	/// </summary>
	public class AdMobBuddyRenderer : ViewRenderer
	{
		GADBannerView adView;
		bool viewOnScreen = false;

		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public static void Init() { }

		/// <summary>
		/// reload the view and hit up google admob 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged(e);

			//convert the element to the control we want
			var adMobElement = Element as AdMobBuddyControl;

			if (null != adMobElement) //TODO: does need this check here?
			{
				adView = new GADBannerView(size: GADAdSizeCons.Banner)
				{
					AdUnitID = adMobElement.AdUnitId,
					RootViewController = UIApplication.SharedApplication.Windows[0].RootViewController
				};

				adView.DidReceiveAd += (sender, args) =>
				{
					if (!viewOnScreen) this.AddSubview(adView);
					viewOnScreen = true;
				};

				adView.LoadRequest(GADRequest.Request);
				base.SetNativeControl(adView);
			}
		}
	}
}
