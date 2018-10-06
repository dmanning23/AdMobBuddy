using System;

namespace AdMobBuddy
{
	public interface IAdManager
	{
		void ShowBannerAd();

		void HideBannerAd();

		void DisplayInterstitialAd();

		void DisplayVideoAd();

		void DisplayRewardedVideoAd();

		event EventHandler<RewardedVideoEventArgs> OnVideoReward;
	}
}