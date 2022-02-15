using Android.Gms.Ads;
using System;
using System.Diagnostics;

namespace AdMobBuddy.Android
{
	internal class AdCallback : FullScreenContentCallback
    {

        public event EventHandler AdFailed;
        public event EventHandler AdDismissed;
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
			AdFailed?.Invoke(this, EventArgs.Empty);
			base.OnAdFailedToShowFullScreenContent(error);
		}

		public override void OnAdDismissedFullScreenContent()
		{
			Debug.WriteLine("Ad closed.");

			base.OnAdDismissedFullScreenContent();
            AdDismissed?.Invoke(this, EventArgs.Empty);
        }

		public override void OnAdShowedFullScreenContent()
		{
			Debug.WriteLine("Ad displayed.");

			base.OnAdShowedFullScreenContent();
		}

		#endregion //Methods
	}
}
