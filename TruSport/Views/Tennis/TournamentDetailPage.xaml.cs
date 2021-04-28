using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels.Tennis;
using Xamarin.Forms;

namespace TruSport.Views.Tennis
{
    public partial class TournamentDetailPage : ContentPage
    {
        TournamentDetailPageViewModel tournamentDetailPageViewModel;

        public TournamentDetailPage(TennisTournament tennisTournament)
        {
            tournamentDetailPageViewModel = new TournamentDetailPageViewModel(Navigation, tennisTournament);
            InitializeComponent();

            this.BindingContext = tournamentDetailPageViewModel;

            FixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "TournamentMatchType.Tournament.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as TennisFixture);
                    return item.TournamentMatchType.Tournament.Name + item.Date;
                }
            });
        }

        private async void PullToRefreshFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefresh.IsRefreshing = true;
            await Task.Delay(2000);

            await tournamentDetailPageViewModel.RefreshFixtures();

            pullToRefresh.IsRefreshing = false;
        }

        SearchBar searchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (FixtureList.DataSource != null)
            {
                this.FixtureList.DataSource.Filter = FilterFixtures;
                this.FixtureList.DataSource.RefreshFilter();
            }
        }

        private bool FilterFixtures(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var fixture = obj as TennisFixture;
            try
            {
                if (fixture.TournamentMatchType.Tournament.Name.ToLower().Contains(searchBar.Text.ToLower())
                    || fixture.TennisMatch[0].Player1.Player.Name.ToLower().Contains(searchBar.Text.ToLower())
                    || (fixture.TennisMatch[0].Player2 != null && fixture.TennisMatch[0].Player2.Player.Name.ToLower().Contains(searchBar.Text.ToLower()))
                    || fixture.Date.ToString().ToLower().Contains(searchBar.Text.ToLower())
                    || fixture.TennisMatch[1].Player1.Player.Name.ToLower().Contains(searchBar.Text.ToLower())
                    || (fixture.TennisMatch[1].Player2 != null && fixture.TennisMatch[1].Player2.Player.Name.ToLower().Contains(searchBar.Text.ToLower())))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
