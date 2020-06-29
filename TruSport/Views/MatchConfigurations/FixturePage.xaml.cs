using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MatchConfigurations
{
    public partial class FixturePage : ContentPage
    {
        MatchFixtureViewModel matchFixtureViewModel;

        Fixture Fixture;
        bool NotBackNavigation = false;

        public FixturePage()
        {
            //NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        public FixturePage(Fixture fixture)
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            //NavigationPage.SetHasBackButton(this, false);
            //NavigationPage.SetHasBackButton(this, false);
            //NavigationPage.SetHasNavigationBar(this, false);
            NotBackNavigation = true;
            Fixture = fixture;
            matchFixtureViewModel = new MatchFixtureViewModel(Navigation, fixture);

            this.BindingContext = matchFixtureViewModel;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //if(!NotBackNavigation)
            //{
            //    matchFixtureViewModel.GenerateSource(Fixture);
            //}

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Send<string>("FixtureList", "Refresh");
        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                SquadLabel.TextColor = Color.White;
                MatchSummaryLabel.TextColor = Color.Gray;
            }
            else if (e.Index == 1)
            {
                MatchSummaryLabel.TextColor = Color.White;
                SquadLabel.TextColor = Color.Gray;
            }
        }


        async void OnHomeTeamSelected(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchRoster;

            //if (items.Count == 0) return;

            //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;

            await Navigation.PushModalAsync(new MatchPopupPage(item));

            HomeRosterItemListView.SelectedItems.Clear();
        }

        async void OnAwayTeamSelected(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchRoster;


            //var item = (MatchRoster)AwayRosterItemListView.SelectedItem;

            await Navigation.PushModalAsync(new MatchPopupPage(item));

            AwayRosterItemListView.SelectedItems.Clear();
        }

        async void BackButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
