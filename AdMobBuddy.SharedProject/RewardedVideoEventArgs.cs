using System;

namespace AdMobBuddy
{
	/// <summary>
	/// The event arguments that get fired off when the user finishes watching a rewarded video ad
	/// </summary>
	public class RewardedVideoEventArgs : EventArgs
	{
		public bool Success { get; set; }

		public RewardedVideoEventArgs(bool success)
		{
			Success = success;
		}
	}
}
