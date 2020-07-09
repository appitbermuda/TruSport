using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruSport.Data;
using TruSport.Services;
using TruSport.ViewModel;
using TruSport.Views;
using TruSport.Views.Cricket;
using TruSport.Views.Football;
using Xamarin.Forms;

namespace TruSport
{
    public partial class MainPage : ContentPage
    {
        FixtureService fixtureService;

        MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            mainPageViewModel = new MainPageViewModel(Navigation);
            fixtureService = new FixtureService();

            
            InitializeComponent();

            this.BindingContext = mainPageViewModel;
            //loader.Easing = Easing.Linear;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.Database != null)
            {
                var sport = await App.Database.GetDefaultSport();

                if (sport != null && !String.IsNullOrEmpty(sport.Sport))
                { 
                    if (sport.Sport.ToLower() == "cricket")
                        App.Current.MainPage = new CricketMasterDetailPage();
                    else
                        App.Current.MainPage = new FootballMasterDetailPage();
                }
            }

            //try
            //{
            //    var coaches = await App.Database.GetCoaches();
            //    if (coaches.Count == 0)
            //    {
            //        var remoteCoaches = await databaseManager.GetCoaches();

            //        await App.Database.ImportCoaches(remoteCoaches);
            //    }
            //    else
            //    {
            //        await App.Database.ClearCoaches();
            //        var remoteCoaches = await databaseManager.GetCoaches();

            //        await App.Database.ImportCoaches(remoteCoaches);
            //    }
            //}
            //catch (Exception ex)
            //{ }

            //try
            //{
            //    var fields = await App.Database.GetFields();
            //    if (fields.Count == 0)
            //    {
            //        var remoteFields = await databaseManager.GetAllFields();

            //        await App.Database.ImportFields(remoteFields);
            //    }
            //    else
            //    {
            //        await App.Database.ClearFields();
            //        var remoteFields = await databaseManager.GetAllFields();

            //        await App.Database.ImportFields(remoteFields);
            //    }
            //}
            //catch (Exception ex)
            //{ }

            //try
            //{
            //    var fixtures = await App.Database.GetFixtures();
            //    if (fixtures.Count == 0)
            //    {
            //        //var remoteFixtures = await databaseManager.GetAllFixtures();

            //        var remoteFixtures = await fixtureService.GetFixtures();

            //        await App.Database.ImportFixtures(remoteFixtures);
            //    }
            //    else
            //    {
            //        await App.Database.ClearFixtures();
            //        //var remoteFixtures = await databaseManager.GetAllFixtures();
            //        var remoteFixtures = await fixtureService.GetFixtures();

            //        await App.Database.ImportFixtures(remoteFixtures);
            //    }
            //}
            //catch (Exception ex)
            //{ }

            ////try
            ////{
            ////    var matches = await App.Database.GetMatches();
            ////    if (matches.Count == 0)
            ////    {
            ////        var remoteMatches = await databaseManager.GetMatches();

            ////        await App.Database.ImportMatches(remoteMatches);
            ////    }
            ////    else
            ////    {
            ////        await App.Database.ClearMatches();
            ////        var remoteMatches = await databaseManager.GetMatches();

            ////        await App.Database.ImportMatches(remoteMatches);
            ////    }
            ////}
            ////catch (Exception ex)
            ////{ }

            //try
            //{
            //    var leagues = await App.Database.GetLeagues();
            //    if (leagues.Count == 0)
            //    {
            //        var remoteLeagues = await databaseManager.GetAllLeagues();

            //        await App.Database.ImportLeagues(remoteLeagues);
            //    }
            //    else
            //    {
            //        await App.Database.ClearLeagues();
            //        var remoteLeagues = await databaseManager.GetAllLeagues();

            //        await App.Database.ImportLeagues(remoteLeagues);
            //    }
            //}
            //catch (Exception ex)
            //{ }

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
            //    var teams = await App.Database.GetTeams();
            //    if (teams.Count == 0)
            //    {
            //        var remoteTeams = await databaseManager.GetTeams();

            //        await App.Database.ImportTeams(remoteTeams);
            //    }
            //    else
            //    {
            //        await App.Database.ClearTeams();
            //        var remoteTeams = await databaseManager.GetTeams();

            //        await App.Database.ImportTeams(remoteTeams);
            //    }
            //}
            //catch (Exception ex)
            //{ }

            //try
            //{
            //    var matchType = await App.Database.GetMatchTypes();
            //    if (matchType.Count == 0)
            //    {
            //        var remoteMatchTypes = await databaseManager.GetAllMatchTypes();

            //        await App.Database.ImportMatchTypes(remoteMatchTypes);
            //    }
            //    else
            //    {
            //        await App.Database.ClearMatchTypes();
            //        var remoteMatchTypes = await databaseManager.GetAllMatchTypes();

            //        await App.Database.ImportMatchTypes(remoteMatchTypes);
            //    }
            //}
            //catch (Exception ex)
            //{ }

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

            ////App.Current.MainPage = new FootballMainPage();
            //App.Current.MainPage = new FootballMasterDetailPage();
        }
    }
}
