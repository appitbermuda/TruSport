using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class EditPlayerPage : ContentPage
    {
        PlayerAdminPageViewModel playerAdminPageViewModel;

        public EditPlayerPage()
        {
            playerAdminPageViewModel = new PlayerAdminPageViewModel(Navigation);

            this.BindingContext = playerAdminPageViewModel;
            InitializeComponent();
        }

        public EditPlayerPage(PlayerSeason player = null)
        {
            playerAdminPageViewModel = new PlayerAdminPageViewModel(Navigation, player);

            this.BindingContext = playerAdminPageViewModel;
            InitializeComponent();
        }

        async void BackButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
