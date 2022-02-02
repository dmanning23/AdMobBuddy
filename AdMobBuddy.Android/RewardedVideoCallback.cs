using Android.Gms.Ads.Rewarded;
using Android.Runtime;
using System;

namespace AdMobBuddy.Android
{
	internal abstract class RewardedVideoCallback : RewardedAdLoadCallback
	{
		private static Delegate cb_onAdLoaded;

		[Register("onAdLoaded", "(Lcom/google/android/gms/ads/rewarded/RewardedAd;)V", "GetOnAdLoadedHandler")]
		public virtual void OnAdLoaded(RewardedAd rewardedAd)
		{
		}

		private static Delegate GetOnAdLoadedHandler()
		{
			if (cb_onAdLoaded is null)
			{
				cb_onAdLoaded = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr>)n_onAdLoaded);
			}
			return cb_onAdLoaded;
		}

		private static void n_onAdLoaded(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
		{
			var thisobject = GetObject<RewardedVideoCallback>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var resultobject = GetObject<RewardedAd>(native_p0, JniHandleOwnership.DoNotTransfer);
			thisobject.OnAdLoaded(resultobject);
		}
	}
}