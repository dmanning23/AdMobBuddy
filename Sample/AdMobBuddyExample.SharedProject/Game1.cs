using MenuBuddy;
using Microsoft.Xna.Framework;

namespace RevMobBuddyExample
{
	public class Game1 : TouchGame
	{
		public Game1()
		{
			Graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;

			VirtualResolution = new Point(720, 1280);
			ScreenResolution = new Point(720, 1280);

#if DESKTOP
			Fullscreen = false;
			Letterbox = true;
#else
			Fullscreen = true;
			Letterbox = false;
#endif
		}

		protected override void InitStyles()
		{
			StyleSheet.LargeFontResource = @"Fonts\ArialBlack96";
			StyleSheet.MediumFontResource = @"Fonts\ArialBlack48";
			StyleSheet.SmallFontResource = @"Fonts\ArialBlack24";

			base.InitStyles();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			ScreenManager.ClearColor = Color.Red;
		}

		public override IScreen[] GetMainMenuScreenStack()
		{
			return new IScreen[] { new MainMenu() };
		}
	}
}
