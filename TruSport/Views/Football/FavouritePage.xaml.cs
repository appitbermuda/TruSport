using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using TruSport.Views.Admin;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class FavouritePage : ContentPage
    {
        

        public FavouritePage()
        {
            

            InitializeComponent();

            //loader.Easing = Easing.Linear;
        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                TeamLabel.TextColor = Color.White;
                FixtureLabel.TextColor = Color.Gray;

                //TeamSelected.IsVisible = true;
                //FixtureSelected.IsVisible = false;
            }
            else if (e.Index == 1)
            {
                FixtureLabel.TextColor = Color.White;
                TeamLabel.TextColor = Color.Gray;

                //TeamSelected.IsVisible = false; 
                //FixtureSelected.IsVisible = true;
            }
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

        async void AdminClicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushModalAsync(new AdminMainPage());
        }

        async void DeleteClicked(object sender, System.EventArgs e)
        {
            var deleteAll = await DisplayAlert("Clear Favourites", "Are you sure you want to clear your favourites list?", "Delete All", "Cancel");

            if (deleteAll)
            {
                try
                {
                    //await databaseManager.DeleteAllFavourites(App.UserID);
                }
                catch (Exception ex)
                { }

                viewModel = new FavouritePageViewModel();
                FavouriteFixtureList.ItemsSource = viewModel.FavouriteFixturesCollection;
                TeamsList.ItemsSource = viewModel.FavouriteTeamCollection;

                //List<Favourite> favourites = new List<Favourite>();
                //favourites = await databaseManager.GetFavourites();

                //var favouriteGroups = favourites.GroupBy(f => f.Type).Select(g => new Grouping<string, Favourite>(g.Key, g));
                //FavouriteGroups = new ObservableCollection<Grouping<string, Favourite>>(favouriteGroups);
                //FavouritesListView.ItemsSource = FavouriteGroups;

                //if (favourites.Count == 0)
                    //DeleteFavourites.IsEnabled = false;
            }
        }
    }
}
