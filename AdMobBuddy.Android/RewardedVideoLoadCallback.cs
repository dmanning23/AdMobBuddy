using Android.Gms.Ads;
using Android.Gms.Ads.Rewarded;
using System;
using System.Diagnostics;

namespace AdMobBuddy.Android
{
	internal class RewardedVideoLoadCallback : RewardedAdLoadCallback
	{
		#region Properties

		public event EventHandler OnRewardedVideoLoaded;

		AdMobAdapter Adapter { get; set; }

		#endregion //Properties

		public RewardedVideoLoadCallback(AdMobAdapter adpater)
		{
			Adapter = adpater;
		}

		public override void OnAdLoaded(Java.Lang.Object p0)
		{
			Debug.WriteLine("Rewarded Video ad received and ready to be displayed.");

			base.OnAdLoaded(p0);

			Adapter.OnRewardedVideoLoaded(p0);
			OnRewardedVideoLoaded?.Invoke(p0, new EventArgs());
		}

		public override void OnAdFailedToLoad(LoadAdError error)
		{
			Debug.WriteLine($"Rewarded Video ad failed to load, error code: {error.Code}, {error.Message}");

			base.OnAdFailedToLoad(error);
		}
	}
}