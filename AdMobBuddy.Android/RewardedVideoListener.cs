using Android.Gms.Ads;
using Android.Gms.Ads.Reward;
using System;

namespace AdMobBuddy.Android
{
	internal class RewardedVideoListener : AdListener, IRewardedVideoAdListener
	{
		#region Properties

		public event EventHandler OnVideoLoaded;
		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

		AdMobAdapter Adapter { get; set; }

		#endregion //Properties

		public RewardedVideoListener(AdMobAdapter adpater)
		{
			Adapter = adpater;
		}

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
	}
}