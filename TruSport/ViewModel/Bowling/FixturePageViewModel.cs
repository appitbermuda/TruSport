using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using TruSport.Services;
using System.Collections.Generic;
using TruSport.Views.Bowling;
using Syncfusion.DataSource.Extensions;
using TruSport.Extensions;
using Xamarin.Essentials;
using NodaTime;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Device = Xamarin.Forms.Device;
using System.Windows.Input;
using Syncfusion.SfCalendar.XForms;
using Newtonsoft.Json;
using TruSport.Views;
using System.Diagnostics;

namespace TruSport.ViewModels.Bowling
{
    public class FixturePageViewModel : BaseViewModel
    {
        #region Fields
        
        public CalendarEventCollection calendarInlineEvents;
        private int selectedIndex;
        private ObservableCollection<Award> awardCollection;
        private ObservableCollection<BowlingFixture> pastCollection;
        private ObservableCollection<BowlingFixture> upcomingCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onFixtureSelectedCommand;
        private Command<object> leagueSelectedCommand;
        private Command calendarVisibilityClickedCommand;
        private Command<object> refreshPastFixturesCommand;
        private Command<object> refreshUpcomingFixturesCommand;
        private bool showPOW;
        private bool refreshActive;
        private bool noUpcomingFixtures;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool isFavouriteVisible;
        private bool noConnectivity;
        private bool cancelFixtureRefresh;
        private bool isUpcomingCalendarVisible;
        private bool showSport;
        private bool hasAwards;
        private bool isPreviousVisible;
        private DateTime _minDate;
        Sport _sport;

        FixtureService fixtureService;
        AwardService awardService;

        INavigation Navigation;

        #endregion

        #region Constructor

        public FixturePageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            PastCollection = new ObservableCollection<BowlingFixture>();
            UpcomingCollection = new ObservableCollection<BowlingFixture>();
            CalendarInlineEvents = new CalendarEventCollection();

            fixtureService = new FixtureService();
            awardService = new AwardService();

            SelectedIndex = 0;

            GenerateSource();

            CalendarCellTapped = new Command<CalendarTappedEventArgs>(CellTapped);

            RefreshPastFixturesCommand = new Command<object>(async (obj) => await RefreshPastFixtures());
            RefreshUpcomingFixturesCommand = new Command<object>(async (obj) => await RefreshUpcomingFixtures());

            OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
            LeagueSelectedCommand = new Command<object>(SelectedLeague);
            CalendarVisibilityClickedCommand = new Command(CalendarVisibilityClicked);

