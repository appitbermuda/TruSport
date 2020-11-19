using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using TruSport.Services;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace TruSport.ViewModels.Bowling
{
    public class TeamPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Team> teamCollection;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;

        TeamService teamService;

        #endregion

        #region Constructor

        public TeamPageViewModel()
        {
            TeamCollection = new ObservableCollection<Team>();

            teamService = new TeamService();

            GenerateSource();

            TeamFavouriteCommand = new Command<object>(TeamFavourite);
        }

        #endregion

        #region Properties

        public Command<object> TeamFavouriteCommand { get; }

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref teamCollection, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            try
            {
                IsActivityIndicatorVisible = true;

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    var teamFavourites = await App.Database.GetBowlingTeamFavourites();

                    var teams = await teamService.GetBowlingTeams();

                    if(teamFavourites != null && teamFavourites.Count > 0)
                        teams.ForEach(e => e.IsFavourite = teamFavourites.Any(d => d.TeamID == e.ID));

                    TeamCollection = new ObservableCollection<Team>(teams);

                    
                }
                else
                    NoConnectivity = true;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Hmmmm", "Looks like something went wrong, please check you are connected to a wifi or cellular connection.", "Okay");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        public async void TeamFavourite(object obj)
        {
            try
            {
                var team = obj as Team;

                var item = TeamCollection.IndexOf(team);
                if (item >= 0)
                {
                    team.IsFavourite = !team.IsFavourite;
                    TeamCollection[item] = team;

                    if (!TeamCollection[item].IsFavourite)
                    {
                        await App.Database.DeleteTeamFavourite(team.ID);
                    }
                    else
                    {

                        var favourite = new Favourite
                        {
                            TeamID = team.ID,
                            Type = "Team"
                        };

                        await App.Database.SaveBowlingFavourite(favourite);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion


        #region Player Info

        TeamListView[] Player = new TeamListView[]
         {

         };

        string[] Agents = new string[]
        {
            "LOCAL",
            "INTERNATIONAL"
        };

        string[] SyncTitles = new string[]
        {
            "First Division",
            "Premier Division",
            "Corona League"
        };

        #endregion
    }
}
