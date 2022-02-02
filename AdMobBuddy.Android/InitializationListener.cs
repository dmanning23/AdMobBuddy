using Android.Gms.Ads.Initialization;
using Java.Lang;
using System.Diagnostics;

namespace AdMobBuddy.Android
{
	public class InitializationListener : Object, IOnInitializationCompleteListener
	{
		#region Properties

		private AdMobAdapter Adapter { get; set; }

		#endregion //Properties

		#region Methods

		public InitializationListener(AdMobAdapter adpater)
		{
			Adapter = adpater;
		}

		public void OnInitializationComplete(IInitializationStatus p0)
		{
			Debug.WriteLine($"OnInitializationComplete: {p0.AdapterStatusMap["com.google.android.gms.ads.MobileAds"].InitializationState}");

			Adapter.LoadInterstitialAd();
			Adapter.LoadRewardedVideo();
		}

		#endregion //Methods
	}
}