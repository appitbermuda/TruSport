using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using TruSport.Services;
using Xamarin.Essentials;
using System.Diagnostics;

namespace TruSport.ViewModels.Cricket
{
    public class CompetitionsPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<League> leagueCollection;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        FixtureService fixtureService;
        LeagueTableService leagueTableService;
        TransferService transferService;
        LeagueService leagueService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public CompetitionsPageViewModel()
        {
            leagueCollection = new ObservableCollection<League>();
            fixtureService = new FixtureService();
            leagueTableService = new LeagueTableService();
            leagueService = new LeagueService();

            GenerateSource();
        }

        #endregion

        #region Properties

        public ObservableCollection<League> LeagueCollection
        {
            get { return leagueCollection; }
            set { Set(ref leagueCollection, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }
        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    var leagues = await leagueService.GetCricketLeagues();
                    leagues = leagues.OrderBy(e => e.Name).ToList();

                    if(leagues != null)
                        LeagueCollection = new ObservableCollection<League>(leagues);
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Competitions");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        #endregion


        #region Player Info

        Fixture[] Player = new Fixture[]
         {
            
         };

        #endregion
    }
}
