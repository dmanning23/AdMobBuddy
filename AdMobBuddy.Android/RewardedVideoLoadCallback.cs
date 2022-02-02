using Android.Gms.Ads;
using Android.Gms.Ads.Rewarded;
using System;
using System.Diagnostics;

namespace AdMobBuddy.Android
{
	internal class RewardedVideoLoadCallback : RewardedVideoCallback
	{
		#region Properties

		public event EventHandler OnRewardedVideoLoaded;

		AdMobAdapter Adapter { get; set; }

		#endregion //Properties

		public RewardedVideoLoadCallback(AdMobAdapter adpater)
		{
			Adapter = adpater;
		}

		public override void OnAdLoaded(RewardedAd rewardedAd)
		{
			Debug.WriteLine("Rewarded Video ad received and ready to be displayed.");

			base.OnAdLoaded(rewardedAd);

			Adapter.OnRewardedVideoLoaded(rewardedAd);
			OnRewardedVideoLoaded?.Invoke(rewardedAd, new EventArgs());
		}

		public override void OnAdFailedToLoad(LoadAdError error)
		{
			Debug.WriteLine($"Rewarded Video ad failed to load, error code: {error.Code}, {error.Message}");

			base.OnAdFailedToLoad(error);
		}
	}
}