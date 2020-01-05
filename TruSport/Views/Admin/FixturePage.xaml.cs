using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class FixturePage : ContentPage
    {
        public FixturePage()
        {
            this.BindingContext = new FixtureAdminPageViewModel(Navigation);

            InitializeComponent();

            FixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "Date",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Fixture);
                    return item.Date + TimeSpan.Parse(item.Time);
                }
            });
        }


        //async void FixtureTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        //{
        //    var item = e.ItemData as FixtureListView;

        //    await Navigation.PushAsync(new EditFixturesPage(item));
        //}

        async void AddClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new EditFixturesPage());
        }

        async void CloseClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        SearchBar searchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (FixtureList.DataSource != null)
            {
                this.FixtureList.DataSource.Filter = FilterFixtures;
                this.FixtureList.DataSource.RefreshFilter();
            }
        }

        private bool FilterFixtures(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var fixture = obj as Fixture;
            try
            {
                if (fixture.HomeTeam.Name.ToLower().Contains(searchBar.Text.ToLower()) || fixture.AwayTeam.Name.ToLower().Contains(searchBar.Text.ToLower())
                    || fixture.Date.ToString("MMMM d, yyyy").ToLower().Contains(searchBar.Text.ToLower()) || fixture.League.Name.ToLower().Contains(searchBar.Text.ToLower()))
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
