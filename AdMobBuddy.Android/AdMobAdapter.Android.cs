using Android.App;
using Android.Gms.Ads;
using Android.Gms.Ads.Interstitial;
using Android.Gms.Ads.Rewarded;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace AdMobBuddy.Android
{
	public class AdMobAdapter : IAdManager
	{
		#region Properties

		/// <summary>
		/// The App ID from AdMob.
		/// </summary>
		private string AppID { get; set; }

		/// <summary>
		/// ID of the AdMob banner ad unit.
		/// </summary>
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

		public bool ChildDirected { get; set; } = false;

		public Activity Activity { get; set; }

		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

		/// <summary>
		/// The insterstitial ad that has been loaded.
		/// Will be null until the ad finishes loading, and also after the user views the ad.
		/// </summary>
		private InterstitialAd InterstitialAd { get; set; }

		/// <summary>
		/// Object that listens for insterstitial ads to be loaded
		/// </summary>
		private InterstitialLoadCallback interstitialLoadCallback;

		/// <summary>
		/// Object that listens for user interactions with insterstitial ads
		/// </summary>
		private AdCallback interstitialListener;

		private RewardedAd RewardedAd { get; set; }

		private RewardedVideoLoadCallback rewardedVideoLoadCallback;
		private AdCallback rewardedVideoListener;
		private RewardListener rewardListener;

		#endregion //Properties

		#region Methods

		public AdMobAdapter(Activity activity, string appId,
			string bannerAdID = "",
			string interstitialAdID = "",
			string rewardedVideoAdID = "",
			string testDeviceID = "",
			bool childDirected = false)
		{
			Activity = activity;
			ChildDirected = childDirected;

			AppID = appId;
			BannerAdID = bannerAdID;
			InterstitialAdID = interstitialAdID;
			RewardedVideoAdID = rewardedVideoAdID;
			TestDeviceID = testDeviceID;

			if (!string.IsNullOrEmpty(interstitialAdID))
			{
				interstitialLoadCallback = new InterstitialLoadCallback(this);
				interstitialListener = new AdCallback(this);
			}

			if (!string.IsNullOrEmpty(rewardedVideoAdID))
			{
				rewardedVideoLoadCallback = new RewardedVideoLoadCallback(this);
				rewardedVideoListener = new AdCallback(this);
				rewardListener = new RewardListener();
			}

			MobileAds.Initialize(Activity, new InitializationListener(this));
		}

		private AdRequest.Builder CreateBuilder()
		{
			var builder = new AdRequest.Builder();
			//builder
			if (!string.IsNullOrEmpty(TestDeviceID))
			{
				//TODO: this test device functionality is deprecated?
				//builder.AddTestDevice(TestDeviceID);
				//builder.TagForChildDirectedTreatment(ChildDirected);
			}
			return builder;
		}

		#region Banner Ads

		public void DisplayBannerAd()
		{
			//Check if there is a banner ad already in the view
			var adView = Activity.FindViewById<AdView>(Resource.Id.banner_ad_id);
			if (null == adView)
			{
				//Create the banner ad
				adView = new AdView(Activity)
				{
					AdUnitId = BannerAdID,
					Id = Resource.Id.banner_ad_id,
					AdSize = GetAdSize(),
				};

				//create a relative layout
				var layout = new RelativeLayout(Activity);
				var adViewParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
				adViewParams.AddRule(LayoutRules.AlignParentBottom, 1);

				//add the banner ad to the layout
				layout.AddView(adView, adViewParams);

				var rootView = Activity.Window.DecorView.RootView;
				var viewGroup = rootView as ViewGroup;
				var layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
				viewGroup.AddView(layout, layoutParams);
			}

			//Create the ad request
			var request = CreateBuilder().Build();

			//Load the ad into the banner
			adView.LoadAd(request);
		}

		private AdSize GetAdSize()
		{
			// Step 2 - Determine the screen width (less decorations) to use for the ad width.
			var display = Activity.WindowManager.DefaultDisplay;
			var outMetrics = new DisplayMetrics();
			display.GetMetrics(outMetrics);

			float widthPixels = outMetrics.WidthPixels;
			float density = outMetrics.Density;

			int adWidth = (int)(widthPixels / density);

			// Step 3 - Get adaptive ad size and return for setting on the ad view.
			return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSize(Activity, adWidth);
		}

		#endregion //Banner Ads

		#region Interstitial Ads

		public void LoadInterstitialAd()
		{
			if (!string.IsNullOrEmpty(InterstitialAdID))
			{
				InterstitialAd.Load(Activity, InterstitialAdID, CreateBuilder().Build(), interstitialLoadCallback);
			}
		}

		public void OnInterstitialLoaded(Java.Lang.Object p0)
		{
			InterstitialAd = p0 as InterstitialAd;
			if (null != InterstitialAd)
			{
				InterstitialAd.FullScreenContentCallback = interstitialListener;
			}
		}

		public void DisplayInterstitialAd()
		{
			if (null != InterstitialAd)
			{
				InterstitialAd.Show(Activity);
			}
			else
			{
				//Sign up for the ad loaded event to immediately show the ad once it loads
				interstitialLoadCallback.OnInterstitialLoaded -= ShowInterstitialAdWhenLoaded;
				interstitialLoadCallback.OnInterstitialLoaded += ShowInterstitialAdWhenLoaded;
				LoadInterstitialAd();
			}
		}

		private void ShowInterstitialAdWhenLoaded(object sender, EventArgs e)
		{
			interstitialLoadCallback.OnInterstitialLoaded -= ShowInterstitialAdWhenLoaded;
			DisplayInterstitialAd();
		}

		#endregion //Interstitial Ads

		#region Rewarded Video

		public void LoadRewardedVideo()
		{
			if (!string.IsNullOrEmpty(RewardedVideoAdID))
			{
				RewardedAd.Load(Activity, RewardedVideoAdID, CreateBuilder().Build(), rewardedVideoLoadCallback);
			}
		}

		public void OnRewardedVideoLoaded(Java.Lang.Object p0)
		{
			RewardedAd = p0 as RewardedAd;
			if (null != RewardedAd)
			{
				RewardedAd.FullScreenContentCallback = rewardedVideoListener;
			}
		}

		public void DisplayRewardedVideoAd()
		{
			//RewardedVideoLoadCallback-=
			if (null != RewardedAd)
			{
				rewardListener.OnVideoReward -= VideoReward;
				rewardListener.OnVideoReward += VideoReward;
				RewardedAd.Show(Activity, rewardListener);
			}
			else
			{
				//Sign up for the ad loaded event to immediately show the ad once it loads
				rewardedVideoLoadCallback.OnRewardedVideoLoaded -= ShowRewardedVideoWhenLoaded;
				rewardedVideoLoadCallback.OnRewardedVideoLoaded += ShowRewardedVideoWhenLoaded;
				LoadRewardedVideo();
			}
		}

		private void ShowRewardedVideoWhenLoaded(object sender, EventArgs e)
		{
			rewardedVideoLoadCallback.OnRewardedVideoLoaded -= ShowRewardedVideoWhenLoaded;
			DisplayRewardedVideoAd();
		}

		protected void VideoReward(object obj, RewardedVideoEventArgs e)
		{
			OnVideoReward?.Invoke(obj, e);
		}

		#endregion //Rewarded Video

		#endregion //Methods
	}
}
