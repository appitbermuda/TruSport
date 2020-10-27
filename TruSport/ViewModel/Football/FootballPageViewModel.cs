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
using TruSport.Views.Football;
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

namespace TruSport.ViewModels
{
    public class FootballPageViewModel : BaseViewModel
    {
        #region Fields

        public CalendarEventCollection calendarInlineEvents;
        private int selectedIndex;
        private int fixtureHeaderCount;
        private ObservableCollection<Award> awardCollection;
        private ObservableCollection<Fixture> pastCollection;
        private ObservableCollection<Fixture> upcomingCollection;
        private ObservableCollection<LiveFixture> liveCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onFixtureSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onLiveFixtureSelectedCommand;
        private Command<object> leagueSelectedCommand;
        private Command calendarVisibilityClickedCommand;
        private Command<object> refreshPastFixturesCommand;
        private Command<object> refreshUpcomingFixturesCommand;
        private Command<object> refreshLiveFixturesCommand;
        private bool showPOW;
        private bool refreshActive;
        private bool noUpcomingFixtures;
        private bool noLiveFixtures;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool isFavouriteVisible;
        private bool noConnectivity;
        private bool cancelFixtureRefresh;
        private bool isUpcomingCalendarVisible;
        private bool showSport;
        private bool isPreviousVisible;
        private bool isLiveVisible;
        private bool _isFootball;
        private bool _isCricket;
        private bool hasAwards;
        private string selectedSport;
        private DateTime _minDate;
        Sport _sport;

        FixtureService fixtureService;
        AwardService awardService;

        INavigation Navigation;

        #endregion

        #region Constructor

        public FootballPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            PastCollection = new ObservableCollection<Fixture>();
            UpcomingCollection = new ObservableCollection<Fixture>();
            LiveCollection = new ObservableCollection<LiveFixture>();
            CalendarInlineEvents = new CalendarEventCollection();

            fixtureService = new FixtureService();
            awardService = new AwardService();

            SelectedIndex = 0;

            GenerateSource();

            CalendarCellTapped = new Command<CalendarTappedEventArgs>(CellTapped);

            RefreshPastFixturesCommand = new Command<object>(async (obj) => await RefreshPastFixtures());
            RefreshUpcomingFixturesCommand = new Command<object>(async (obj) => await RefreshUpcomingFixtures());
            RefreshLiveFixturesCommand = new Command<object>(async (obj) => await RefreshLiveFixtures());

            OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
            OnLiveFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(LiveFixtureSelected);
            LeagueSelectedCommand = new Command<object>(SelectedLeague);
            CalendarVisibilityClickedCommand = new Command(CalendarVisibilityClicked);
            SelectSportCommand = new Command(SelectSport);
            SelectedSportCommand = new Command<string>(SportSelected);

