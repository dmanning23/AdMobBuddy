using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using GoogleAds.ExampleApps.InterstitialExample.Resources;

namespace GoogleAds.ExampleApps.InterstitialExample
{
    public partial class MainPage : PhoneApplicationPage
    {
        private InterstitialAd interstitialAd;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            showInterstitial.IsEnabled = false;
            requestInterstitial.IsEnabled = true;
            requestInterstitial.Click += OnRequestInterstitialClick;
            showInterstitial.Click += OnShowInterstitialClick;
        }

        private void OnShowInterstitialClick(object sender, EventArgs e)
        {
            // Show Interstitial can only be clicked after an interstitial is received.
            interstitialAd.ShowAd();
        }

        private void OnRequestInterstitialClick(object sender, EventArgs e)
        {
            // NOTE: Edit "MY_AD_UNIT_ID" with your interstitial
            // ad unit id.
            interstitialAd = new InterstitialAd("MY_AD_UNIT_ID");
            // NOTE: You can edit the event handler to do something custom here. Once the
            // interstitial is received it can be shown whenever you want.
            interstitialAd.ReceivedAd += OnAdReceived;
            interstitialAd.FailedToReceiveAd += OnFailedToReceiveAd;
            interstitialAd.DismissingOverlay += OnDismissingOverlay;
            AdRequest adRequest = new AdRequest();
            adRequest.ForceTesting = true;
            interstitialAd.LoadAd(adRequest);
            showInterstitial.IsEnabled = false;
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            Debug.WriteLine("Received interstitial successfully. Click 'Show Interstitial'.");
            requestInterstitial.IsEnabled = false;
            showInterstitial.IsEnabled = true;
        }

        private void OnDismissingOverlay(object sender, AdEventArgs e)
        {
            Debug.WriteLine("Dismissing interstitial.");
            showInterstitial.IsEnabled = false;
            requestInterstitial.IsEnabled = true;
        }

        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs errorCode)
        {
            Debug.WriteLine("Failed to receive interstitial with error " + errorCode.ErrorCode);
            showInterstitial.IsEnabled = false;
            requestInterstitial.IsEnabled = true;
        }
    }
}