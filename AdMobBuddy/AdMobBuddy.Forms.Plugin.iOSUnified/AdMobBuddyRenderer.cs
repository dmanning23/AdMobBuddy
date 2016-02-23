using AdMobBuddy.Forms.Plugin.Abstractions;
using AdMobBuddy.Forms.Plugin.iOS;
using Google.MobileAds;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdMobBuddy.Forms.Plugin.Abstractions.AdMobBuddyControl), typeof(AdMobBuddyRenderer))]
namespace AdMobBuddy.Forms.Plugin.iOS
{
	/// <summary>
	/// AdMobBuddy Renderer for iOS
	/// </summary>
	public class AdMobBuddyRenderer : ViewRenderer
	{
		BannerView adView;
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

			// Setup your BannerView, review AdSizeCons class for more Ad sizes. 
			adView = new BannerView(AdSizeCons.Banner)
			{
				AdUnitID = adMobElement.AdUnitId,
				RootViewController = UIApplication.SharedApplication.Windows[0].RootViewController
			};

			// Wire AdReceived event to know when the Ad is ready to be displayed
			adView.AdReceived += (sender, args) =>
			{
				if (!viewOnScreen)
				{
					AddSubview(adView);
				}
				viewOnScreen = true;
			};

			adView.LoadRequest(Request.GetDefaultRequest());
			base.SetNativeControl(adView);
		}
	}
}
