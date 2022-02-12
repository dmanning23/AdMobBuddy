using Foundation;
using Google.MobileAds;
using System;
using System.Diagnostics;

namespace AdMobBuddy.iOS
{
	public class RewardedVideoListener : RewardBasedVideoAdDelegate
	{
		#region Properties

		public event EventHandler OnRewardedVideoLoaded;

		public event EventHandler<RewardedVideoEventArgs> OnVideoReward;

        public event EventHandler OnRewardedVideoFailed;
        public event EventHandler OnRewardedVideoDismissed;

        private bool _rewardReceived;

		private AdMobAdapter Adapter { get; set; }

		#endregion //Properties

		#region Methods

		public RewardedVideoListener(AdMobAdapter adapter)
		{
			Adapter = adapter;
        }
		
		public override void DidRewardUser(RewardBasedVideoAd rewardBasedVideoAd, AdReward reward)
		{
			Debug.WriteLine($"Rewarded user!!!");
            _rewardReceived = true;
			OnVideoReward?.Invoke(this, new RewardedVideoEventArgs(true));
		}

		public override void DidFailToLoad(RewardBasedVideoAd rewardBasedVideoAd, NSError error)
		{
			Debug.WriteLine($"Reward based video ad failed to load with error: {error.LocalizedDescription}.");
			OnRewardedVideoFailed?.Invoke(this, EventArgs.Empty);
		}

		public override void DidReceiveAd(RewardBasedVideoAd rewardBasedVideoAd)
		{
			Debug.WriteLine("Reward based video ad is received.");

			OnRewardedVideoLoaded?.Invoke(this, new EventArgs());
		}

		public override void DidOpen(RewardBasedVideoAd rewardBasedVideoAd)
		{
			Debug.WriteLine("Opened reward based video ad.");
		}

		public override void DidStartPlaying(RewardBasedVideoAd rewardBasedVideoAd)
        {
            _rewardReceived = false;
			Debug.WriteLine("Reward based video ad started playing.");
		}

		public override void DidClose(RewardBasedVideoAd rewardBasedVideoAd)
		{
			Debug.WriteLine("Reward based video ad is closed.");
            if(!_rewardReceived)
				OnRewardedVideoDismissed?.Invoke(this, EventArgs.Empty); 
			Adapter.LoadRewardedVideoAd();
		}

		public override void WillLeaveApplication(RewardBasedVideoAd rewardBasedVideoAd)
		{
			Debug.WriteLine("Rewarded Video clicked! Reward based video ad will leave application.");
		}

		#endregion //Methods
	}
}