using System;

namespace AdMobBuddy
{
	public interface IAdManager
	{
		void DisplayInterstitialAd();

		void DisplayRewardedVideoAd();

		event EventHandler<RewardedVideoEventArgs> OnVideoReward;
	}
}