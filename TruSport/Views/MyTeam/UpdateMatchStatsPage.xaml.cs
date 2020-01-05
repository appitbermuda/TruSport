using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MyTeam
{
    public partial class UpdateMatchStatsPage : ContentPage
    {
        TeamFixtureViewModel teamFixtureViewModel;

        public UpdateMatchStatsPage()
        {
            teamFixtureViewModel = new TeamFixtureViewModel(Navigation);

            this.BindingContext = teamFixtureViewModel;
            InitializeComponent();
        }

        public UpdateMatchStatsPage(Fixture fixture)
        {
            teamFixtureViewModel = new TeamFixtureViewModel(Navigation, fixture, "");

            this.BindingContext = teamFixtureViewModel;
            InitializeComponent();
        }

        async void BackButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
