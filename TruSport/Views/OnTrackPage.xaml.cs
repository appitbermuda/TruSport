using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TruSport.Services;
using TruSport.ViewModel.Football;
using TruSport.Views.Football;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class OnTrackPage : ContentPage
    {
        OnTrackPageViewModel onTrackPageViewModel;
        public OnTrackPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            onTrackPageViewModel = new OnTrackPageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = onTrackPageViewModel;
        }
    }
}
