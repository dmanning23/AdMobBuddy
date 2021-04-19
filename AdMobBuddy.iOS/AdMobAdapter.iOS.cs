using Foundation;
using Google.MobileAds;
using System;
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

		private event EventHandler OnRewardedVideoLoaded;

		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

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
			string interstitialAdID = "",
			string rewardedVideoAdID = "",
			string testDeviceID = "",
			bool childDirected = false)
		{
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

			//initialize rewarded video and preload a rewared video ad
			RewardBasedVideoAd.SharedInstance.FailedToLoad += DidFailToLoad;
			RewardBasedVideoAd.SharedInstance.AdReceived += DidReceiveAd;
			RewardBasedVideoAd.SharedInstance.Opened += DidOpen;
			RewardBasedVideoAd.SharedInstance.PlayingStarted += DidStartPlaying;
			RewardBasedVideoAd.SharedInstance.Closed += DidClose;
			RewardBasedVideoAd.SharedInstance.WillLeaveApplication += WillLeaveApplication;
			RewardBasedVideoAd.SharedInstance.UserRewarded += DidRewardUser;

			LoadRewardedVideoAd();
		}

		private void Initialized(InitializationStatus status)
		{
			Console.WriteLine($"AdMob Initialized");
		}

		#region Interstitial Ads

		private void LoadInterstitialAd()
		{
			if (!string.IsNullOrEmpty(InterstitialAdID))
			{
				AdViewInterstitial = new Interstitial(InterstitialAdID);
				AdViewInterstitial.AdReceived += (obj, e) =>
				{
					Console.WriteLine("Interstitial ad received and ready to be displayed.");
					OnInterstitialLoaded?.Invoke(this, new EventArgs());
				};
				AdViewInterstitial.ScreenDismissed += (obj, e) =>
				{
					Console.WriteLine("Interstitial ad closed.");
					LoadInterstitialAd();
				};

				AdViewInterstitial.ReceiveAdFailed += (obj, e) =>
				{
					Console.WriteLine($"Interstitial ad failed to load, error: {e.Error.DebugDescription}");
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
				Console.WriteLine($"There was an error showing the ad");
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
				OnRewardedVideoLoaded -= RewardedVideoLoaded;
				OnRewardedVideoLoaded += RewardedVideoLoaded;

				LoadRewardedVideoAd();
			}
		}

		protected void RewardedVideoLoaded(object obj, EventArgs e)
		{
			OnRewardedVideoLoaded -= RewardedVideoLoaded;

			if (RewardBasedVideoAd.SharedInstance.IsReady)
			{
				InvokeOnMainThread(() => RewardBasedVideoAd.SharedInstance.Present(ViewController));
				Console.WriteLine("Reward based video ad is being displayed.");
			}
		}

		public void DidRewardUser(object sender, RewardBasedVideoAdRewardEventArgs e)
		{
			OnVideoReward?.Invoke(this, new RewardedVideoEventArgs(true));
		}

		public void DidFailToLoad(object sender, RewardBasedVideoAdErrorEventArgs e)
		{
			Console.WriteLine($"Reward based video ad failed to load with error: {e.Error.LocalizedDescription}.");
		}

		public void DidReceiveAd(object sender, EventArgs e)
		{
			Console.WriteLine("Reward based video ad is received.");
			OnRewardedVideoLoaded?.Invoke(this, new EventArgs());
		}

		public void DidOpen(object sender, EventArgs e)
		{
			Console.WriteLine("Opened reward based video ad.");
		}

		public void DidStartPlaying(object sender, EventArgs e)
		{
			Console.WriteLine("Reward based video ad started playing.");
		}

		public void DidClose(object sender, EventArgs e)
		{
			Console.WriteLine("Reward based video ad is closed.");
			LoadRewardedVideoAd();
		}

		public void WillLeaveApplication(object sender, EventArgs e)
		{
			Console.WriteLine("Rewarded Video clicked! Reward based video ad will leave application.");
		}

		#endregion //Rewarded Video Ads

		#endregion //Methods
	}
}