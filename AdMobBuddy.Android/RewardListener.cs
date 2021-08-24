using Android.Gms.Ads;
using Android.Gms.Ads.Rewarded;
using System;

namespace AdMobBuddy.Android
{
	public class RewardListener : Java.Lang.Object, IOnUserEarnedRewardListener
	{
		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

		public void OnUserEarnedReward(IRewardItem p0)
		{
			OnVideoReward?.Invoke(this, new RewardedVideoEventArgs(true));
		}
	}
}