            MessagingCenter.Subscribe<string>("Fixtures", "RefreshFixtures", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("Fixtures", "RefreshFixtures");
                RefreshFixturesTimer();

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

        public Command SelectSportCommand { get; }
        public Command<string> SelectedSportCommand { get; }

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

        public Command<object> RefreshLiveFixturesCommand
        {
            get { return refreshLiveFixturesCommand; }
            set { refreshLiveFixturesCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnFixtureSelectedCommand
        {
            get { return onFixtureSelectedCommand; }
            set { onFixtureSelectedCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnLiveFixtureSelectedCommand
        {
            get { return onLiveFixtureSelectedCommand; }
            set { onLiveFixtureSelectedCommand = value; }
        }

        public Sport Sport
        {
            get { return _sport; }
            set { Set(ref _sport, value); }
        }


        public bool HasAwards
        {
            get { return hasAwards; }
            set { Set(ref hasAwards, value); }
        }

        public ObservableCollection<Fixture> PastCollection
        {
            get { return pastCollection; }
            set { Set(ref pastCollection, value); }
        }

        public ObservableCollection<Fixture> UpcomingCollection
        {
            get { return upcomingCollection; }
            set { Set(ref upcomingCollection, value); }
        }

        public ObservableCollection<LiveFixture> LiveCollection
        {
            get { return liveCollection; }
            set { Set(ref liveCollection, value); }
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

        public bool NoLiveFixtures
        {
            get { return noLiveFixtures; }
            set { Set(ref noLiveFixtures, value); }
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

        public ObservableCollection<Award> AwardCollection
        {
            get { return awardCollection; }
            set { Set(ref awardCollection, value); }
        }

        public DateTime MinDate
        {
            get { return _minDate; }
            set { Set(ref _minDate, value); }
        }

        public string SelectedSport
        {
            get { return selectedSport; }
            set { Set(ref selectedSport, value); }
        }

        public bool ShowSport
        {
            get { return showSport; }
            set { Set(ref showSport, value); }
        }

        public bool ShowPOW
        {
            get { return showPOW; }
            set { Set(ref showPOW, value); }
        }

        public bool IsLiveVisible
        {
            get { return isLiveVisible; }
            set { Set(ref isLiveVisible, value); }
        }

        public bool IsPreviousVisible
        {
            get { return isPreviousVisible; }
            set { Set(ref isPreviousVisible, value); }
        }

        public bool IsCricket
        {
            get { return _isCricket; }
            set { Set(ref _isCricket, value); }
        }

        public bool IsFootball
        {
            get { return _isFootball; }
            set { Set(ref _isFootball, value); }
        }

        public int FixtureHeaderCount
        {
            get { return fixtureHeaderCount; }
            set { Set(ref fixtureHeaderCount, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            CancelFixtureRefresh = true;

            IsActivityIndicatorVisible = true;

            try
            {
                //SelectedSport = await SecureStorage.GetAsync("Sport");
                var awards = await awardService.GetFootballPlayerOfTheWeek();

                var _showPOW = await SecureStorage.GetAsync("ShowPOW");
                ShowPOW = ((_showPOW != null ? Convert.ToBoolean(_showPOW) : true) && awards != null);

                IsFootball = true;

                if (awards != null && awards.Count > 0)
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

                    var pastFixtures = await fixtureService.GetPastFootballFixtures();
                    if (pastFixtures != null)
                    {
                        //var pastFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) < DateTime.Now);
                        if (pastFixtures.Count() > 0)
                        {
                            FixtureHeaderCount++;
                            IsPreviousVisible = true;
                            PastCollection = new ObservableCollection<Fixture>(pastFixtures.OrderByDescending(e => e.FixtureTime));
                        }
                        else
                        {
                            IsPreviousVisible = false;
                        }
                    }

                    var upcomingFixtures = await fixtureService.GetUpcomingFootballFixtures();
                    if (upcomingFixtures != null)
                    {
                        //var upcomingFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) >= DateTime.Now);
                        UpcomingCollection = new ObservableCollection<Fixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));

                        if (upcomingFixtures.Count() > 0)
                        {
                            NoUpcomingFixtures = false;
                            SelectedIndex = 1;

                            foreach (var fixture in UpcomingCollection)
                            {
                                CalendarInlineEvent event1 = new CalendarInlineEvent();
                                event1.StartTime = fixture.FixtureTime;
                                event1.EndTime = event1.StartTime.AddHours(2);
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
                    FixtureHeaderCount++;

                    var liveFixtures = await fixtureService.GetLiveFootballFixtures();
                     if (liveFixtures != null)
                    { 
                        //var liveFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now &&
                            //e.FixtureTime.AddMinutes(110) >= DateTime.Now && !e.IsPostponed);

                        if (liveFixtures.Count() > 0)
                        {
                            RefreshActive = true;
                            NoLiveFixtures = false;
                            FixtureHeaderCount++;

                            Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                            {
                                if (liveFixtures.Count() > 0)
                                {
                                    RefreshActive = true;
                                    Device.BeginInvokeOnMainThread(async () => await RefreshFixtures());
                                }
                                else
                                {
                                    RefreshActive = false;
                                    NoLiveFixtures = true;
                                    return false;
                                }

                                return true;
                            });

                            LiveCollection = new ObservableCollection<LiveFixture>(liveFixtures);
                        }
                        else
                        {
                            RefreshActive = false;

                            NoLiveFixtures = true;
                        }
                    }
                }
                else
                    NoConnectivity = true;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
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

                    var pastFixtures = await fixtureService.GetPastFootballFixtures();

                    if (pastFixtures != null)
                    {
                        PastCollection = new ObservableCollection<Fixture>(pastFixtures);
                    }
                    //var upcomingFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) >= DateTime.Now);
                    var upcomingFixtures = await fixtureService.GetUpcomingFootballFixtures();
                    if (upcomingFixtures != null)
                    {
                        UpcomingCollection = new ObservableCollection<Fixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));
                    }

                    var liveFixtures = await fixtureService.GetLiveFootballFixtures();

                    if (liveFixtures != null)
                    {

                        //var liveFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now &&
                        //e.FixtureTime.AddMinutes(110) >= DateTime.Now && !e.IsPostponed);

                        if (liveFixtures.Count() > 0 && FixtureHeaderCount < 3)
                            FixtureHeaderCount++;

                        LiveCollection = new ObservableCollection<LiveFixture>(liveFixtures);
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
                var fixtures = await fixtureService.GetPastFootballFixtures();

                if (fixtures != null)
                {
                    var pastFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) <= DateTime.Now);
                    PastCollection = new ObservableCollection<Fixture>(pastFixtures);
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
                var fixtures = await fixtureService.GetUpcomingFootballFixtures();

                if (fixtures != null)
                {
                    var upcomingFixtures = fixtures.Where(e => e.FixtureTime.AddMinutes(110) >= DateTime.Now);

                    UpcomingCollection = new ObservableCollection<Fixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));


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

        internal async Task RefreshFixturesTimer()
        {
            try
            {
                if (!RefreshActive)
                {
                    var fixtures = await fixtureService.GetFootballFixtures();

                    Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                    {
                        var liveFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now &&
                            e.FixtureTime.AddMinutes(110) >= DateTime.Now);

                        if (liveFixtures.Count() > 0)
                        {
                            RefreshActive = true;
                            Device.BeginInvokeOnMainThread(async () => await RefreshFixtures());
                        }
                        else
                        {
                            RefreshActive = false;
                            NoLiveFixtures = true;
                            return false;
                        }

                        return true;
                    });
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshFixturesTimer");
            }
        }

        internal async Task RefreshLiveFixtures()
        {
            try
            {
                var liveFixtures = await fixtureService.GetLiveFootballFixtures();

                NoLiveFixtures = liveFixtures.Count > 0;

                LiveCollection = new ObservableCollection<LiveFixture>(liveFixtures);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshLiveFixtures");
            }
        }

        private async void CalendarVisibilityClicked()
        {
            IsUpcomingCalendarVisible = !IsUpcomingCalendarVisible;
        }

        private async void SelectSport()
        {
            ShowSport = !ShowSport;
        }

        private async void SportSelected(string sport)
        {
            try
            {
                if (sport != SelectedSport)
                {
                    //await SecureStorage.SetAsync("Sport", sport);
                    Application.Current.MainPage = new FootballMasterDetailPage();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport Selected");
            }

        }

        private async void SelectedLeague(object obj)
        {
            try
            {
                var groupResult = obj as Syncfusion.DataSource.Extensions.GroupResult;

                var items = new List<Fixture>(groupResult.Items.ToList<Fixture>());
                var data = items[0];

                //var fixture = fixtures.FirstOrDefault();

                var league = data.League;

                await Navigation.PushAsync(new CompetitionDetailsPage(league));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League Selected");
            }
        }

        //async Task Favourite()
        //{
        //    try
        //    {
        //        if (IsFavourite)
        //        {
        //            IsFavourite = false;

        //            await App.Database.DeleteFixtureFavourite(FixtureItem.ID);
        //        }
        //        else
        //        {
        //            IsFavourite = true;

        //            var favourite = new Favourite
        //            {
        //                FixtureID = FixtureItem.ID,
        //                Type = "Fixture"
        //            };

        //            await App.Database.SaveFavourite(favourite);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        IsFavourite = !IsFavourite;
        //        await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //    finally
        //    {

        //    }
        //}

        private async void FixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Fixture;

            if (item != null)
                await Navigation.PushAsync(new FixtureDetailsPage(item));
        }

        private async void LiveFixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Fixture;

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
