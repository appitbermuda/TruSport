using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Bowling;
using Xamarin.Forms;

namespace TruSport.Views.Bowling
{
    public partial class FavouritePage : ContentPage
    {
        FavouritePageViewModel favouritePageViewModel;

        public FavouritePage()
        {
            favouritePageViewModel = new FavouritePageViewModel();

            InitializeComponent();

            this.BindingContext = favouritePageViewModel;
        }

        async void TeamSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Team)TeamsList.SelectedItem;

            await Navigation.PushAsync(new Bowling.TeamProfilePage(item));

            TeamsList.SelectedItems.Clear();
        }

        async void FixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (BowlingFixture)FavouriteFixtureList.SelectedItem;

            await Navigation.PushAsync(new Bowling.FixtureDetailsPage(item));

            FavouriteFixtureList.SelectedItems.Clear();
        }
    }
}
