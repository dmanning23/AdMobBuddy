using AdMobBuddy;
using AdMobBuddy.Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace RevMobBuddyExample.Android
{
	[Activity(Label = "AdMob Example"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, ScreenOrientation = ScreenOrientation.SensorPortrait
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			var g = new Game1();
			SetContentView((View)g.Services.GetService(typeof(View)));

			g.Services.AddService<IAdManager>(new AdMobAdapter(this, 
				"ca-app-pub-3940256099942544/6300978111", //banner ad id
				"ca-app-pub-3940256099942544/1033173712", //interstitial ad id
				"ca-app-pub-3940256099942544/5224354917", //rewarded video ad id
				"34A3D7C62F372FD18218F63F37D12398", //test device id
				false)); //child directed flag

			g.Run();
		}

		/// <summary>
		/// When the app resumes, make sure to hide the navbars again
		/// </summary>
		protected override void OnResume()
		{
			base.OnResume();

			HideNavBars();
		}

		/// <summary>
		/// This method hides the Android nav bar
		/// </summary>
		private void HideNavBars()
		{
			//Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutStable | SystemUiFlags.LayoutHideNavigation | SystemUiFlags.LayoutFullscreen | SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky);
		}
	}
}

