using Android.Gms.Ads;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using RevMobBuddy;
using System;
using Color = Android.Graphics.Color;

namespace RevMobBuddyExample.Android
{
	public class AndroidAdMobAdManager : IAdManager
	{
		#region Properties

		public event EventHandler OnVideoReward;

		Game _game;

		LinearLayout _adContainer;

		AdView _banner;

		#endregion //Properties

		#region Methods

		public AndroidAdMobAdManager(Game game)
		{
			_game = game;
		}

		public void DisplayInterstitialAd()
		{
			throw new NotImplementedException();
		}

		public void DisplayRewardedVideoAd()
		{
			throw new NotImplementedException();
		}

		public void DisplayVideoAd()
		{
			throw new NotImplementedException();
		}

		public void HideBannerAd()
		{
			if (null != _adContainer && null != _banner)
			{
				_adContainer.RemoveView(_banner);

				var activity = (Activity1)Game.Activity;
				activity._mainLayout.RemoveView(_adContainer);
			}

			_adContainer = null;
			_banner = null;
		}

		public void ShowBannerAd()
		{
			if (null != _adContainer || null != _banner)
			{
				//There is already a banner ad displayed
				HideBannerAd();
			}

			// A container to show the add at the bottom of the page
			var activity = (Activity1)Game.Activity;

			// The actual ad
			_banner = new AdView(activity)
			{
				AdUnitId = "ca-app-pub-3940256099942544/6300978111", // Get this id from admob "Monetize" tab
				AdSize = AdSize.SmartBanner,
			};

			_adContainer = new LinearLayout(activity);
			_adContainer.Orientation = Orientation.Horizontal;
			_adContainer.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.Bottom);
			_adContainer.SetBackgroundColor(Color.Transparent); // Need on some devices, not sure why

			activity._mainLayout.AddView(_adContainer);
			_adContainer.AddView(_banner);

			_banner.LoadAd(new AdRequest.Builder()
				.AddTestDevice("DEADBEEF9A2078B6AC72133BB7E6E177") // Prevents generating real impressions while testing
				.Build());
		}

		public void Initialize()
		{
		}

		#endregion //Methods
	}
}