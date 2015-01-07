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

using GoogleAds.ExampleApps.BannerExample.Resources;

namespace GoogleAds.ExampleApps.BannerExample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
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