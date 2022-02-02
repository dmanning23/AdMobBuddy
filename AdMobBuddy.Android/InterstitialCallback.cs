using Android.Gms.Ads.Interstitial;
using Android.Runtime;
using System;

namespace AdMobBuddy.Android
{
	internal abstract class InterstitialCallback : InterstitialAdLoadCallback
	{
		private static Delegate cb_onAdLoaded;

		[Register("onAdLoaded", "(Lcom/google/android/gms/ads/interstitial/InterstitialAd;)V", "GetOnAdLoadedHandler")]
		public virtual void OnAdLoaded(InterstitialAd interstitialAd)
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
			var thisobject = GetObject<InterstitialCallback>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var resultobject = GetObject<InterstitialAd>(native_p0, JniHandleOwnership.DoNotTransfer);
			thisobject.OnAdLoaded(resultobject);
		}
	}
}