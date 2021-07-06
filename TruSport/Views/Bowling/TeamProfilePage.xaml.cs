using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels.Bowling;
using Xamarin.Forms;

namespace TruSport.Views.Bowling
{
    public partial class TeamProfilePage : ContentPage
    {
        TeamProfilePageViewModel teamProfilePageViewModel;

        public TeamProfilePage(Team Team)
        {
            teamProfilePageViewModel = new TeamProfilePageViewModel(Navigation, Team);
            this.BindingContext = teamProfilePageViewModel;

            InitializeComponent();

            if (Team.Alias == null || Team.Alias == "")
                TitleLabel.Text = Team.Name;
            else
                TitleLabel.Text = Team.Alias;

            FixtureListView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "League.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as BowlingFixture);
                    return item.League.Name + item.Date;
                }
            });

            //TransferList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            //{
            //    PropertyName = "PreviousTeam",
            //    KeySelector = (object obj1) =>
            //    {
            //        var item = (obj1 as Transfer);
            //        return item.PreviousTeam;
            //    }
            //});
        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {

            if (e.Index == 3)
            {
                try
                {
                    var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                    var fixtureIndex = teamProfilePageViewModel.FixtureCollection.IndexOf(fixture);

                    int index = FixtureListView.DataSource.DisplayItems.IndexOf(teamProfilePageViewModel.FixtureCollection[fixtureIndex]);

                    if (index != 0)
                    {
                        FixtureListView.LayoutManager.ScrollToRowIndex(index + 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                    }
                    else
                    {
                        // Programmatic scrolling based on the item index.
                        FixtureListView.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                    }
                }
                catch (Exception ex)
                { }
            }
        }

        void PositionContainer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                TeamProfileTab.SelectedIndex = TeamProfileTab.Items.IndexOf(TeamProfileTab.Items.FirstOrDefault(x => x.Title == "Table"));
            }
            catch (Exception ex)
            { }
        }

        void FormContainer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                TeamProfileTab.SelectedIndex = TeamProfileTab.Items.IndexOf(TeamProfileTab.Items.FirstOrDefault(x => x.Title == "Fixtures"));
            }
            catch (Exception ex)
            { }
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
