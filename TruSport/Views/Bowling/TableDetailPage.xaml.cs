using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModel.Bowling;
using Xamarin.Forms;

namespace TruSport.Views.Bowling
{
    public partial class TableDetailPage : ContentPage
    {
        TableDetailPageViewModel tableDetailPageViewModel;

        public TableDetailPage(BowlingLeagueStanding table)
        {
            tableDetailPageViewModel = new TableDetailPageViewModel(Navigation, table);

            InitializeComponent();

            this.BindingContext = tableDetailPageViewModel;
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
