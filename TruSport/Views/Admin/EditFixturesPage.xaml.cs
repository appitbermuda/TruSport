using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class EditFixturesPage : ContentPage
    {
        
        FixtureAdminPageViewModel fixtureAdminPageViewModel;

        public EditFixturesPage()
        {
            
            fixtureAdminPageViewModel = new FixtureAdminPageViewModel(Navigation);
            this.BindingContext = fixtureAdminPageViewModel;
            InitializeComponent();
        }

        public EditFixturesPage(Fixture fixture)
        {
            

            fixtureAdminPageViewModel = new FixtureAdminPageViewModel(Navigation, fixture);
            this.BindingContext = fixtureAdminPageViewModel;

            InitializeComponent();
        }

        void OpenMatchDatePicker(object sender, System.EventArgs e)
        {
            MatchDatePicker.IsOpen = !MatchDatePicker.IsOpen;
        }

        void OpenMatchTimePicker(object sender, System.EventArgs e)
        {
            MatchTimePicker.IsOpen = !MatchTimePicker.IsOpen;
        }


        //void HomeTeamSelected(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    var homeTeamValue = HomeTeamPicker.SelectedItem as String;
        //    UserTypeLabel.Text = homeTeamValue;

        //    userTypeID = userPageViewModel.UserTypeCollection.Where(x => x.Name == userTypeValue).FirstOrDefault().ID;
        //}

        void AwayTeamSelected(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        async void BackClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

            async void SaveClicked(object sender, System.EventArgs e)
        {
            try
            {
                if (fixtureAdminPageViewModel.SaveCommand.CanExecute(null))
                    fixtureAdminPageViewModel.SaveCommand.Execute(null);

                //Fixture fixture = new Fixture();
                //if (thisFixture.ID == null || thisFixture.ID == "")
                //{
                //    fixture = new Fixture
                //    {
                //        HomeTeamID = "",
                //        AwayTeamID = "",
                //        Date = DateTime.Now,
                //        MatchTypeID = "",
                //        Time = DateTime.Now,
                //        FieldID = "",
                //        LeagueID = "",
                //    };
                //}
                //else
                //{
                //    fixture = new Fixture
                //    {
                //        HomeTeamID = "",
                //        AwayTeamID = "",
                //        Date = DateTime.Now,
                //        MatchTypeID = "",
                //        Time = DateTime.Now,
                //        FieldID = "",
                //        LeagueID = "",
                //        ID = thisFixture.ID
                //    };
                //}

                //await databaseManager.SaveFixture(fixture);

                //await DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "There was an issue saving this fixture, please try again.", "Okay");
            }
        }
    }
}
