using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Syncfusion.DataSource;
using Syncfusion.SfCalendar.XForms;
using TruSport.Model;
using TruSport.ViewModels;
using TruSport.Views.Admin;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class FixturePage : ContentPage
    {
        FixturePageViewModel fixturePageViewModel;

        public FixturePage()
        {
            fixturePageViewModel = new FixturePageViewModel(Navigation);
            this.BindingContext = fixturePageViewModel;
            InitializeComponent();

            //loader.Easing = Easing.CubicInOut;

            //PastFixtureList.DataSource.SortDescriptors.Add(new SortDescriptor { PropertyName = "Date", Direction = ListSortDirection.Descending });

            PastFixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "League.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Fixture);
                    return item.League.Name + item.Date;
                }
            });

            UpcomingFixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "League.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Fixture);
                    return item.League.Name + item.Date;
                }
            });

            //upcomingCalendar.TranslateTo(0, upcomingCalendar.Height, 1200, Easing.BounceOut);
            //upcomingCalendar.LayoutTo(new Rectangle(upcomingCalendar.Bounds.X, upcomingCalendar.Bounds.Y, upcomingCalendar.Bounds.Width, 0), 500, Easing.CubicIn);
            //fixturePageViewModel.PropertyChanged += FixturePageViewModel_PropertyChanged;
        }

        private void FixturePageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsUpcomingCalendarVisible") return;

            var viewModel = (FixturePageViewModel)sender;
            if (viewModel.IsUpcomingCalendarVisible)
            {
                upcomingCalendar.TranslateTo(0, 0, 1200, Easing.BounceOut);
                //upcomingCalendar.LayoutTo(new Rectangle(upcomingCalendar.Bounds.X, upcomingCalendar.Bounds.Y, 400, 400), 500, Easing.CubicOut);
            }
            else
            {
                upcomingCalendar.TranslateTo(0, upcomingCalendar.Height, 1200, Easing.BounceOut);
                //upcomingCalendar.LayoutTo(new Rectangle(upcomingCalendar.Bounds.X, upcomingCalendar.Bounds.Y, upcomingCalendar.Bounds.Width, 0), 500, Easing.CubicIn);
            }
        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if(e.Index == 0)
            {
                PastLabel.TextColor = Color.White;
                UpcomingLabel.TextColor = Color.Gray;
                LiveLabel.TextColor = Color.Gray;
            }
            else if (e.Index == 1)
            {
                UpcomingLabel.TextColor = Color.White;
                LiveLabel.TextColor = Color.Gray;
                PastLabel.TextColor = Color.Gray;
            }
            else if (e.Index == 2)
            {
                LiveLabel.TextColor = Color.White;
                UpcomingLabel.TextColor = Color.Gray;
                PastLabel.TextColor = Color.Gray;
            }
        }

        private async void PullToRefreshPastFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshPast.IsRefreshing = true;
            await Task.Delay(2000);

            await fixturePageViewModel.RefreshPastFixtures();

            pullToRefreshPast.IsRefreshing = false;
        }

        private async void PullToRefreshUpcomingFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshUpcoming.IsRefreshing = true;
            await Task.Delay(2000);

            await fixturePageViewModel.RefreshUpcomingFixtures();

            pullToRefreshUpcoming.IsRefreshing = false;
        }

        private async void PullToRefreshLiveFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshLive.IsRefreshing = true;
            await Task.Delay(2000);

            await fixturePageViewModel.RefreshLiveFixtures();

            pullToRefreshLive.IsRefreshing = false;
        }

        async void PastFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Fixture)PastFixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            PastFixtureList.SelectedItems.Clear();
        }

        async void UpcomingFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Fixture)UpcomingFixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            UpcomingFixtureList.SelectedItems.Clear();
        }

        async void LiveFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (LiveFixture)LiveFixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            LiveFixtureList.SelectedItems.Clear();
        }

        async void AdminClicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushModalAsync(new AdminMainPage());
        }

        SearchBar searchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (fixturePageViewModel.IsUpcomingCalendarVisible)
            {
                fixturePageViewModel.IsUpcomingCalendarVisible = false;
                upcomingCalendar.SelectedDates.Clear();
            }

            searchBar = (sender as SearchBar);
            if (UpcomingFixtureList.DataSource != null)
            {
                this.UpcomingFixtureList.DataSource.Filter = FilterFixtures;
                this.UpcomingFixtureList.DataSource.RefreshFilter();
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
                    || fixture.Date.ToString().ToLower().Contains(searchBar.Text.ToLower()) || fixture.League.Name.ToLower().Contains(searchBar.Text.ToLower())
                    || (fixture.HomeTeam.Name.ToLower() + " " + fixture.League.Name.ToLower()).Contains(searchBar.Text.ToLower())
                    || (fixture.AwayTeam.Name.ToLower() + " " + fixture.League.Name.ToLower()).Contains(searchBar.Text.ToLower()))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        DateTime selectedDate = new DateTime();
        void CalendarCellTapped(object sender, CalendarTappedEventArgs e)
        {
            SfCalendar calendar = (sender as SfCalendar);
            selectedDate = e.DateTime;

            if (UpcomingFixtureList.DataSource != null)
            {
                this.UpcomingFixtureList.DataSource.Filter = FilterFixturesByDate;
                this.UpcomingFixtureList.DataSource.RefreshFilter();
            }

            if (fixturePageViewModel.IsUpcomingCalendarVisible)
                fixturePageViewModel.IsUpcomingCalendarVisible = false;
        }

        private bool FilterFixturesByDate(object obj)
        {
            if (selectedDate < DateTime.Now)
                return true;

            var fixture = obj as Fixture;
            try
            {
                if (fixture.Date == selectedDate.Date)
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
