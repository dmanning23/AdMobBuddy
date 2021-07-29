using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.Gms.Ads.Rewarded;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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