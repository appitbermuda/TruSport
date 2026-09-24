using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
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

        async void Handle_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchType;

            await Navigation.PushAsync(new StatPage(item));

            MatchTypeList.SelectedItems.Clear();
        }
    }
}
