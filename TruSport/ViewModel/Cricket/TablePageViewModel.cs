using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using TruSport.Services;
using Xamarin.Essentials;
using System.Diagnostics;

namespace TruSport.ViewModels.Cricket
{
    public class TablePageViewModel : BaseViewModel
    {
        #region Fields
        private CricketLeagueTable tappedInfo; 
        private ObservableCollection<CricketLeagueTable> premierTeamCollection;
        private ObservableCollection<CricketLeagueTable> firstDivisionTeamCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        LeagueTableService leagueTableService;

        #endregion

        #region Constructor

        public TablePageViewModel()
        {
            PremierTeamCollection = new ObservableCollection<CricketLeagueTable>();
            FirstDivisionTeamCollection = new ObservableCollection<CricketLeagueTable>();
            
            leagueTableService = new LeagueTableService();
            GenerateSource();
        }

        #endregion

        #region Properties

        public ObservableCollection<CricketLeagueTable> PremierTeamCollection
        {
            get { return premierTeamCollection; }
            set { Set(ref this.premierTeamCollection, value); }
        }

        public ObservableCollection<CricketLeagueTable> FirstDivisionTeamCollection
        {
            get { return firstDivisionTeamCollection; }
            set { Set(ref this.firstDivisionTeamCollection, value); }
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

                    var premDivTeams = await leagueTableService.GetPremierLeagueCricketTables();
                    if (premDivTeams != null)
                        PremierTeamCollection = new ObservableCollection<CricketLeagueTable>(premDivTeams.OrderBy(e => e.Position));

                    var firstDivTeams = await leagueTableService.GetFirstDivisionCricketTables();
                    if (firstDivTeams != null)
                        FirstDivisionTeamCollection = new ObservableCollection<CricketLeagueTable>(firstDivTeams.OrderBy(e => e.Position));
                }
                else
                    NoConnectivity = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Table");
            }

            IsActivityIndicatorVisible = false;
        }

        #endregion

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as CricketLeagueTable;
        }
    }
}