            MessagingCenter.Subscribe<string>("Fixtures", "RefreshFixtures", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("Fixtures", "RefreshFixtures");
                await RefreshFixtures();

            });
        }

        #endregion

        #region Properties

        public CalendarEventCollection CalendarInlineEvents
        {
            get { return calendarInlineEvents; }
            set { Set(ref calendarInlineEvents, value); }
        }

        public ICommand CalendarCellTapped { get; set; }
        public Command<object> LeagueSelectedCommand
        {
            get { return leagueSelectedCommand; }
            set { leagueSelectedCommand = value; }
        }

        public Command CalendarVisibilityClickedCommand
        {
            get { return calendarVisibilityClickedCommand; }
            set { calendarVisibilityClickedCommand = value; }
        }

        public Command<object> RefreshPastFixturesCommand
        {
            get { return refreshPastFixturesCommand; }
            set { refreshPastFixturesCommand = value; }
        }

        public Command<object> RefreshUpcomingFixturesCommand
        {
            get { return refreshUpcomingFixturesCommand; }
            set { refreshUpcomingFixturesCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnFixtureSelectedCommand
        {
            get { return onFixtureSelectedCommand; }
            set { onFixtureSelectedCommand = value; }
        }

        public Sport Sport
        {
            get { return _sport; }
            set { Set(ref _sport, value); }
        }

        public ObservableCollection<Award> AwardCollection
        {
            get { return awardCollection; }
            set { Set(ref awardCollection, value); }
        }

        public ObservableCollection<BowlingFixture> PastCollection
        {
            get { return pastCollection; }
            set { Set(ref pastCollection, value); }
        }

        public ObservableCollection<BowlingFixture> UpcomingCollection
        {
            get { return upcomingCollection; }
            set { Set(ref upcomingCollection, value); }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { Set(ref selectedIndex, value); }
        }

        public bool RefreshActive
        {
            get { return refreshActive; }
            set { Set(ref refreshActive, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool IsFavourite
        {
            get { return isFavourite; }
            set { Set(ref isFavourite, value); }
        }

        public bool IsFavouriteVisible
        {
            get { return isFavouriteVisible; }
            set { Set(ref isFavouriteVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool NoUpcomingFixtures
        {
            get { return noUpcomingFixtures; }
            set { Set(ref noUpcomingFixtures, value); }
        }

        public bool CancelFixtureRefresh
        {
            get { return cancelFixtureRefresh; }
            set { Set(ref cancelFixtureRefresh, value); }
        }

        public bool IsUpcomingCalendarVisible
        {
            get { return isUpcomingCalendarVisible; }
            set { Set(ref isUpcomingCalendarVisible, value); }
        }

        public DateTime MinDate
        {
            get { return _minDate; }
            set { Set(ref _minDate, value); }
        }

        public bool ShowSport
        {
            get { return showSport; }
            set { Set(ref showSport, value); }
        }

        public bool HasAwards
        {
            get { return hasAwards; }
            set { Set(ref hasAwards, value); }
        }

        public bool ShowPOW
        {
            get { return showPOW; }
            set { Set(ref showPOW, value); }
        }

        public bool IsPreviousVisible
        {
            get { return isPreviousVisible; }
            set { Set(ref isPreviousVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            CancelFixtureRefresh = true;
            HasAwards = false;
            IsActivityIndicatorVisible = true;

            try
            {
                var awards = await awardService.GetBowlingPlayerOfTheWeek();

                var _showPOW = await SecureStorage.GetAsync("ShowPOW");
                ShowPOW = ((_showPOW != null ? Convert.ToBoolean(_showPOW) : true) && awards != null);

                if(awards != null)
                {
                    HasAwards = true;
                    AwardCollection = new ObservableCollection<Award>(awards);
                }

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;
                    IsUpcomingCalendarVisible = false;

                    MinDate = DateTime.Now.Date;

                    //var pastFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) < DateTime.Now).ToList();
                    var pastFixtures = await fixtureService.GetPastBowlingFixtures();

                    if (pastFixtures != null)
                    {
                        //var pastFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) < DateTime.Now);
                        if (pastFixtures.Count() > 0)
                        {
                            IsPreviousVisible = true;
                            PastCollection = new ObservableCollection<BowlingFixture>(pastFixtures.OrderByDescending(e => e.FixtureTime));
                        }
                        else
                        {
                            IsPreviousVisible = false;
                        }
                    }

                    //var upcomingFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) >= DateTime.Now).ToList();
                    var upcomingFixtures = await fixtureService.GetUpcomingBowlingFixtures();
                    if (upcomingFixtures != null)
                    {
                        //var upcomingFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) >= DateTime.Now);
                        UpcomingCollection = new ObservableCollection<BowlingFixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));

                        if (upcomingFixtures.Count() > 0)
                        {
                            NoUpcomingFixtures = false;
                            SelectedIndex = 1;

                            foreach (var fixture in UpcomingCollection)
                            {
                                CalendarInlineEvent event1 = new CalendarInlineEvent();
                                event1.StartTime = fixture.FixtureTime;
                                event1.EndTime = event1.StartTime.AddHours(8);
                                event1.Subject = string.Format("{0} v {1}", fixture.HomeTeam.Name, fixture.AwayTeam.Name);
                                event1.Color = (Color)App.Current.Resources["primaryDarkBlueTwo"];

                                CalendarInlineEvents.Add(event1);
                            }
                        }
                        else
                        {
                            NoUpcomingFixtures = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                NoConnectivity = true;
            }

            IsActivityIndicatorVisible = false;
        }

        internal async Task RefreshFixtures()
        {
            try
            { 
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    var fixtures = await fixtureService.GetBowlingFixtures();

                    if (fixtures != null)
                    {
                        //var pastFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) <= DateTime.Now);
                        var pastFixtures = await fixtureService.GetPastBowlingFixtures();
                        if (pastFixtures != null)
                        {
                            PastCollection = new ObservableCollection<BowlingFixture>(pastFixtures);
                        }

                        //var upcomingFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) >= DateTime.Now);
                        var upcomingFixtures = await fixtureService.GetUpcomingBowlingFixtures();
                        if (upcomingFixtures != null)
                        {
                            UpcomingCollection = new ObservableCollection<BowlingFixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));
                        }

                    }
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshFixtures");
            }
        }


        internal async Task RefreshPastFixtures()
        {
            try
            {
                var fixtures = await fixtureService.GetPastBowlingFixtures();

                if (fixtures != null)
                {
                    var pastFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) <= DateTime.Now);
                    PastCollection = new ObservableCollection<BowlingFixture>(pastFixtures);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshPastFixtures");
            }
        }

        internal async Task RefreshUpcomingFixtures()
        {
            try
            {
                var fixtures = await fixtureService.GetUpcomingBowlingFixtures();

                if (fixtures != null)
                {
                    var upcomingFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) >= DateTime.Now);

                    UpcomingCollection = new ObservableCollection<BowlingFixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));


                    if (upcomingFixtures.Count() > 0)
                    {
                        NoUpcomingFixtures = false;
                        SelectedIndex = 1;
                    }
                    else
                        NoUpcomingFixtures = true;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshUpcomingFixtures");
            }
        }

        private async void CalendarVisibilityClicked()
        {
            IsUpcomingCalendarVisible = !IsUpcomingCalendarVisible;
        }

        private async void SelectedLeague(object obj)
        {
            try
            {
                var groupResult = obj as Syncfusion.DataSource.Extensions.GroupResult;

                //var items = new List<BowlingFixture>(groupResult.Items.ToList<BowlingFixture>());
                //var data = items[0];

                ////var fixture = fixtures.FirstOrDefault();

                //var league = data.League;

                //await Navigation.PushAsync(new CompetitionDetailsPage(league));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "SelectedLeague");
            }
        }

        private async void FixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as BowlingFixture;

            if (item != null)
                await Navigation.PushAsync(new FixtureDetailsPage(item));
        }

        private void CellTapped(CalendarTappedEventArgs obj)
        {
            var text = obj.DateTime.ToString("dd/MM/yyyy") + " " + obj.SelectedAppointment.ToString();
            IsUpcomingCalendarVisible = false;
        }

        #endregion

    }
}
