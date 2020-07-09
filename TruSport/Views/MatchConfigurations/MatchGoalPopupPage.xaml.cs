using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruSport.Model;
using TruSport.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TruSport.Views.MatchConfigurations
{
    public partial class MatchGoalPopupPage : ContentPage
    {
        AssignMatchViewModel assignMatchViewModel;

        public MatchGoalPopupPage()
        {
            InitializeComponent();
        }

        public MatchGoalPopupPage(Fixture fixture)
        {
            assignMatchViewModel = new AssignMatchViewModel(Navigation, fixture);
            this.BindingContext = assignMatchViewModel;

            //On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);

            InitializeComponent();
        }

        private async void BackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        SearchBar searchBar = null;

        private void HomeTeam_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (HomeRosterItemListView.DataSource != null)
            {
                this.HomeRosterItemListView.DataSource.Filter = FilterFixtures;
                this.HomeRosterItemListView.DataSource.RefreshFilter();
            }
        }

        private void AwayTeam_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (AwayRosterItemListView.DataSource != null)
            {
                this.AwayRosterItemListView.DataSource.Filter = FilterFixtures;
                this.AwayRosterItemListView.DataSource.RefreshFilter();
            }
        }

        private bool FilterFixtures(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var player = obj as Player;
            try
            {
                if (player.FirstName.ToLower().Contains(searchBar.Text.ToLower()) || player.LastName.ToLower().Contains(searchBar.Text.ToLower()))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
