using AdMobBuddy.Forms.Plugin.Abstractions;
using System;
using AdMobBuddy.Forms.Plugin.Droid;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdMobBuddy.Forms.Plugin.Abstractions.AdMobBuddyControl), typeof(AdMobBuddyRenderer))]
namespace AdMobBuddy.Forms.Plugin.Droid
{
	/// <summary>
	/// AdMobBuddy control Renderer for Android
	/// </summary>
	public class AdMobBuddyRenderer : ViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			//convert the element to the control we want
			var adMobElement = Element as AdMobBuddyControl;

			if ((adMobElement != null) && (e.OldElement == null))
			{
				AdView ad = new AdView(this.Context);
				ad.AdSize = AdSize.Banner;
				ad.AdUnitId = adMobElement.AdUnitId;
				var requestbuilder = new AdRequest.Builder();
				ad.LoadAd(requestbuilder.Build());
				this.SetNativeControl(ad);
			}
		}
	}
}