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
    public class CompetitionsDetailPageViewModel : BaseViewModel
    {
        #region Fields
        private LeagueTableListView tappedInfo; 
        private ObservableCollection<League> leagueCollection;
        private ObservableCollection<CricketFixture> fixtureCollection;
        private ObservableCollection<CricketFixture> fixtureFloatCollection;
        private ObservableCollection<CricketLeagueTable> tableCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool _isActivityVisible;
        private bool isLoadMoreVisible;
        private string _title;
        private int totalCount;
        private int tabCount;
        private bool isTableExist;
        private bool noConnectivity;
        FixtureService fixtureService;
        LeagueTableService leagueTableService;
        LeagueService leagueService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public CompetitionsDetailPageViewModel(INavigation navigation, League League)
        {
            Navigation = navigation;
            Title = League.Name;
            LeagueCollection = new ObservableCollection<League>();
            FixtureCollection = new ObservableCollection<CricketFixture>();
            TableCollection = new ObservableCollection<CricketLeagueTable>();

            fixtureService = new FixtureService();
            leagueTableService = new LeagueTableService();

            GenerateSource(League.ID);

            LoadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems);
        }

        #endregion

        #region Properties
        internal SfListView PlayerCategoryList
        {
            get;
            set;
        }
        internal SfListView AgentCategoryList
        {
            get;
            set;
        }
        public Command<object> LoadMoreItemsCommand { get; set; }
        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ItemTapCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }
        public Command<object> FavoriteTapCommand
        {
            get { return favoriteTapCommand; }
            set { favoriteTapCommand = value; }
        }
        public Command<object> ResetTapCommand
        {
            get { return resetTapCommand; }
            set { resetTapCommand = value; }
        }

        public ObservableCollection<League> LeagueCollection
        {
            get { return leagueCollection; }
            set { Set(ref leagueCollection, value); }
        }

        public ObservableCollection<CricketFixture> FixtureCollection
        {
            get { return fixtureCollection; }
            set { Set(ref fixtureCollection, value); }
        }

        public ObservableCollection<CricketFixture> FixtureFloatCollection
        {
            get { return fixtureFloatCollection; }
            set { Set(ref fixtureFloatCollection, value); }
        }

        public ObservableCollection<CricketLeagueTable> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public bool IsTableExist
        {
            get { return isTableExist; }
            set { Set(ref isTableExist, value); }
        }

        public int TabCount
        {
            get { return tabCount; }
            set { Set(ref tabCount, value); }
        }

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }

        public bool IsLoadMoreVisible
        {
            get { return isLoadMoreVisible; }
            set { Set(ref isLoadMoreVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool IsActivityVisible
        {
            get { return _isActivityVisible; }
            set { Set(ref _isActivityVisible, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }
        #endregion

        #region Generate Source

        internal async void GenerateSource(string LeagueID)
        {
            try
            {
                TabCount = 0;
                IsActivityIndicatorVisible = true;
                IsLoadMoreVisible = false;

                var table = await leagueTableService.GetCricketLeagueTableByLeague(LeagueID);

                if (table != null && table.Count > 1)
                {
                    IsTableExist = true;
                    table = table.Where(e => e.Season.IsCurrent).ToList();
                    TableCollection = new ObservableCollection<CricketLeagueTable>(table);
                    TabCount++;
                }

                var fixtures = await fixtureService.GetCricketLeagueFixtures(LeagueID);

                if (fixtures != null)
                {
                    TabCount++;
                    FixtureFloatCollection = new ObservableCollection<CricketFixture>(fixtures.Where(e => e.Season.IsCurrent).OrderBy(e => e.Date).ThenBy(e => TimeSpan.Parse(e.Time)));
                    TotalCount = FixtureFloatCollection.Count;

                    if (TotalCount > 0)
                    {
                        var startIndex = FixtureFloatCollection.OrderBy(e => e.Date).IndexOf(e => e.Date > DateTime.Now);

                        if (startIndex >= 0)
                        {
                            if (startIndex != 0 && TotalCount > 50)
                            {
                                IsLoadMoreVisible = true;

                                AddFixtures(startIndex, 50);
                            }
                            else
                                AddFixtures(0, TotalCount);

                        }
                        else
                            AddFixtures(0, TotalCount);
                    }
                }

                //MessagingCenter.Send<string>("FixturesLoaded", "Refresh");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Competitions Detail");
            }
            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        private bool CanLoadMoreItems(object obj)
        {
            try
            {
                if (FixtureCollection.Count >= TotalCount)
                    return false;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private async void LoadMoreItems(object obj)
        {
            try
            {
                var listview = obj as Syncfusion.ListView.XForms.SfListView;
                listview.IsBusy = true;

                if (FixtureFloatCollection.Count > 0)
                {
                    var index = FixtureCollection.Count;
                    var count = index + 50 >= TotalCount ? TotalCount - index : 50;
                    //count = count >= TotalCount ? TotalCount - index : 50;
                    AddFixtures(index, count);
                }

                listview.IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Load More");
            }

        }

        private void AddFixtures(int index, int count)
        {
            try
            {
                count = count >= TotalCount ? TotalCount - index : 50;
                for (int i = index; i < index + count; i++)
                {
                    if (i < TotalCount)
                    {
                        var fixture = FixtureFloatCollection[i];

                        FixtureCollection.Add(fixture);
                    }
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "AddFixtures");
            }
            //IsUpNextIndicatorVisible = false;
        }

        #endregion

        public async Task<List<CricketFixture>> GetLeagueFixtures(string LeagueID)
        {

            List<CricketFixture> fixtures = new List<CricketFixture>();

            fixtures = await fixtureService.GetCricketLeagueFixtures(LeagueID);

            return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
        }

        #region Player Info

        CricketFixture[] Player = new CricketFixture[]
         {
            
         };

        #endregion
    }
}
