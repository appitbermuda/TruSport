using System;
using System.Collections.Generic;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MyTeam
{
    public partial class EditPlayerPage : ContentPage
    {
        TeamRosterViewModel teamRosterViewModel;

        public EditPlayerPage()
        {
            InitializeComponent();
        }

        public EditPlayerPage(string PlayerID)
        {
            teamRosterViewModel = new TeamRosterViewModel(Navigation, PlayerID);

            this.BindingContext = teamRosterViewModel;
            InitializeComponent();
        }
    }
}
