using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels.Bowling;
using Xamarin.Forms;

namespace TruSport.Views.Bowling
{
    public partial class FixtureDetailsPage : ContentPage
    {
        FixtureDetailPageViewModel fixtureDetailPageViewModel;

        public FixtureDetailsPage(BowlingFixture fixture)
        {
            fixtureDetailPageViewModel = new FixtureDetailPageViewModel(Navigation, fixture);

            InitializeComponent();

            this.BindingContext = fixtureDetailPageViewModel;

            BowlingResultListView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "Game",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as BowlingGameResult);
                    return item.Game;
                }
            });

            //BowlingResultListView.DataSource.SortDescriptors.Add(new SortDescriptor()
            //{
            //    PropertyName = "Game",
            //    Direction = ListSortDirection.Ascending
            //});
        }

        void SfTabView_TabItemTapped(System.Object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            try
            {
                if ((e.TabItem.Title == "Table" && (fixtureDetailPageViewModel.TableCollection == null || fixtureDetailPageViewModel.TableCollection.Count == 0)) || (e.TabItem.Title == "Head to Head" && (fixtureDetailPageViewModel.HeadToHeadFixtureCollection == null || fixtureDetailPageViewModel.HeadToHeadFixtureCollection.Count == 0)))
                    e.Cancel = true;
            }
            catch (Exception ex)
            {

            }
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
