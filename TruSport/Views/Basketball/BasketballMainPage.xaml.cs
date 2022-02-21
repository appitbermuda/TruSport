using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.DataSource;
using Syncfusion.SfCalendar.XForms;
using TruSport.Model;
using TruSport.ViewModels.Basketball;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class BasketballMainPage : ContentPage
    {
        BasketballPageViewModel basketballPageViewModel;

        public BasketballMainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            basketballPageViewModel = new BasketballPageViewModel(Navigation);

            this.BindingContext = basketballPageViewModel;
            InitializeComponent();

            PastFixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "League.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as BasketballFixture);
                    return item.League.Name + item.Date;
                }
            });

            UpcomingFixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "League.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as BasketballFixture);
                    return item.League.Name + item.Date;
                }
            });
        }

        private void FixturePageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsUpcomingCalendarVisible") return;

            var viewModel = (FixturePageViewModel)sender;
            if (viewModel.IsUpcomingCalendarVisible)
            {
                upcomingCalendar.TranslateTo(0, 0, 1200, Easing.BounceIn);
            }
            else
            {
                upcomingCalendar.TranslateTo(0, upcomingCalendar.Height, 1200, Easing.BounceIn);
            }
        }

        private async void PullToRefreshPastFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshPast.IsRefreshing = true;
            await Task.Delay(2000);

            await basketballPageViewModel.RefreshPastFixtures();

            pullToRefreshPast.IsRefreshing = false;
        }

        private async void PullToRefreshUpcomingFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshUpcoming.IsRefreshing = true;
            await Task.Delay(2000);

            await basketballPageViewModel.RefreshUpcomingFixtures();

            pullToRefreshUpcoming.IsRefreshing = false;
        }

        private async void PullToRefreshLiveFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshLive.IsRefreshing = true;
            await Task.Delay(2000);

            await basketballPageViewModel.RefreshLiveFixtures();

            pullToRefreshLive.IsRefreshing = false;
        }

        async void PastFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (BasketballFixture)PastFixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            PastFixtureList.SelectedItems.Clear();
        }

        async void UpcomingFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (BasketballFixture)UpcomingFixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            UpcomingFixtureList.SelectedItems.Clear();
        }

        async void LiveFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (BasketballFixture)LiveFixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            LiveFixtureList.SelectedItems.Clear();
        }

        SearchBar upcomingSearchBar = null;
        SearchBar previousSearchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (basketballPageViewModel.IsUpcomingCalendarVisible)
            {
                basketballPageViewModel.IsUpcomingCalendarVisible = false;
                upcomingCalendar.SelectedDates.Clear();
            }

            upcomingSearchBar = (sender as SearchBar);
            if (UpcomingFixtureList.DataSource != null)
            {
                this.UpcomingFixtureList.DataSource.Filter = FilterUpcomingFixtures;
                this.UpcomingFixtureList.DataSource.RefreshFilter();
            }
        }

        private void PreviousSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            previousSearchBar = (sender as SearchBar);
            if (PastFixtureList.DataSource != null)
            {
                this.PastFixtureList.DataSource.Filter = FilterPreviousFixtures;
                this.PastFixtureList.DataSource.RefreshFilter();
            }
        }

        private bool FilterPreviousFixtures(object obj)
        {
            if (previousSearchBar == null || previousSearchBar.Text == null)
                return true;

            var fixture = obj as BasketballFixture;
            try
            {
                if (fixture.HomeTeam.Name.ToLower().Contains(previousSearchBar.Text.ToLower()) || fixture.AwayTeam.Name.ToLower().Contains(previousSearchBar.Text.ToLower())
                    || fixture.Date.ToString().ToLower().Contains(previousSearchBar.Text.ToLower()) || fixture.League.Name.ToLower().Contains(previousSearchBar.Text.ToLower())
                    || (fixture.HomeTeam.Name.ToLower() + " " + fixture.League.Name.ToLower()).Contains(previousSearchBar.Text.ToLower())
                    || (fixture.AwayTeam.Name.ToLower() + " " + fixture.League.Name.ToLower()).Contains(previousSearchBar.Text.ToLower()))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool FilterUpcomingFixtures(object obj)
        {
            if (upcomingSearchBar == null || upcomingSearchBar.Text == null)
                return true;

            var fixture = obj as BasketballFixture;
            try
            {
                if (fixture.HomeTeam.Name.ToLower().Contains(upcomingSearchBar.Text.ToLower()) || fixture.AwayTeam.Name.ToLower().Contains(upcomingSearchBar.Text.ToLower())
                    || fixture.Date.ToString().ToLower().Contains(upcomingSearchBar.Text.ToLower()) || fixture.League.Name.ToLower().Contains(upcomingSearchBar.Text.ToLower())
                    || (fixture.HomeTeam.Name.ToLower() + " " + fixture.League.Name.ToLower()).Contains(upcomingSearchBar.Text.ToLower())
                    || (fixture.AwayTeam.Name.ToLower() + " " + fixture.League.Name.ToLower()).Contains(upcomingSearchBar.Text.ToLower()))
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

            if (basketballPageViewModel.IsUpcomingCalendarVisible)
                basketballPageViewModel.IsUpcomingCalendarVisible = false;
        }

        private bool FilterFixturesByDate(object obj)
        {
            if (selectedDate < DateTime.Now)
                return true;

            var fixture = obj as BasketballFixture;
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

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.MainPage is FlyoutPage mdp)
            {
                mdp.IsPresented = true;
            }
        }

        async void ShowHidePOW_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                bool showPOW;
                // get reference to the layout to animate
                var layout = this.FindByName<StackLayout>("POWContainer");
                // setup information for animation
                Action<double> callback = input => { layout.HeightRequest = input; }; // update the height of the layout with this callback
                double startingHeight = 0; // the layout's height when we begin animation
                double endingHeight = 0; // final desired height of the layout
                uint rate = 5; // pace at which aniation proceeds
                uint length = 1000; // one second animation
                Easing easing = Easing.CubicOut; // There are a couple easing types, just tried this one for effect

                if (layout.Height <= 0)
                {
                    POWContainer.IsVisible = true;
                    showPOW = true;
                    startingHeight = 0; // the layout's height when we begin animation
                    endingHeight = 422; // final desired height of the layout
                }
                else
                {
                    showPOW = false;
                    startingHeight = layout.Height; // the layout's height when we begin animation
                    endingHeight = -10; // final desired height of the layout
                }

                // now start animation with all the setup information
                layout.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing);

                await SecureStorage.SetAsync("ShowPOW", showPOW.ToString());
            }
            catch (Exception ex)
            { }

        }
    }
}
