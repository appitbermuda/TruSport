using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class TeamPage : ContentPage
    {
        TeamAdminPageViewModel teamAdminPageViewModel;

        public TeamPage()
        {
            teamAdminPageViewModel = new TeamAdminPageViewModel();

            this.BindingContext = teamAdminPageViewModel;

            InitializeComponent();
        }

        async void TeamTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Team;

            await Navigation.PushAsync(new EditTeamPage(item));
        }

        async void CloseClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        SearchBar searchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (TeamSyncList.DataSource != null)
            {
                this.TeamSyncList.DataSource.Filter = FilterFixtures;
                this.TeamSyncList.DataSource.RefreshFilter();
            }
        }

        private bool FilterFixtures(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var team = obj as Team;
            try
            {
                if (team.Name.ToLower().Contains(searchBar.Text.ToLower()) || team.League.Name.ToLower().Contains(searchBar.Text.ToLower()))
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
