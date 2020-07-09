using System;
using System.Collections.Generic;
using TruSport.ViewModels.Cricket;
using Xamarin.Forms;

namespace TruSport.Views.Cricket
{
    public partial class LeagueStatsPage : ContentPage
    {
        LeagueStatPageViewModel leagueStatPageViewModel;
        public LeagueStatsPage()
        {
            leagueStatPageViewModel = new LeagueStatPageViewModel();

            this.BindingContext = leagueStatPageViewModel;
            InitializeComponent();
        }
    }
}
