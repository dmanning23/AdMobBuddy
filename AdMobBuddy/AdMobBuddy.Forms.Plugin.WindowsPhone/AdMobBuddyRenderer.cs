using AdMobBuddy.Forms.Plugin.Abstractions;
using AdmobBuddy.Forms.Plugin.WindowsPhone;
using GoogleAds;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(AdMobBuddyControl), typeof(AdMobBuddyRenderer))]
namespace AdmobBuddy.Forms.Plugin.WindowsPhone
{
	/// <summary>
	/// Admob buddy control renderer for winphone
	/// </summary>
	public class AdMobBuddyRenderer : ViewRenderer
	{
		/// <summary>
		/// reload the view and hit up google admob 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			//convert the element to the control we want
			var adMobElement = Element as AdMobBuddyControl;

			if ((adMobElement != null) && (e.OldElement == null))
			{
				AdView bannerAd = new AdView
				{
					Format = AdFormats.Banner,
					AdUnitID = adMobElement.AdUnitId,
				};
				AdRequest adRequest = new AdRequest();
				bannerAd.LoadAd(adRequest);
				Children.Add(bannerAd);
			}
		}
	}
}
