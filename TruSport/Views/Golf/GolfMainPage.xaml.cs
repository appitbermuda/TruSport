using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Syncfusion.SfCalendar.XForms;
using TruSport.Model;
using TruSport.ViewModels.Golf;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views.Golf
{
    public partial class GolfMainPage : ContentPage
    {
        GolfPageViewModel golfPageViewModel;
        VisualContainer visualContainer;
        public bool isScrolled;
        HeaderItem headerItem;

        public GolfMainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            golfPageViewModel = new GolfPageViewModel(Navigation);

            this.BindingContext = golfPageViewModel;
            InitializeComponent();

            //PastFixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            //{
            //    PropertyName = "Date",
            //    KeySelector = (object obj1) =>
            //    {
            //        var item = (obj1 as GolfFixture);
            //        return item.League.Name + item.Date;
            //    }
            //});

            FixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "Date",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as GolfFixture);
                    return item.Date + item.Time;
                }
            });
        }

        private void GolfPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsUpcomingCalendarVisible") return;

            var viewModel = (GolfPageViewModel)sender;
            if (viewModel.IsUpcomingCalendarVisible)
            {
                upcomingCalendar.TranslateTo(0, 0, 1200, Easing.BounceIn);
            }
            else
            {
                upcomingCalendar.TranslateTo(0, upcomingCalendar.Height, 1200, Easing.BounceIn);
            }
        }

        private async void LoadMoreClicked(object sender, EventArgs e)
        {
            try
            {
                golfPageViewModel.IsLoadMoreVisible = false;
                //To get the current first item which is visible in the View.
                var firstItem = FixtureList.DataSource.DisplayItems[1];
                //golfPageViewModel.IsActivityVisible = true;
                await Task.Delay(4000);
                var r = new Random();

                //To avoid layout calls for arranging each and every items to be added in the View. 
                FixtureList.DataSource.BeginInit();

                var fixtureFloat = golfPageViewModel.FixtureFloatCollection.OrderBy(x => x.Date).ToList();
                var startFixture = fixtureFloat.OrderBy(x => x.Date).FirstOrDefault(x => x.Date > DateTime.Now);
                var startIndex = fixtureFloat.IndexOf(startFixture);

                if (startIndex > 0)
                {
                    var fixtures = golfPageViewModel.FixtureFloatCollection.Take(startIndex).ToList();
                    for (int i = 0; i < startIndex; i++)
                    {
                        golfPageViewModel.FixtureCollection.Insert(0, fixtures[i]);
                    }
                    FixtureList.DataSource.EndInit();

                    var firstItemIndex = FixtureList.DataSource.DisplayItems.IndexOf(firstItem);
                    //var header = (FixtureList.HeaderTemplate != null && !FixtureList.IsStickyHeader) ? 1 : 0;
                    var totalItems = firstItemIndex;

                    //Need to scroll back to previous position else the ScrollViewer moves to top of the list.
                    FixtureList.LayoutManager.ScrollToRowIndex(totalItems, true);
                    //golfPageViewModel.IsActivityVisible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Load More");
            }
        }
        //private async void PullToRefreshPastFixtures_Refreshing(object sender, EventArgs args)
        //{
        //    pullToRefreshPast.IsRefreshing = true;
        //    await Task.Delay(2000);

        //    await golfPageViewModel.RefreshPastFixtures();

        //    pullToRefreshPast.IsRefreshing = false;
        //}

        private async void PullToRefreshUpcomingFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshUpcoming.IsRefreshing = true;
            await Task.Delay(2000);

            await golfPageViewModel.RefreshUpcomingFixtures();

            pullToRefreshUpcoming.IsRefreshing = false;
        }

        //private async void PullToRefreshLiveFixtures_Refreshing(object sender, EventArgs args)
        //{
        //    pullToRefreshLive.IsRefreshing = true;
        //    await Task.Delay(2000);

        //    await golfPageViewModel.RefreshLiveFixtures();

        //    pullToRefreshLive.IsRefreshing = false;
        //}

        //async void PastFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        //{
        //    var items = e.AddedItems;

        //    if (items.Count == 0) return;

        //    var item = (GolfFixture)PastFixtureList.SelectedItem;

        //    await Navigation.PushAsync(new FixtureDetailsPage(item));

        //    PastFixtureList.SelectedItems.Clear();
        //}

        async void UpcomingFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (GolfFixture)FixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            FixtureList.SelectedItems.Clear();
        }

        //async void LiveFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        //{
        //    var items = e.AddedItems;

        //    if (items.Count == 0) return;

        //    var item = (GolfFixture)LiveFixtureList.SelectedItem;

        //    await Navigation.PushAsync(new FixtureDetailsPage(item));

        //    LiveFixtureList.SelectedItems.Clear();
        //}

        SearchBar upcomingSearchBar = null;
        SearchBar previousSearchBar = null;

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (golfPageViewModel.IsUpcomingCalendarVisible)
            {
                golfPageViewModel.IsUpcomingCalendarVisible = false;
                upcomingCalendar.SelectedDates.Clear();
            }

            upcomingSearchBar = (sender as SearchBar);
            if (FixtureList.DataSource != null)
            {
                this.FixtureList.DataSource.Filter = FilterUpcomingFixtures;
                this.FixtureList.DataSource.RefreshFilter();
            }
        }

        //private void PreviousSearchTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    previousSearchBar = (sender as SearchBar);
        //    if (PastFixtureList.DataSource != null)
        //    {
        //        this.PastFixtureList.DataSource.Filter = FilterPreviousFixtures;
        //        this.PastFixtureList.DataSource.RefreshFilter();
        //    }
        //}

        private bool FilterPreviousFixtures(object obj)
        {
            if (previousSearchBar == null || previousSearchBar.Text == null)
                return true;

            var fixture = obj as GolfFixture;
            try
            {
                if (fixture.League.Name.ToLower().Contains(previousSearchBar.Text.ToLower()) || fixture.Date.ToString().ToLower().Contains(previousSearchBar.Text.ToLower()))
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

            var fixture = obj as GolfFixture;
            try
            {
                if (fixture.League.Name.ToLower().Contains(upcomingSearchBar.Text.ToLower()) || fixture.Date.ToString().ToLower().Contains(upcomingSearchBar.Text.ToLower()))
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

            if (FixtureList.DataSource != null)
            {
                this.FixtureList.DataSource.Filter = FilterFixturesByDate;
                this.FixtureList.DataSource.RefreshFilter();
            }

            if (golfPageViewModel.IsUpcomingCalendarVisible)
                golfPageViewModel.IsUpcomingCalendarVisible = false;
        }

        private bool FilterFixturesByDate(object obj)
        {
            if (selectedDate < DateTime.Now)
                return true;

            var fixture = obj as GolfFixture;
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
