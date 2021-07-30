using Android.Gms.Ads;
using System;
using System.Diagnostics;
using Android.Gms.Ads.Interstitial;

namespace AdMobBuddy.Android
{
	internal class InterstitialLoadCallback : InterstitialAdLoadCallback
	{
		#region Properties

		public event EventHandler OnInterstitialLoaded;

		AdMobAdapter Adapter { get; set; }

		#endregion //Properties

		#region Methods

		public InterstitialListener(AdMobAdapter adpater)
		{
			Adapter = adpater;
		}

		public override void OnAdLoaded()
		{
			Debug.WriteLine("Interstitial ad received and ready to be displayed.");

			base.OnAdLoaded();

			if (null != OnInterstitialLoaded)
			{
				OnInterstitialLoaded(this, new EventArgs());
			}
		}

		public override void OnAdClosed()
		{
			Debug.WriteLine("Interstitial ad closed.");

			base.OnAdClosed();

			Adapter.LoadInterstitialAd();
		}

		public override void OnAdOpened()
		{
			Debug.WriteLine("Interstitial ad opened.");

			base.OnAdOpened();
		}

		public override void OnAdFailedToLoad(LoadAdError error)
		{
			Debug.WriteLine($"Interstitial ad failed to load, error code: {error.Code}, {error.Message}");

			base.OnAdFailedToLoad(error);
		}

		#endregion //Methods
	}
}
