using System;
using System.Collections.Generic;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MyTeam
{
    public partial class RosterPage : ContentPage
    {
        TeamRosterViewModel teamRosterViewModel;

        public RosterPage()
        {
            teamRosterViewModel = new TeamRosterViewModel(Navigation);

            this.BindingContext = teamRosterViewModel;

            InitializeComponent();
        }
    }
}
