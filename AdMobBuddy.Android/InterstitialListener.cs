using Android.Gms.Ads;
using System;

namespace AdMobBuddy.Android
{
	internal class InterstitialListener : AdListener
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
			Console.WriteLine("Interstitial ad received and ready to be displayed.");
			if (null != OnInterstitialLoaded)
			{
				OnInterstitialLoaded(this, new EventArgs());
			}

			base.OnAdLoaded();
		}

		public override void OnAdClosed()
		{
			Console.WriteLine("Interstitial ad closed.");
			base.OnAdClosed();
			Adapter.LoadInterstitialAd();
		}

		public override void OnAdOpened()
		{
			Console.WriteLine("Interstitial ad opened.");
			base.OnAdOpened();
		}

		public override void OnAdFailedToLoad(int errorCode)
		{
			Console.WriteLine($"Interstitial ad failed to load, error code: {errorCode}");
			base.OnAdFailedToLoad(errorCode);
		}

		#endregion //Methods
	}
}
