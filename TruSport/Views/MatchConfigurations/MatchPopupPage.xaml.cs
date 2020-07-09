using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruSport.Model;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MatchConfigurations
{
    public partial class MatchPopupPage : ContentPage
    {
        AssignMatchViewModel assignMatchViewModel;

        public MatchPopupPage()
        {
            InitializeComponent();
        }

        public MatchPopupPage(MatchRoster player)
        {
            assignMatchViewModel = new AssignMatchViewModel(Navigation, player);
            this.BindingContext = assignMatchViewModel;

            InitializeComponent();
        }


        public MatchPopupPage(MatchRoster player, Fixture fixture)
        {
            assignMatchViewModel = new AssignMatchViewModel(Navigation, player, fixture);
            this.BindingContext = assignMatchViewModel;

            InitializeComponent();
        }

        private async void BackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
