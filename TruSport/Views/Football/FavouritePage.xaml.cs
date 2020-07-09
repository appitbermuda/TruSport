using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using TruSport.Views.Admin;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views.Football
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

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            //if (e.Index == 0)
            //{
            //    TeamLabel.TextColor = Color.White;
            //    FixtureLabel.TextColor = Color.Gray;

            //    //TeamSelected.IsVisible = true;
            //    //FixtureSelected.IsVisible = false;
            //}
            //else if (e.Index == 1)
            //{
            //    FixtureLabel.TextColor = Color.White;
            //    TeamLabel.TextColor = Color.Gray;

            //    //TeamSelected.IsVisible = false; 
            //    //FixtureSelected.IsVisible = true;
            //}
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

            var item = (Fixture)FavouriteFixtureList.SelectedItem;

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
