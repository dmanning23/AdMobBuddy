using AdMobBuddy;
using MenuBuddy;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RevMobBuddyExample
{
	public class MainMenu : MenuStackScreen
	{
		bool reward = false;

		public MainMenu() : base("AdMobBuddy Example")
		{
			CoverOtherScreens = true;
			CoveredByOtherScreens = true;
		}

		public override async Task LoadContent()
		{
			await base.LoadContent();

			//add banner button
			var banner = new MenuEntry("Banner Ad", Content);
			AddMenuEntry(banner);
			banner.OnClick += (obj, e) =>
			{
				var ads = ScreenManager.Game.Services.GetService<IAdManager>();
				ads.DisplayBannerAd();
			};

			//add interstitial button
			var interstitial = new MenuEntry("Interstitial Ad", Content);
			AddMenuEntry(interstitial);
			interstitial.OnClick += (obj, e) =>
			{
				var ads = ScreenManager.Game.Services.GetService<IAdManager>();
				ads.DisplayInterstitialAd();
			};

			//add rewarded video button
			var rewardedVideo = new MenuEntry("Rewarded Video Ad", Content);
			AddMenuEntry(rewardedVideo);
			rewardedVideo.OnClick += (obj, e) =>
			{
				var ads = ScreenManager.Game.Services.GetService<IAdManager>();
				ads.OnVideoReward -= VideoReward;
				ads.OnVideoReward += VideoReward;
				ads.DisplayRewardedVideoAd();
			};
		}

		protected void VideoReward(object obj, RewardedVideoEventArgs e)
		{
			var ads = ScreenManager.Game.Services.GetService<IAdManager>();
			ads.OnVideoReward -= VideoReward;
			reward = true;
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			if (reward)
			{
				reward = false;
				ScreenManager.AddScreen(new OkScreen("You got a video reward!", Content));
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}
	}
}