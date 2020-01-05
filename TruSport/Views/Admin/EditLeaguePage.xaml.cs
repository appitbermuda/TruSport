using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class EditLeaguePage : ContentPage
    {
        League thisLeague;

        public EditLeaguePage()
        {
            InitializeComponent();
        }

        public EditLeaguePage(League league)
        {
            thisLeague = league;
            this.BindingContext = new LeagueAdminPageViewModel(league);
            InitializeComponent();
        }

        async void SaveClicked(object sender, System.EventArgs e)
        {
            try
            {
                League league = new League();
                if (thisLeague.ID == null || thisLeague.ID == "")
                {
                    league = new League
                    {
                        Name = leagueName.Text
                    };
                }
                else
                {
                    league = new League
                    {
                        Name = leagueName.Text,
                        ID = thisLeague.ID
                    };
                }

                //await databaseManager.SaveLeague(league);

                await DisplayAlert("Success", "The league was saved successfully!", "Okay");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "There was an issue saving this league, please try again.", "Okay");
            }
        }
    }
}
