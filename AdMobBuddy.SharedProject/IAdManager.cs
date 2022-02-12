using System;

namespace AdMobBuddy
{
	public interface IAdManager
	{
		/// <summary>
		/// This flag is for whether the ads served should be child directed
		/// </summary>
		bool ChildDirected { get; set; }

		void DisplayBannerAd();

		void DisplayInterstitialAd();

		void DisplayRewardedVideoAd();

		event EventHandler<RewardedVideoEventArgs> OnVideoReward;

        event EventHandler OnRewardedVideoDismissed;

        event EventHandler OnRewardedVideoFailed;

        event EventHandler OnInterstitialDismissed;

        event EventHandler OnInterstitialFailed;
    }
}