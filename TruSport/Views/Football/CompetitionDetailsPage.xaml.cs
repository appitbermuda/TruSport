using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class CompetitionDetailsPage : ContentPage
    {
        CompetitionsPageViewModel competitionsPageViewModel;
        VisualContainer visualContainer;
        public bool isScrolled;
        HeaderItem headerItem;

        public CompetitionDetailsPage()
        {
            competitionsPageViewModel = new CompetitionsPageViewModel();

            this.BindingContext = competitionsPageViewModel;
            InitializeComponent();

            FixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "MatchType.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Fixture);
                    return item.MatchType.Name;
                }
            });

            //loader.Easing = Easing.Linear;
        }

        public CompetitionDetailsPage(League League)
        {
            Title = League.Name;
            competitionsPageViewModel = new CompetitionsPageViewModel(Navigation, League.ID);

            this.BindingContext = competitionsPageViewModel;
            InitializeComponent();

            //FixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            //{
            //    PropertyName = "MatchType.Name",
            //    KeySelector = (object obj1) =>
            //    {
            //        var item = (obj1 as Fixture);
            //        return item.MatchType.Name;
            //    }
            //});
            

        }

        protected override void OnAppearing()
        {
            //MessagingCenter.Subscribe<string>("FixtureLoaded", "Refresh", (sender) =>
            //{
            //    //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddMonths(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //    var fixture = competitionsPageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //    var fixtureIndex = competitionsPageViewModel.FixtureCollection.IndexOf(fixture);

            //    if (fixtureIndex - 1 >= 0)
            //    {
            //        fixtureIndex = fixtureIndex - 1;
            //    }

            //    int index = FixtureList.DataSource.DisplayItems.IndexOf(competitionsPageViewModel.FixtureCollection[fixtureIndex]);

            //    if (index != 0)
            //    {
            //        // Programmatic scrolling based on the item index.
            //        FixtureList.LayoutManager.ScrollToRowIndex(index - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //        // Programmatic scrolling based on the item data.
            //        //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index - 1], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //    }
            //    else
            //    {
            //        // Programmatic scrolling based on the item index.
            //        FixtureList.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //        // Programmatic scrolling based on the item data.
            //        //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //    }
            //});
            ////var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddMonths(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //var fixture = competitionsPageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //var fixtureIndex = competitionsPageViewModel.FixtureCollection.IndexOf(fixture);

            //int index = FixtureListOnly.DataSource.DisplayItems.IndexOf(competitionsPageViewModel.FixtureCollection[fixtureIndex]);

            //if (index != 0)
            //{
            //    // Programmatic scrolling based on the item index.
            //    FixtureListOnly.LayoutManager.ScrollToRowIndex(index - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //    // Programmatic scrolling based on the item data.
            //    //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index - 1], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //}
            //else
            //{
            //    // Programmatic scrolling based on the item index.
            //    FixtureListOnly.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //    // Programmatic scrolling based on the item data.
            //    //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //}

            //if (FixtureListOnly != null)
            //{
            //    try
            //    {
            //        //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddMonths(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //        //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //        var fixture = competitionsPageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //        var fixtureIndex = competitionsPageViewModel.FixtureCollection.IndexOf(fixture);

            //        int index = FixtureListOnly.DataSource.DisplayItems.IndexOf(competitionsPageViewModel.FixtureCollection[fixtureIndex]);

            //        if (index != 0)
            //        {
            //            // Programmatic scrolling based on the item index.
            //            FixtureListOnly.LayoutManager.ScrollToRowIndex(index + 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //            // Programmatic scrolling based on the item data.
            //            //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index - 1], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //        }
            //        else
            //        {
            //            // Programmatic scrolling based on the item index.
            //            FixtureListOnly.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //            // Programmatic scrolling based on the item data.
            //            //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //        }
            //    }
            //    catch (Exception ex)
            //    { }
            //}

            //if(FixtureList != null)
            //{
            //    try
            //    {
            //        //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddMonths(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //        //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //        var fixture = competitionsPageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
            //        var fixtureIndex = competitionsPageViewModel.FixtureCollection.IndexOf(fixture);

            //        int index = FixtureList.DataSource.DisplayItems.IndexOf(competitionsPageViewModel.FixtureCollection[fixtureIndex]);

            //        if (index != 0)
            //        {
            //            // Programmatic scrolling based on the item index.
            //            FixtureList.LayoutManager.ScrollToRowIndex(index + 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //            // Programmatic scrolling based on the item data.
            //            //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index - 1], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //        }
            //        else
            //        {
            //            // Programmatic scrolling based on the item index.
            //            FixtureList.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //            // Programmatic scrolling based on the item data.
            //            //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
            //        }
            //    }
            //    catch (Exception ex)
            //    { }
            //}

            base.OnAppearing();
        }



        void TabViewChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                FixtureLabel.TextColor = Color.White;
                TableLabel.TextColor = Color.Gray;

                ////var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddMonths(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                //var fixture = competitionsPageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                //var fixtureIndex = competitionsPageViewModel.FixtureCollection.IndexOf(fixture);

                //if (fixtureIndex - 1 >= 0)
                //{
                //    fixtureIndex = fixtureIndex - 1;
                //}

                //int index = FixtureList.DataSource.DisplayItems.IndexOf(competitionsPageViewModel.FixtureCollection[fixtureIndex]);

                //if (index != 0)
                //{
                //    // Programmatic scrolling based on the item index.
                //    FixtureList.LayoutManager.ScrollToRowIndex(index - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                //    // Programmatic scrolling based on the item data.
                //    //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index - 1], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                //}
                //else
                //{
                //    // Programmatic scrolling based on the item index.
                //    FixtureList.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                //    // Programmatic scrolling based on the item data.
                //    //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                //}

                try
                {
                    //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddMonths(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                    //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                    var fixture = competitionsPageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                    var fixtureIndex = competitionsPageViewModel.FixtureCollection.IndexOf(fixture);

                    int index = FixtureList.DataSource.DisplayItems.IndexOf(competitionsPageViewModel.FixtureCollection[fixtureIndex]);

                    if (index != 0)
                    {
                        // Programmatic scrolling based on the item index.
                        FixtureList.LayoutManager.ScrollToRowIndex(index + 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                        // Programmatic scrolling based on the item data.
                        //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index - 1], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                    }
                    else
                    {
                        // Programmatic scrolling based on the item index.
                        FixtureList.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                        // Programmatic scrolling based on the item data.
                        //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                    }
                }
                catch (Exception ex)
                { }
            }
            else if (e.Index == 1)
            {
                TableLabel.TextColor = Color.White;
                FixtureLabel.TextColor = Color.Gray;
            }
        }

        private async void PullToRefreshFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshFixtures.IsRefreshing = true;
            pullToRefresh.IsRefreshing = true;
            await Task.Delay(2000);

            //await competitionsPageViewModel.RefreshPastFixtures();

            pullToRefreshFixtures.IsRefreshing = false;
            pullToRefresh.IsRefreshing = false;
        }

        async void OnFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Fixture)FixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            FixtureList.SelectedItems.Clear();
        }

        async void OnFixtureOnlySelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Fixture)FixtureListOnly.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            FixtureListOnly.SelectedItems.Clear();
        }

        //private void HeaderItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Visibility")
        //    {
        //        if (headerItem.Visibility && isScrolled)
        //            LoadMoreOnTop();
        //    }
        //}

        //private async void LoadMoreOnTop()
        private async void LoadMoreClicked(object sender, EventArgs e)
        {
            try
            {
                competitionsPageViewModel.IsLoadMoreVisible = false;
                //To get the current first item which is visible in the View.
                var firstItem = FixtureList.DataSource.DisplayItems[1];
                competitionsPageViewModel.IsActivityVisible = true;
                await Task.Delay(4000);
                var r = new Random();

                //To avoid layout calls for arranging each and every items to be added in the View. 
                FixtureList.DataSource.BeginInit();

                var fixtureFloat = competitionsPageViewModel.FixtureFloatCollection.OrderBy(x => x.Date).ToList();
                var startFixture = fixtureFloat.OrderBy(x => x.Date).FirstOrDefault(x => x.Date > DateTime.Now);
                var startIndex = fixtureFloat.IndexOf(startFixture);

                if (startIndex > 0)
                {
                    var fixtures = competitionsPageViewModel.FixtureFloatCollection.Take(startIndex).ToList();
                    for (int i = 0; i < startIndex; i++)
                    {
                        competitionsPageViewModel.FixtureCollection.Insert(0, fixtures[i]);
                    }
                    FixtureList.DataSource.EndInit();

                    var firstItemIndex = FixtureList.DataSource.DisplayItems.IndexOf(firstItem);
                    //var header = (FixtureList.HeaderTemplate != null && !FixtureList.IsStickyHeader) ? 1 : 0;
                    var totalItems = firstItemIndex;

                    //Need to scroll back to previous position else the ScrollViewer moves to top of the list.
                    FixtureList.LayoutManager.ScrollToRowIndex(totalItems, true);
                    competitionsPageViewModel.IsActivityVisible = false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Load More");
            }
        }

        private async void LoadMoreOnlyClicked(object sender, EventArgs e)
        {
            try
            {
                competitionsPageViewModel.IsLoadMoreVisible = false;
                //To get the current first item which is visible in the View.
                var firstItem = FixtureListOnly.DataSource.DisplayItems[0];
                competitionsPageViewModel.IsActivityVisible = true;
                await Task.Delay(4000);
                var r = new Random();

                //To avoid layout calls for arranging each and every items to be added in the View. 
                FixtureListOnly.DataSource.BeginInit();

                var fixtureFloat = competitionsPageViewModel.FixtureFloatCollection.OrderBy(x => x.Date).ToList();
                var startFixture = fixtureFloat.OrderBy(x => x.Date).FirstOrDefault(x => x.Date > DateTime.Now);
                var startIndex = fixtureFloat.IndexOf(startFixture);

                if (startIndex > 0)
                {
                    var fixtures = competitionsPageViewModel.FixtureFloatCollection.Take(startIndex).ToList();
                    for (int i = 0; i < startIndex; i++)
                    {
                        competitionsPageViewModel.FixtureCollection.Insert(0, fixtures[i]);
                    }
                    FixtureListOnly.DataSource.EndInit();

                    var firstItemIndex = FixtureListOnly.DataSource.DisplayItems.IndexOf(firstItem);
                    var header = (FixtureListOnly.HeaderTemplate != null && !FixtureListOnly.IsStickyHeader) ? 1 : 0;
                    var totalItems = firstItemIndex + header;

                    //Need to scroll back to previous position else the ScrollViewer moves to top of the list.
                    //FixtureList.LayoutManager.ScrollToRowIndex(totalItems, true);
                    competitionsPageViewModel.IsActivityVisible = false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Load More");
            }
}

        //private void ListView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        //{
        //    //To avoid loading items initially when page loaded.
        //    if (!isScrolled)
        //        (FixtureList.LayoutManager as LinearLayout).ScrollToRowIndex(competitionsPageViewModel.FixtureCollection.Count - 1, true);
        //    headerItem = visualContainer.Children[0] as HeaderItem;
        //    headerItem.PropertyChanged += HeaderItem_PropertyChanged;
        //    isScrolled = true;
        //}
    }
}
