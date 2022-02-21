using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Basketball;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class FavouritePage : ContentPage
    {
        FavouritePageViewModel favouritePageViewModel;

        public FavouritePage()
        {
            favouritePageViewModel = new FavouritePageViewModel();

            InitializeComponent();

            this.BindingContext = favouritePageViewModel;

            //loader.Easing = Easing.Linear;
        }

        async void TeamSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Team)TeamsList.SelectedItem;

            await Navigation.PushAsync(new TeamProfilePage(item));

            TeamsList.SelectedItems.Clear();
        }

        async void FixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (BasketballFixture)FavouriteFixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            FavouriteFixtureList.SelectedItems.Clear();
        }

        async void DeleteClicked(object sender, System.EventArgs e)
        {
            var deleteAll = await DisplayAlert("Clear Favourites", "Are you sure you want to clear your favourites list?", "Delete All", "Cancel");

            if (deleteAll)
            {
                try
                {
                    //await App.Database.DeleteAllFavourites();
                    //FavouriteFixtureList.ItemsSource = favouritePageViewModel.FavouriteFixturesCollection;
                    //TeamsList.ItemsSource = favouritePageViewModel.FavouriteTeamCollection;

                }
                catch (Exception ex)
                { }

            }
        }
    }
}
