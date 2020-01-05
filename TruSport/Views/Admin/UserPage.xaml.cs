using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class UserPage : ContentPage
    {
        UserPageViewModel userPageViewModel;

        public UserPage()
        {
            userPageViewModel = new UserPageViewModel(Navigation);

            BindingContext = userPageViewModel;

            InitializeComponent();
        }

        //async void AddClicked(object sender, System.EventArgs e)
        //{
        //    await Navigation.PushAsync(new EditUserPage());
        //}


        async void UserTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as User;

            UserSyncList.SelectedItem = null;
            //UserSyncList.SelectedItems.Clear();
            await Navigation.PushAsync(new EditUserPage(item));
        }

        async void CloseClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        SearchBar searchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (UserSyncList.DataSource != null)
            {
                this.UserSyncList.DataSource.Filter = FilterFixtures;
                this.UserSyncList.DataSource.RefreshFilter();
            }
        }

        private bool FilterFixtures(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var user = obj as User;
            try
            {
                if (user.FirstName.ToLower().Contains(searchBar.Text.ToLower()) || user.LastName.ToLower().Contains(searchBar.Text.ToLower())
                    || user.UserType.Name.ToLower().Contains(searchBar.Text.ToLower()))
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
