using System;

namespace AdMobBuddy.WindowsDX
{
	/// <summary>
	/// This is a stubbed out AdManager class for testing your app on WindowsDX
	/// </summary>
	public class TestAdManager : IAdManager
	{
		public bool ChildDirected { get; set; }

		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

        /// <inheritdoc />
        public event EventHandler OnRewardedVideoDismissed;

        /// <inheritdoc />
        public event EventHandler OnRewardedVideoFailed;

        /// <inheritdoc />
        public event EventHandler OnInterstitialDismissed;

        /// <inheritdoc />
        public event EventHandler OnInterstitialFailed;

        public virtual void DisplayBannerAd()
		{
		}

		public virtual void DisplayInterstitialAd()
		{
		}

		public virtual void DisplayRewardedVideoAd()
		{
			VideoReward(this, new RewardedVideoEventArgs(true));
		}	

		protected void VideoReward(object obj, RewardedVideoEventArgs e)
		{
			if (null != OnVideoReward)
			{
				OnVideoReward(obj, e);
			}
		}
	}
}
