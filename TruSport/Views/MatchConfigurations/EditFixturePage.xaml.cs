using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MatchConfigurations
{
    public partial class EditFixturePage : ContentPage
    {
        MatchEditFixtureViewModel matchEditFixtureViewModel;

        public EditFixturePage()
        {
            InitializeComponent();
        }

        public EditFixturePage(Fixture fixture)
        {
            matchEditFixtureViewModel = new MatchEditFixtureViewModel(Navigation, fixture);

            this.BindingContext = matchEditFixtureViewModel;

            InitializeComponent();
        }


        async void BackButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void OpenMatchDatePicker(object sender, System.EventArgs e)
        {
            MatchDatePicker.IsOpen = !MatchDatePicker.IsOpen;
        }

        void OpenMatchTimePicker(object sender, System.EventArgs e)
        {
            MatchTimePicker.IsOpen = !MatchTimePicker.IsOpen;
        }

        //private void LeagueSelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        //{
        //    DisplayAlert("Selection Changed", "SelectedIndex: " + leagueComboBox.SelectedIndex, "OK");
        //}
        //void OpenLeaguePicker(object sender, System.EventArgs e)
        //{
        //    LeaguePicker.IsOpen = true;
        //}

        //void OpenMatchTypePicker(object sender, System.EventArgs e)
        //{
        //    MatchTypePicker.IsOpen = true;
        //}

        //void OpenFieldPicker(object sender, System.EventArgs e)
        //{
        //    FieldPicker.IsOpen = true;
        //}

        //void OpenHomeTeamPicker(object sender, System.EventArgs e)
        //{
        //    HomeTeamPicker.IsOpen = true;
        //}

        //void OpenAwayTeamPicker(object sender, System.EventArgs e)
        //{
        //    AwayTeamPicker.IsOpen = true;
        //}

        void AwayTeamSelected(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
