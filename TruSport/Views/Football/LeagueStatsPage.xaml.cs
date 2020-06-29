using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class LeagueStatsPage : ContentPage
    {
        LeagueStatPageViewModel leagueStatPageViewModel;
        public LeagueStatsPage()
        {
            leagueStatPageViewModel = new LeagueStatPageViewModel();

            this.BindingContext = leagueStatPageViewModel;

            InitializeComponent();

            PlayerGoalsList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as LeagueStat);
                    return item.LeagueName;
                }
            });

            TeamScoredList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as LeagueStat);
                    return item.LeagueName;
                }
            });

            TeamConcededList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as LeagueStat);
                    return item.LeagueName;
                }
            });
        }
    }
}
