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
        CompetitionsDetailPageViewModel competitionsDetailPageViewModel;
        VisualContainer visualContainer;
        public bool isScrolled;
        HeaderItem headerItem;

        public CompetitionDetailsPage(League League)
        {
            Title = League.Name;
            //TitleLabel.Text = League.Name;
            competitionsDetailPageViewModel = new CompetitionsDetailPageViewModel(Navigation, League);

            this.BindingContext = competitionsDetailPageViewModel;
            InitializeComponent();

            FixtureList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "MatchType.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Fixture);
                    return item.MatchType.Name + item.Date;
                }
            });
        }

        async void OnFixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Fixture)FixtureList.SelectedItem;

            await Navigation.PushAsync(new FixtureDetailsPage(item));

            FixtureList.SelectedItems.Clear();
        }

        //private async void LoadMoreOnTop()
        private async void LoadMoreClicked(object sender, EventArgs e)
        {
            try
            {
                competitionsDetailPageViewModel.IsLoadMoreVisible = false;
                //To get the current first item which is visible in the View.
                var firstItem = FixtureList.DataSource.DisplayItems[1];
                competitionsDetailPageViewModel.IsActivityVisible = true;
                await Task.Delay(4000);
                var r = new Random();

                //To avoid layout calls for arranging each and every items to be added in the View. 
                FixtureList.DataSource.BeginInit();

                var fixtureFloat = competitionsDetailPageViewModel.FixtureFloatCollection.OrderBy(x => x.Date).ToList();
                var startFixture = fixtureFloat.OrderBy(x => x.Date).FirstOrDefault(x => x.Date > DateTime.Now);
                var startIndex = fixtureFloat.IndexOf(startFixture);

                if (startIndex > 0)
                {
                    var fixtures = competitionsDetailPageViewModel.FixtureFloatCollection.Take(startIndex).ToList();
                    for (int i = 0; i < startIndex; i++)
                    {
                        competitionsDetailPageViewModel.FixtureCollection.Insert(0, fixtures[i]);
                    }
                    FixtureList.DataSource.EndInit();

                    var firstItemIndex = FixtureList.DataSource.DisplayItems.IndexOf(firstItem);
                    //var header = (FixtureList.HeaderTemplate != null && !FixtureList.IsStickyHeader) ? 1 : 0;
                    var totalItems = firstItemIndex;

                    //Need to scroll back to previous position else the ScrollViewer moves to top of the list.
                    FixtureList.LayoutManager.ScrollToRowIndex(totalItems, true);
                    competitionsDetailPageViewModel.IsActivityVisible = false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Load More");
            }
        }

        void SfTabView_TabItemTapped(System.Object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            try
            {
                if (e.TabItem.Title == "Table" && !competitionsDetailPageViewModel.IsTableExist)
                    e.Cancel = true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tab Item Tapped");
            }
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
