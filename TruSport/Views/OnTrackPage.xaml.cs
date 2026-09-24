using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TruSport.Services;
using TruSport.ViewModel.Football;
using TruSport.Views.Basketball;
using TruSport.Views.Bowling;
using TruSport.Views.Cricket;
using TruSport.Views.Golf;
using TruSport.Views.Tennis;
using TruSport.Views.Triathlon;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.Database != null)
            {
                var sport = await App.Database.GetDefaultSport();

                if (sport != null && !String.IsNullOrEmpty(sport.Sport))
                {
                    if (sport.Sport == Constants.Cricket)
                        App.Current.MainPage = new CricketMasterDetailPage();
                    else if (sport.Sport == Constants.Bowling)
                        App.Current.MainPage = new BowlingMasterDetailPage();
                    else if (sport.Sport == Constants.Tennis)
                        App.Current.MainPage = new TennisMasterDetailPage();
                    else if (sport.Sport == Constants.Basketball)
                        App.Current.MainPage = new BasketballMasterDetailPage();
                    else if (sport.Sport == Constants.Triathlon)
                        App.Current.MainPage = new TriathlonFlyoutPage();
                    else if (sport.Sport == Constants.Golf)
                        App.Current.MainPage = new GolfFlyoutPage();
                    else
                        App.Current.MainPage = new FootballMasterDetailPage();
                }
            }
        }
    }
}
