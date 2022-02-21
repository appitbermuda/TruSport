using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Basketball;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class CompetitionsPage : ContentPage
    {
        CompetitionsPageViewModel competitionsPageViewModel;
        public CompetitionsPage()
        {
            competitionsPageViewModel = new CompetitionsPageViewModel();

            this.BindingContext = competitionsPageViewModel;
            InitializeComponent();
        }

        async void Handle_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as League;

            await Navigation.PushAsync(new CompetitionDetailsPage(item));

            LeagueList.SelectedItems.Clear();
        }
    }
}
