using Android.Gms.Ads;
using System;
using System.Diagnostics;

namespace AdMobBuddy.Android
{
	internal class AdCallback : FullScreenContentCallback
	{
		#region Properties

		private AdMobAdapter Adapter { get; set; }

		#endregion //Properties

		#region Methods

		public AdCallback(AdMobAdapter adpater)
		{
			Adapter = adpater;
		}

		public override void OnAdFailedToShowFullScreenContent(AdError error)
		{
			Debug.WriteLine($"Ad failed to load, error code: {error.Code}, {error.Message}");

			base.OnAdFailedToShowFullScreenContent(error);
		}

		public override void OnAdDismissedFullScreenContent()
		{
			Debug.WriteLine("Ad closed.");

			base.OnAdDismissedFullScreenContent();

			Adapter.LoadInterstitialAd();
		}

		public override void OnAdShowedFullScreenContent()
		{
			Debug.WriteLine("Ad displayed.");

			base.OnAdShowedFullScreenContent();
		}

		#endregion //Methods
	}
}
