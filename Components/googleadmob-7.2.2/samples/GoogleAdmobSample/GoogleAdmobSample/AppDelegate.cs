using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if __UNIFIED__
using Foundation;
using UIKit;
using CoreGraphics;

#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

using CGRect = global::System.Drawing.RectangleF;
using CGSize = global::System.Drawing.SizeF;
using CGPoint = global::System.Drawing.PointF;
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

using MonoTouch.Dialog;

using GoogleAdMobAds;

namespace GoogleAdmobSample
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UINavigationController navController;
		DialogViewController dvcDialog;

		GADBannerView adViewTableView;
		GADBannerView adViewWindow;
		GADInterstitial adInterstitial;

		bool adOnTable = false;
		bool adOnWindow = false;
		bool interstitialRequested = false;

		// Get you own AdmobId from: http://www.google.com/ads/admob/
		const string bannerId = "YOUR-ADMOB-BANNER-ID-HERE";
		const string intersitialId = "YOUR-ADMOB-INTERSTITIAL-ID-HERE";

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var root = new RootElement ("GoogleAdmobSample") {
				new Section ("Insert Ad") {
					new StringElement ("Banner on TableView", AddToTableView),
					new StringElement ("Banner on Window", AddToWindow),
					new StringElement ("Interstitial on Window", AddToView)
				},
				new Section ("Remove Ad") {
					new StringElement ("from TableView", RemoveAdFromTableView),
					new StringElement ("from Window", RemoveAdFromWindow),
				}
			};

			dvcDialog = new DialogViewController (UITableViewStyle.Grouped, root, false);
			navController = new UINavigationController (dvcDialog);

			window.RootViewController = navController;
			window.MakeKeyAndVisible ();

			return true;
		}

		void AddToTableView ()
		{
			if (adViewTableView == null) {

				// Setup your GADBannerView, review GADAdSizeCons class for more Ad sizes. 
				adViewTableView = new GADBannerView (size: GADAdSizeCons.Banner, origin: new CGPoint (-10, 0)) {
					AdUnitID = bannerId,
					RootViewController = navController
				};

				// Wire AdReceived event to know when the Ad is ready to be displayed
				adViewTableView.AdReceived += (object sender, EventArgs e) => {
					if (!adOnTable) {
						dvcDialog.Root.Add (new Section (caption: "Ad Section") {
							new UIViewElement (caption: "Ad", view: adViewTableView, transparent: true)
						});
						adOnTable = true;
					}
				};
			}
			adViewTableView.LoadRequest (GADRequest.Request);
		}

		void RemoveAdFromTableView ()
		{
			if (adViewTableView != null) {
				if (adOnTable) {
					dvcDialog.Root.RemoveAt (idx: 2, anim: UITableViewRowAnimation.Fade);
				}
				adOnTable = false;

				// You need to explicitly Dispose GADBannerView when you dont need it anymore
				// to avoid crashes if pending request are in progress
				adViewTableView.Dispose ();
				adViewTableView = null;
			}
		}

		void AddToWindow ()
		{
			if (adViewWindow == null) {

				// Setup your GADBannerView, review GADAdSizeCons class for more Ad sizes. 
				adViewWindow = new GADBannerView (size: GADAdSizeCons.Banner, 
					origin: new CGPoint (0, window.Bounds.Size.Height - GADAdSizeCons.Banner.Size.Height)) {
					AdUnitID = bannerId,
					RootViewController = navController
				};

				// Wire AdReceived event to know when the Ad is ready to be displayed
				adViewWindow.AdReceived += (object sender, EventArgs e) => {
					if (!adOnWindow) {
						navController.View.Subviews.First ().Frame = new CGRect (0, 0, 320, UIScreen.MainScreen.Bounds.Height - 50);
						navController.View.AddSubview (adViewWindow);
						adOnWindow = true;
					}
				};
			}
			adViewWindow.LoadRequest (GADRequest.Request);
		}

		void RemoveAdFromWindow ()
		{
			if (adViewWindow != null) {
				if (adOnWindow) {
					navController.View.Subviews.First ().Frame = new CGRect (0, 0, 320, UIScreen.MainScreen.Bounds.Height);
					adViewWindow.RemoveFromSuperview ();
				}
				adOnWindow = false;

				// You need to explicitly Dispose GADBannerView when you dont need it anymore
				// to avoid crashes if pending request are in progress
				adViewWindow.Dispose ();
				adViewWindow = null;
			}
		}

		void AddToView ()
		{
			if (interstitialRequested)
				return;

			if (adInterstitial == null) {
				adInterstitial = new GADInterstitial (intersitialId);
						
				adInterstitial.ScreenDismissed += (sender, e) => { 
					interstitialRequested = false;

					// You need to explicitly Dispose GADInterstitial when you dont need it anymore
					// to avoid crashes if pending request are in progress
					adInterstitial.Dispose ();
					adInterstitial = null;
				};
			}

			interstitialRequested = true;
			adInterstitial.LoadRequest (GADRequest.Request);

			ShowInterstitial ();
		}

		async void ShowInterstitial ()
		{
			do {
				await Task.Delay (100);
			} while (!adInterstitial.IsReady);

			InvokeOnMainThread (() => adInterstitial.PresentFromRootViewController (navController));
		}

	}
}

