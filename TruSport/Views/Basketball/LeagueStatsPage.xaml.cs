using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels.Basketball;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class LeagueStatsPage : ContentPage
    {
        LeagueStatPageViewModel leagueStatPageViewModel;
        public LeagueStatsPage()
        {
            leagueStatPageViewModel = new LeagueStatPageViewModel();

            this.BindingContext = leagueStatPageViewModel;

            InitializeComponent();

            PlayerStatsList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as BasketballLeagueStat);
                    return item.LeagueName;
                }
            });

            TeamScoredList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as BasketballLeagueStat);
                    return item.LeagueName;
                }
            });

            TeamConcededList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as BasketballLeagueStat);
                    return item.LeagueName;
                }
            });
        }
    }
}
