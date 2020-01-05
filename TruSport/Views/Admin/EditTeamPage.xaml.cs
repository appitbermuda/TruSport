using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class EditTeamPage : ContentPage
    {
        TeamListView thisTeam;
        TeamAdminPageViewModel teamAdminPageViewModel;

        public EditTeamPage()
        {
            teamAdminPageViewModel = new TeamAdminPageViewModel(Navigation);

            this.BindingContext = teamAdminPageViewModel;
            InitializeComponent();
        }

        public EditTeamPage(Team team)
        {
            //thisTeam = team;
            teamAdminPageViewModel = new TeamAdminPageViewModel(Navigation, team);

            this.BindingContext = teamAdminPageViewModel;
            InitializeComponent();
        }

        async void SaveClicked(object sender, System.EventArgs e)
        {
            try
            {

                if (teamAdminPageViewModel.SaveCommand.CanExecute(null))
                    teamAdminPageViewModel.SaveCommand.Execute(null);

                //Team team = new Team();
                //if (thisTeam.ID == null || thisTeam.ID == "")
                //{
                //    team = new Team
                //    {
                //        Alias = teamAlias.Text,
                //        Name = teamName.Text,
                //        HomeFieldID = "",
                //        LeagueID = "",
                //        TeamLogo = ""
                //    };
                //}
                //else
                //{
                //    team = new Team
                //    {
                //        Alias = teamAlias.Text,
                //        Name = teamName.Text,
                //        HomeFieldID = "",
                //        ID = thisTeam.ID,
                //        LeagueID = "",
                //        TeamLogo = ""
                //    };
                //}

                //await databaseManager.SaveTeam(team);

                //await DisplayAlert("Success", "The team was saved successfully!", "Okay");
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "There was an issue saving this team, please try again.", "Okay");
            }
        }
    }
}
