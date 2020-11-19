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
using TruSport.Views.Bowling;

namespace TruSport.ViewModels.Bowling
{
    public class TablePageViewModel : BaseViewModel
    {
        #region Fields
        private BowlingLeagueStanding tappedInfo; 
        private ObservableCollection<BowlingLeagueStanding> somersbyLeagueCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        INavigation Navigation;
        LeagueTableService leagueTableService;

        #endregion

        #region Constructor

        public TablePageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            SomersbyLeagueCollection = new ObservableCollection<BowlingLeagueStanding>();
            
            leagueTableService = new LeagueTableService();
            GenerateSource();

            TableTappedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        #endregion

        #region Properties
        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> TableTappedCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }

        public ObservableCollection<BowlingLeagueStanding> SomersbyLeagueCollection
        {
            get { return somersbyLeagueCollection; }
            set { Set(ref this.somersbyLeagueCollection, value); }
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

                    var somersbyLeague = await leagueTableService.GetSomersbyLeagueBowlingTables();
                    if (somersbyLeague != null)
                        SomersbyLeagueCollection = new ObservableCollection<BowlingLeagueStanding>(somersbyLeague.OrderBy(e => e.Position));
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
            try
            {
                tappedInfo = e.ItemData as BowlingLeagueStanding;

                await Navigation.PushModalAsync(new TableDetailPage(tappedInfo));
            }
            catch(Exception ex)
            {

            }

        }
    }
}
