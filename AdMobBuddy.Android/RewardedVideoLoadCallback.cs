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

		public new void OnAdLoaded(Java.Lang.Object p0)
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

		/*

		public void OnRewardedVideoAdLoaded()
		{
			Console.WriteLine("Rewarded Video loaded.");
			if (null != OnVideoLoaded)
			{
				OnVideoLoaded(this, new EventArgs());
			}
		}

		public void OnRewardedVideoAdOpened()
		{
			Console.WriteLine("Rewarded Video opened.");
		}

		public void OnRewardedVideoStarted()
		{
			Console.WriteLine("Rewarded Video started.");
		}

		public void OnRewarded(IRewardItem reward)
		{
			Console.WriteLine("Rewarded Video completed.");
			if (null != OnVideoReward)
			{
				OnVideoReward(this, new RewardedVideoEventArgs(true));
			}
		}

		public void OnRewardedVideoAdClosed()
		{
			Console.WriteLine("Rewarded Video closed.");
			if (null != OnVideoReward)
			{
				OnVideoReward(this, new RewardedVideoEventArgs(false));
			}
			Adapter.LoadRewardedVideoAd();
		}

		public void OnRewardedVideoAdLeftApplication()
		{
			Console.WriteLine("Rewarded Video clicked!");
		}

		public void OnRewardedVideoAdFailedToLoad(int errorCode)
		{
			Console.WriteLine($"Rewarded Video failed to load, error code: {errorCode}");
			if (null != OnVideoReward)
			{
				OnVideoReward(this, new RewardedVideoEventArgs(false));
			}
		}

		public void OnRewardedVideoCompleted()
		{
			Console.WriteLine("Rewarded Video completed.");
		}

		*/
	}
}