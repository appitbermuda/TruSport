using System;
using System.Collections.Generic;
using TruSport.Services;
using TruSport.ViewModel;
using TruSport.Views.Basketball;
using TruSport.Views.Bowling;
using TruSport.Views.Cricket;
using TruSport.Views.Tennis;
using TruSport.Views.Triathlon;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class SportsPage : ContentPage
    {
        FixtureService fixtureService;

        SportsPageViewModel sportsPageViewModel;

        public SportsPage()
        {
            sportsPageViewModel = new SportsPageViewModel(Navigation);
            fixtureService = new FixtureService();

            InitializeComponent();

            this.BindingContext = sportsPageViewModel;
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
                    else if(sport.Sport == Constants.Bowling)
                        App.Current.MainPage = new BowlingMasterDetailPage();
                    else if (sport.Sport == Constants.Tennis)
                        App.Current.MainPage = new TennisMasterDetailPage();
                    else if (sport.Sport == Constants.Basketball)
                        App.Current.MainPage = new BasketballMasterDetailPage();
                    else if (sport.Sport == Constants.Triathlon)
                        App.Current.MainPage = new TriathlonFlyoutPage();
                    else
                        App.Current.MainPage = new FootballMasterDetailPage();
                }
            }
        }

        void ShowAlert(string message)
        => MainThread.BeginInvokeOnMainThread(()
        => DisplayAlert("Notification", message, "OK").ContinueWith((task)
            => { if (task.IsFaulted) throw task.Exception; }));
    }
}
