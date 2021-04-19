using Android.App;
using Android.Gms.Ads;
using Android.Gms.Ads.Mediation;
using Android.Gms.Ads.Reward;
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
			string interstitialAdID = "",
			string rewardedVideoAdID = "",
			string testDeviceID = "",
			bool childDirected = false)
		{
			_activity = activity;
			ChildDirected = childDirected;

			AppID = appId;
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
