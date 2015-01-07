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

using GoogleAds;
using GoogleAds.ExampleApps.BannerExampleCode.Resources;

namespace GoogleAds.ExampleApps.BannerExampleCode
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // NOTE: Edit "MY_AD_UNIT_ID" with your ad unit id.
            AdView bannerAd = new AdView
            {
                Format = AdFormats.Banner,
                AdUnitID = "MY_AD_UNIT_ID"
            };
            bannerAd.ReceivedAd += OnAdReceived;
            bannerAd.FailedToReceiveAd += OnFailedToReceiveAd;
            LayoutRoot.Children.Add(bannerAd);
            AdRequest adRequest = new AdRequest();
            adRequest.ForceTesting = true;
            bannerAd.LoadAd(adRequest);
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            Debug.WriteLine("Received ad successfully");
        }

        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs errorCode)
        {
            Debug.WriteLine("Failed to receive ad with error " + errorCode.ErrorCode);
        }
    }
}