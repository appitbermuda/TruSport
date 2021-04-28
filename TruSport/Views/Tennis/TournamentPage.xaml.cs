using System;
using System.Collections.Generic;
using TruSport.ViewModels.Tennis;
using Xamarin.Forms;

namespace TruSport.Views.Tennis
{
    public partial class TournamentPage : ContentPage
    {
        TournamentPageViewModel tournamentPageViewModel;

        public TournamentPage()
        {
            tournamentPageViewModel = new TournamentPageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = tournamentPageViewModel;
        }
    }
}
