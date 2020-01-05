using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TruSport.Services;
using TruSport.Views.Football;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class OnTrackPage : ContentPage
    {
        CoachService coachService;
        FieldService fieldService;
        LeagueService leagueService;
        LeagueTableService leagueTableService;
        TeamService teamService;
        MatchTypeService matchTypeService;
        FixtureService fixtureService;
        //Connectivity connectivity;

        public OnTrackPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            //connectivity = new Connectivity;
            coachService = new CoachService();
            fieldService = new FieldService();
            leagueService = new LeagueService();
            leagueTableService = new LeagueTableService();
            teamService = new TeamService();
            matchTypeService = new MatchTypeService();
            fixtureService = new FixtureService();

            InitializeComponent();

            //loader.Easing = Easing.Linear;
        }

        
        protected override async void OnAppearing()
        {
            bool NoData = false;
            //Analytics.TrackEvent("Data loading");
            //base.OnAppearing();

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {


                //try
                //{
                //    Analytics.TrackEvent("coaches clear");
                //    await App.Database.ClearCoaches();
                //    Analytics.TrackEvent("coaches cleared");
                //    var coaches = await App.Database.GetCoaches();
                //    Analytics.TrackEvent("coaches loading");
                //    if (coaches.Count == 0)
                //    {
                //        Analytics.TrackEvent("coaches remote");
                //        //var remoteCoaches = await databaseManager.GetCoaches();
                //        var remoteCoaches = await coachService.GetCoaches();
                //        Analytics.TrackEvent("coaches loaded");
                //        await App.Database.ImportCoaches(remoteCoaches);
                //        Analytics.TrackEvent("coaches imported");
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Crashes.TrackError(ex);
                //    await DisplayAlert("Somethings Wrong", "Looks like something is going wrong, contact us if this persists.", "Okay");
                //}


                //try
                //{
                //    await App.Database.ClearFields();
                //    var fields = await App.Database.GetFields();
                //    if (fields.Count == 0)
                //    {
                //        //var remoteFields = await databaseManager.GetAllFields();
                //        var remoteFields = await fieldService.GetFields();

                //        await App.Database.ImportFields(remoteFields);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    await DisplayAlert("Somethings Wrong", "Looks like something is going wrong, contact us if this persists.", "Okay");
                //}

                //try
                //{
                //    await App.Database.ClearFixtures();
                //    var fixtures = await App.Database.GetFixtures();
                //    if (fixtures.Count == 0)
                //    {
                //        //var remoteFixtures = await databaseManager.GetAllFixtures();

                //        var remoteFixtures = await fixtureService.GetFixtures();

                //        await App.Database.ImportFixtures(remoteFixtures);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    await DisplayAlert("Somethings Wrong", "Looks like something is going wrong, contact us if this persists.", "Okay");
                //}

                //try
                //{
                //    var matches = await App.Database.GetMatches();
                //    if (matches.Count == 0)
                //    {
                //        var remoteMatches = await databaseManager.GetMatches();

                //        await App.Database.ImportMatches(remoteMatches);
                //    }
                //    else
                //    {
                //        await App.Database.ClearMatches();
                //        var remoteMatches = await databaseManager.GetMatches();

                //        await App.Database.ImportMatches(remoteMatches);
                //    }
                //}
                //catch (Exception ex)
                //{ }

                try
                {
                    await App.Database.ClearLeagues();
                    var leagues = await App.Database.GetLeagues();
                    if (leagues.Count == 0)
                    {
                        //var remoteLeagues = await databaseManager.GetAllLeagues();
                        var remoteLeagues = await leagueService.GetLeagues();

                        await App.Database.ImportLeagues(remoteLeagues);
                    }
                }
                catch (Exception ex)
                {
                    NoData = true;
                }

            //try
            //{
            //    var players = await App.Database.GetPlayers();
            //    if (players.Count == 0)
            //    {
            //        var remotePlayers = await databaseManager.GetPlayers();

            //        await App.Database.ImportPlayers(remotePlayers);
            //    }
            //    else
            //    {
            //        await App.Database.ClearPlayers();
            //        var remotePlayers = await databaseManager.GetPlayers();

            //        await App.Database.ImportPlayers(remotePlayers);
            //    }
            //}
            //catch (Exception ex)
            //{ }
            
            //try
            //{
            //    await App.Database.ClearTeams();
            //    var teams = await App.Database.GetTeams();
            //    if (teams.Count == 0)
            //    {
            //        //var remoteTeams = await databaseManager.GetTeams();
            //        var remoteTeams = await teamService.GetTeams();

            //        await App.Database.ImportTeams(remoteTeams);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Somethings Wrong", "Looks like something is going wrong, contact us if this persists.", "Okay");
            //}

            
            //try
            //{
            //    await App.Database.ClearMatchTypes();
            //    var matchType = await App.Database.GetMatchTypes();
            //    if (matchType.Count == 0)
            //    {
            //        //var remoteMatchTypes = await databaseManager.GetAllMatchTypes();
            //        var remoteMatchTypes = await matchTypeService.GetMatchTypes();

            //        await App.Database.ImportMatchTypes(remoteMatchTypes);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Somethings Wrong", "Looks like something is going wrong, contact us if this persists.", "Okay");
            //}

            //try
            //{
            //    var userTypes = await App.Database.GetUserTypes();
            //    if (userTypes.Count == 0)
            //    {
            //        var remoteUserTypes = await databaseManager.GetUserTypes();

            //        await App.Database.ImportUserTypes(remoteUserTypes);
            //    }
            //    else
            //    {
            //        await App.Database.ClearUserTypes();
            //        var remoteUserTypes = await databaseManager.GetUserTypes();

            //        await App.Database.ImportUserTypes(remoteUserTypes);
            //    }
            //}
            //catch (Exception ex)
            //{ }

                

            }

            try
            {
                    
                App.Current.MainPage = new FootballMasterDetailPage();
                //App.Current.MainPage = new FootballMasterDetailPage();
                //App.Current.MainPage = new NewsPage();
            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
            }
            
        }
    }
}
