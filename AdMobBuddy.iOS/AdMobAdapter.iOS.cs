using Foundation;
using Google.MobileAds;
using System;
using System.Diagnostics;
using UIKit;

namespace AdMobBuddy.iOS
{
	public class AdMobAdapter : NSObject, IAdManager
	{
		#region Properties

		/// <summary>
		/// Viewcontroller used to display interstitial ads.
		/// </summary>
		readonly UIViewController ViewController;

		#region IDs

		public string BannerAdID { get; set; }

		/// <summary>
		/// ID of the AdMob Interstitial ad unit.
		/// </summary>
		public string InterstitialAdID { get; set; }

		/// <summary>
		/// ID of the AdMob Rewarded Video ad unit.
		/// </summary>
		public string RewardedVideoAdID { get; set; }

		public string TestDeviceID { get; set; }

		#endregion //IDs

		#region Interstitials

		private event EventHandler OnInterstitialLoaded;

		/// <summary>
		/// The ad view interstitial.
		/// </summary>
		Interstitial AdViewInterstitial;

		#endregion //Interstitials

		#region Rewarded Video

		RewardedVideoListener RewardedVideoListener { get; set; }

		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

		/// <inheritdoc />
		public event EventHandler OnRewardedVideoDismissed;

		/// <inheritdoc />
		public event EventHandler OnRewardedVideoFailed;

		/// <inheritdoc />
		public event EventHandler OnInterstitialDismissed;

		/// <inheritdoc />
		public event EventHandler OnInterstitialFailed;

		#endregion //Rewarded Video

