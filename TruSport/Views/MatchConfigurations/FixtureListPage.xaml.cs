using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MatchConfigurations
{
    public partial class FixtureListPage : ContentPage
    {
        MatchFixtureViewModel matchFixtureViewModel;

        public FixtureListPage()
        {
            matchFixtureViewModel = new MatchFixtureViewModel(Navigation);

            this.BindingContext = matchFixtureViewModel;

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

        private async void PullToRefreshFixtures_Refreshing(object sender, EventArgs args)
        {
            pullToRefresh.IsRefreshing = true;
            await Task.Delay(2000);

            //await matchFixtureViewModel.RefreshFixtures();

            pullToRefresh.IsRefreshing = false;
        }

        async void FixtureSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Fixture)FixtureList.SelectedItem;

            await Navigation.PushAsync(new FixturePage(item));

            FixtureList.SelectedItems.Clear();
        }
    }
}
