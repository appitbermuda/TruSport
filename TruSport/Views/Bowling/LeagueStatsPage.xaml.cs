using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels.Bowling;
using Xamarin.Forms;

namespace TruSport.Views.Bowling
{
    public partial class LeagueStatsPage : ContentPage
    {
        LeagueStatPageViewModel leagueStatPageViewModel;

        public LeagueStatsPage()
        {
            leagueStatPageViewModel = new LeagueStatPageViewModel();

            InitializeComponent();

            this.BindingContext = leagueStatPageViewModel;

            PlayerSeasonHGList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as LeagueStat);
                    return item.LeagueName;
                }
            });

            PlayerSeasonHSList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as LeagueStat);
                    return item.LeagueName;
                }
            });
            TeamSeasonHGList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "LeagueName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as LeagueStat);
                    return item.LeagueName;
                }
            });

            TeamSeasonHSList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
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