		private bool _childDirected;
		public bool ChildDirected
		{
			get
			{
				return _childDirected;
			}
			set
			{
				_childDirected = value;
				MobileAds.SharedInstance.RequestConfiguration.TagForChildDirectedTreatment(ChildDirected);
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initialise a new AdMob Ad Overlay. This is a Cross platform method which will run on both Android and iOS.
		/// </summary>
		/// <param name="game">The host game to et the service container from</param>
		/// <param name="location">The location to place the add on the screen</param>
		public AdMobAdapter(UIViewController controller,
			string bannerAdID = "",
			string interstitialAdID = "",
			string rewardedVideoAdID = "",
			string testDeviceID = "",
			bool childDirected = false)
		{
			BannerAdID = bannerAdID;
			InterstitialAdID = interstitialAdID;
			RewardedVideoAdID = rewardedVideoAdID;
			TestDeviceID = testDeviceID;
			_childDirected = childDirected;

			ViewController = controller;

			//Initialize AdMob
			MobileAds.SharedInstance.Start(Initialized);

			MobileAds.SharedInstance.RequestConfiguration.TagForChildDirectedTreatment(ChildDirected);

			//Preload an interstitial ad
			LoadInterstitialAd();

			//Setup rewarded video and preload an ad
			RewardedVideoListener = new RewardedVideoListener(this);
			RewardedVideoListener.OnVideoReward += RewardedVideoListener_OnVideoReward;
			RewardedVideoListener.OnRewardedVideoFailed += RewardedVideoListener_OnRewardedVideoFailed;
			RewardedVideoListener.OnRewardedVideoDismissed += RewardedVideoListener_OnRewardedVideoDismissed; ;
			RewardBasedVideoAd.SharedInstance.Delegate = RewardedVideoListener;

			LoadRewardedVideoAd();
		}

		private void RewardedVideoListener_OnRewardedVideoDismissed(object sender, EventArgs e)
		{
			OnRewardedVideoDismissed?.Invoke(this, EventArgs.Empty);
		}

		private void RewardedVideoListener_OnRewardedVideoFailed(object sender, EventArgs e)
		{
			OnRewardedVideoFailed?.Invoke(this, EventArgs.Empty);
		}

		private void RewardedVideoListener_OnVideoReward(object sender, RewardedVideoEventArgs e)
		{
			OnVideoReward?.Invoke(sender, e);
		}

		private void Initialized(InitializationStatus status)
		{
			Debug.WriteLine($"AdMob Initialized");
		}

		#region Banner Ads

		public void DisplayBannerAd()
		{
			var bannerView = new BannerView(BannerAdSize())
			{
				AdUnitId = BannerAdID,
				RootViewController = this.ViewController
			};

			bannerView.TranslatesAutoresizingMaskIntoConstraints = false;

			ViewController.View.AddSubview(bannerView);
			bannerView.BottomAnchor.ConstraintEqualTo(ViewController.View.BottomAnchor, 0).Active = true;
			bannerView.CenterXAnchor.ConstraintEqualTo(ViewController.View.CenterXAnchor, 0).Active = true;

			bannerView.LoadRequest(Request.GetDefaultRequest());
		}

		private AdSize BannerAdSize()
		{
			var frame = ViewController.View.Frame;

			var viewWidth = frame.Size.Width;

			return AdSizeCons.GetCurrentOrientationAnchoredAdaptiveBannerAdSize(frame.Size);
		}

		#endregion //Banner Ads

		#region Interstitial Ads

		private void LoadInterstitialAd()
		{
			if (!string.IsNullOrEmpty(InterstitialAdID))
			{
				AdViewInterstitial = new Interstitial(InterstitialAdID);
				AdViewInterstitial.AdReceived += (obj, e) =>
				{
					Debug.WriteLine("Interstitial ad received and ready to be displayed.");
					OnInterstitialLoaded?.Invoke(this, new EventArgs());
				};
				AdViewInterstitial.ScreenDismissed += (obj, e) =>
				{
					Debug.WriteLine("Interstitial ad closed.");
					OnInterstitialDismissed?.Invoke(this, EventArgs.Empty);
					LoadInterstitialAd();
				};

				AdViewInterstitial.ReceiveAdFailed += (obj, e) =>
				{
					OnInterstitialFailed?.Invoke(this, EventArgs.Empty);
					Debug.WriteLine($"Interstitial ad failed to load, error: {e.Error.DebugDescription}");
				};
				AdViewInterstitial.LoadRequest(Request.GetDefaultRequest());
			}
		}

		public void DisplayInterstitialAd()
		{
			if (AdViewInterstitial != null && AdViewInterstitial.IsReady && !AdViewInterstitial.HasBeenUsed)
			{
				//A new ad is already prepared
				InterstitialAdLoaded(this, new EventArgs());
			}
			else
			{
				OnInterstitialLoaded -= InterstitialAdLoaded;
				OnInterstitialLoaded += InterstitialAdLoaded;

				LoadInterstitialAd();
			}
		}

		private void InterstitialAdLoaded(object sender, EventArgs e)
		{
			OnInterstitialLoaded -= InterstitialAdLoaded;

			try
			{
				if (AdViewInterstitial != null && AdViewInterstitial.IsReady)
				{
					AdViewInterstitial.Present(ViewController);
					//Engine.Pause = true;
				}
			}
			catch
			{
				Debug.WriteLine($"There was an error showing the ad");
			}
		}

		#endregion //Interstitial Ads

		#region Rewarded Video Ads

		public void LoadRewardedVideoAd()
		{
			if (!string.IsNullOrEmpty(RewardedVideoAdID))
			{
				RewardBasedVideoAd.SharedInstance.LoadRequest(Request.GetDefaultRequest(), RewardedVideoAdID);
			}
		}

		public void DisplayRewardedVideoAd()
		{
			if (RewardBasedVideoAd.SharedInstance.IsReady)
			{
				RewardedVideoLoaded(this, new EventArgs());
			}
			else
			{
				RewardedVideoListener.OnRewardedVideoLoaded -= RewardedVideoLoaded;
				RewardedVideoListener.OnRewardedVideoLoaded += RewardedVideoLoaded;

				LoadRewardedVideoAd();
			}
		}

		protected void RewardedVideoLoaded(object obj, EventArgs e)
		{
			RewardedVideoListener.OnRewardedVideoLoaded -= RewardedVideoLoaded;

			if (RewardBasedVideoAd.SharedInstance.IsReady)
			{
				Debug.WriteLine("Reward based video ad is being displayed.");
				InvokeOnMainThread(() => RewardBasedVideoAd.SharedInstance.Present(ViewController));
			}
		}

		#endregion //Rewarded Video Ads

		#endregion //Methods
	}
}