using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels;
using TruSport.Views.Admin;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class TeamProfilePage : ContentPage
    {
        TeamProfilePageViewModel teamProfilePageViewModel;

        public TeamProfilePage()
        {
            InitializeComponent();

            //loader.Easing = Easing.Linear;
            //tableloader.Easing = Easing.Linear;

            FixtureListView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "League.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Fixture);
                    return item.League.Name + item.Date;
                }
            });

            TransferList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "PreviousTeam",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Transfers);
                    return item.PreviousTeam;
                }
            });
        }

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
                    var item = (obj1 as Fixture);
                    return item.League.Name + item.Date;
                }
            });

            TransferList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "PreviousTeam",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Transfers);
                    return item.PreviousTeam;
                }
            });
        }

        async void AdminClicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushModalAsync(new AdminMainPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                ProfileLabel.TextColor = Color.White;
                FixtureLabel.TextColor = Color.Gray;
                PlayerLabel.TextColor = Color.Gray;
                TableLabel.TextColor = Color.Gray;
                TransferLabel.TextColor = Color.Gray;

                //ProfileSelected.IsVisible = true;

                //PlayerSelected.IsVisible = false;
                //TableSelected.IsVisible = false;
                //FixtureSelected.IsVisible = false;
            }
            else if (e.Index == 1)
            {
                PlayerLabel.TextColor = Color.White;
                ProfileLabel.TextColor = Color.Gray;
                FixtureLabel.TextColor = Color.Gray;
                TableLabel.TextColor = Color.Gray;
                TransferLabel.TextColor = Color.Gray;

                //ProfileSelected.IsVisible = false;
                //PlayerSelected.IsVisible = true;
                //TableSelected.IsVisible = false;
                //FixtureSelected.IsVisible = false;
            }
            else if (e.Index == 2)
            {
                TableLabel.TextColor = Color.White;
                ProfileLabel.TextColor = Color.Gray;
                PlayerLabel.TextColor = Color.Gray;
                FixtureLabel.TextColor = Color.Gray;
                TransferLabel.TextColor = Color.Gray;

                //ProfileSelected.IsVisible = false;
                //PlayerSelected.IsVisible = false;
                //TableSelected.IsVisible = true;
                //FixtureSelected.IsVisible = false;
            }
            else if (e.Index == 3)
            {
                FixtureLabel.TextColor = Color.White;
                ProfileLabel.TextColor = Color.Gray;
                PlayerLabel.TextColor = Color.Gray;
                TableLabel.TextColor = Color.Gray;
                TransferLabel.TextColor = Color.Gray;

                //ProfileSelected.IsVisible = false;
                //PlayerSelected.IsVisible = false;
                //TableSelected.IsVisible = false;
                //FixtureSelected.IsVisible = true;
            }
            else if (e.Index == 4)
            {
                TransferLabel.TextColor = Color.White;
                FixtureLabel.TextColor = Color.Gray;
                ProfileLabel.TextColor = Color.Gray;
                PlayerLabel.TextColor = Color.Gray;
                TableLabel.TextColor = Color.Gray;
                

                //ProfileSelected.IsVisible = false;
                //PlayerSelected.IsVisible = false;
                //TableSelected.IsVisible = false;
                //FixtureSelected.IsVisible = true;
            }

            if (e.Index == 3)
            {
                try
                {
                    //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddMonths(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                    //var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                    var fixture = teamProfilePageViewModel.FixtureCollection.Where(x => x.Date < DateTime.Now.AddDays(-1)).OrderByDescending(x => x.Date).ThenBy(x => x.Time).FirstOrDefault();
                    var fixtureIndex = teamProfilePageViewModel.FixtureCollection.IndexOf(fixture);

                    int index = FixtureListView.DataSource.DisplayItems.IndexOf(teamProfilePageViewModel.FixtureCollection[fixtureIndex]);

                    if (index != 0)
                    {
                        // Programmatic scrolling based on the item index.
                        FixtureListView.LayoutManager.ScrollToRowIndex(index + 1, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                        // Programmatic scrolling based on the item data.
                        //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index - 1], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                    }
                    else
                    {
                        // Programmatic scrolling based on the item index.
                        FixtureListView.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                        // Programmatic scrolling based on the item data.
                        //FixtureListView.ScrollTo(teamProfilePageViewModel.FixtureCollection[index], Syncfusion.ListView.XForms.ScrollToPosition.Start, true);
                    }
                }
                catch(Exception ex)
                { }
            }
        }
    }
}
