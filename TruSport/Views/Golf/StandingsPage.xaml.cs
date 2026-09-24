using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Golf;
using Xamarin.Forms;

namespace TruSport.Views.Golf
{
    public partial class StandingsPage : ContentPage
    {
        StandingsPageViewModel standingsPageViewModel;

        public StandingsPage()
        {
            standingsPageViewModel = new StandingsPageViewModel();

            InitializeComponent();

            this.BindingContext = standingsPageViewModel;
        }

        async void Handle_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as League;

            await Navigation.PushAsync(new TablePage(item));

            LeagueList.SelectedItems.Clear();
        }
    }
}
