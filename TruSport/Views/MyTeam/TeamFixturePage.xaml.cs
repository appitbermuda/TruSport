using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using TruSport.Model;
using TruSport.ViewModel;
using TruSport.Views.MatchConfigurations;
using Xamarin.Forms;

namespace TruSport.Views.MyTeam
{
    public partial class TeamFixturePage : ContentPage
    {
        TeamFixtureViewModel teamFixtureViewModel;

        public TeamFixturePage()
        {
            InitializeComponent();
        }

        public TeamFixturePage(Fixture fixture)
        {
            //NavigationPage.SetHasBackButton(this, false);
            teamFixtureViewModel = new TeamFixtureViewModel(Navigation, fixture);

            this.BindingContext = teamFixtureViewModel;

            InitializeComponent();
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

        async void BackButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
