using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class PlayerPage : ContentPage
    {
        PlayerAdminPageViewModel playerAdminPageViewModel;

        public PlayerPage()
        {
            playerAdminPageViewModel = new PlayerAdminPageViewModel(Navigation);

            this.BindingContext = playerAdminPageViewModel;

            InitializeComponent();
        }

        async void PlayerTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Team;

            await Navigation.PushAsync(new EditTeamPage(item));
        }

        SearchBar searchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (PlayerList.DataSource != null)
            {
                this.PlayerList.DataSource.Filter = FilterFixtures;
                this.PlayerList.DataSource.RefreshFilter();
            }
        }

        private bool FilterFixtures(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var playerSeason = obj as PlayerSeason;
            try
            {
                if (playerSeason.Player.Name.ToLower().Contains(searchBar.Text.ToLower()) || playerSeason.Player.Name.ToLower().Contains(searchBar.Text.ToLower()))
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
