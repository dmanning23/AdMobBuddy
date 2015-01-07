using Xamarin.Forms;

namespace AdMobBuddy.Forms.Plugin.Abstractions
{
	/// <summary>
	/// AdMobBuddy Interface
	/// The AdMobBuddyControl is a xamarin view that can be added to Forms apps to display AdMob ads.
	/// </summary>
	public class AdMobBuddyControl : View
	{
		/// <summary>
		/// The ID of the AdMob ad to display
		/// This is the string Id from your Google Play account
		/// </summary>
		public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create<AdMobBuddyControl, string>(p => p.AdUnitId, "");

		/// <summary>
		/// The ID of the AdMob ad to display
		/// This is the string Id from your Google Play account
		/// </summary>
		public string AdUnitId
		{
			get { return (string)GetValue(AdUnitIdProperty); }
			set { SetValue(AdUnitIdProperty, value); }
		}
	}
}
