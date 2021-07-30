using Android.App;
using Android.Gms.Ads;
using Android.Gms.Ads.Mediation;
using Android.Gms.Ads.Reward;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace AdMobBuddy.Android
{
	public class AdMobAdapter : IAdManager
	{
		#region Properties

		protected InterstitialAd InterstitialAdHandler { get; set; }

		protected IRewardedVideoAd RewardedVideoAdHandler { get; set; }

		InterstitialListener interstitialListener;
		RewardedVideoListener videoRewardedListener;

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

		public bool ChildDirected { get; set; }

		Activity _activity;

		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

		#endregion //Properties

		#region Methods

		public AdMobAdapter(Activity activity, string appId,
			string bannerAdID = "",
			string interstitialAdID = "",
			string rewardedVideoAdID = "",
			string testDeviceID = "",
			bool childDirected = false)
		{
			_activity = activity;
			ChildDirected = childDirected;

			AppID = appId;
			BannerAdID = bannerAdID;
			InterstitialAdID = interstitialAdID;
			RewardedVideoAdID = rewardedVideoAdID;
			TestDeviceID = testDeviceID;

			MobileAds.Initialize(_activity, AppID);

			if (!string.IsNullOrEmpty(interstitialAdID))
			{
				InterstitialAdHandler = new InterstitialAd(_activity);
				InterstitialAdHandler.AdUnitId = InterstitialAdID;

				interstitialListener = new InterstitialListener(this);
				InterstitialAdHandler.AdListener = interstitialListener;
			}

			if (!string.IsNullOrEmpty(rewardedVideoAdID))
			{
				RewardedVideoAdHandler = MobileAds.GetRewardedVideoAdInstance(_activity);
				RewardedVideoAdHandler.UserId = AppID;

				videoRewardedListener = new RewardedVideoListener(this);
				RewardedVideoAdHandler.RewardedVideoAdListener = videoRewardedListener;
				videoRewardedListener.OnVideoReward += VideoReward;
			}

			LoadInterstitialAd();
			LoadRewardedVideoAd();
		}

		private AdRequest.Builder CreateBuilder()
		{
			var builder = new AdRequest.Builder();
			if (!string.IsNullOrEmpty(TestDeviceID))
			{
				builder.AddTestDevice(TestDeviceID);
			}
			return builder;
		}

		#region Banner Ads

		public void DisplayBannerAd()
		{
			//Check if there is a banner ad already in the view
			var adView = _activity.FindViewById<AdView>(Resource.Id.banner_ad_id);
			if (null == adView)
			{
				//Create the banner ad
				adView = new AdView(_activity)
				{
					AdUnitId = BannerAdID,
					Id = Resource.Id.banner_ad_id,
					AdSize = GetAdSize(),
				};

				//create a relative layout
				var layout = new RelativeLayout(_activity);
				var adViewParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
				adViewParams.AddRule(LayoutRules.AlignParentBottom, 1);

				//add the banner ad to the layout
				layout.AddView(adView, adViewParams);

				var rootView = _activity.Window.DecorView.RootView;
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
			var display = _activity.WindowManager.DefaultDisplay;
			var outMetrics = new DisplayMetrics();
			display.GetMetrics(outMetrics);

			float widthPixels = outMetrics.WidthPixels;
			float density = outMetrics.Density;

			int adWidth = (int)(widthPixels / density);

			// Step 3 - Get adaptive ad size and return for setting on the ad view.
			return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSize(_activity, adWidth);
		}

		#endregion //Banner Ads

		#region Interstitial Ads

		public void LoadInterstitialAd()
		{
			InterstitialAdHandler?.LoadAd(CreateBuilder().Build());
		}

		public void DisplayInterstitialAd()
		{
			if (null != InterstitialAdHandler && null != interstitialListener)
			{
				if (InterstitialAdHandler.IsLoaded)
				{
					InterstitialAdLoaded(this, new EventArgs());
				}
				else
				{
					interstitialListener.OnInterstitialLoaded -= InterstitialAdLoaded;
					interstitialListener.OnInterstitialLoaded += InterstitialAdLoaded;

					LoadInterstitialAd();
				}
			}
		}

		private void InterstitialAdLoaded(object sender, EventArgs e)
		{
			interstitialListener.OnInterstitialLoaded -= InterstitialAdLoaded;
			InterstitialAdHandler.Show();
		}

		#endregion //Interstitial Ads

		#region Rewarded Video

		public void LoadRewardedVideoAd()
		{
			var builder = CreateBuilder();
			builder.TagForChildDirectedTreatment(ChildDirected);
			var adRequest = builder.Build();
			RewardedVideoAdHandler?.LoadAd(RewardedVideoAdID, adRequest);
		}

		public void DisplayRewardedVideoAd()
		{
			if (null != RewardedVideoAdHandler && null != videoRewardedListener)
			{
				if (RewardedVideoAdHandler.IsLoaded)
				{
					RewardedVideoLoaded(this, new EventArgs());
				}
				else
				{
					videoRewardedListener.OnVideoLoaded -= RewardedVideoLoaded;
					videoRewardedListener.OnVideoLoaded += RewardedVideoLoaded;

					LoadRewardedVideoAd();
				}
			}
		}

		protected void RewardedVideoLoaded(object obj, EventArgs e)
		{
			videoRewardedListener.OnVideoLoaded -= RewardedVideoLoaded;
			RewardedVideoAdHandler?.Show();
		}

		protected void VideoReward(object obj, RewardedVideoEventArgs e)
		{
			if (null != OnVideoReward)
			{
				OnVideoReward(obj, e);
			}
		}

		#endregion //Rewarded Video

		#endregion //Methods
	}
}
