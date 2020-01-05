using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using TruSport.Data;
using TruSport.Model;
using TruSport.Views.Football;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class MatchViewModel : BaseViewModel
    {
        // This is just as an example. Do not create a static property in your ViewModel, for other ViewModels to communicate with, in a production app.
        //public static List<SelectableData<MatchRosterListView>> SelectedData { get; set; }

        public List<SelectableData<MatchRosterListView>> HomeTeamDataSource { get; set; }
        public List<SelectableData<MatchRosterListView>> AwayTeamDataSource { get; set; }
        public List<SelectableData<MatchRosterListView>> HomeSelectedData { get; set; }
        public List<SelectableData<MatchRosterListView>> AwaySelectedData { get; set; }
        public List<SelectableData<MatchRosterListView>> HomeData { get; set; }
        public List<SelectableData<MatchRosterListView>> AwayData { get; set; }
        public List<MatchRosterListView> homeMatchRosters { get; set; }
        public List<MatchRosterListView> awayMatchRosters { get; set; }
        bool _ShowHomeTeamSubmit;
        bool _ShowAwayTeamSubmit;

        public IEnumerable<Grouping<bool, SelectableData<MatchRosterListView>>> teamSheets;
        public ObservableCollection<Grouping<bool, SelectableData<MatchRosterListView>>> TeamSheetGroups { get; set; }

        public string thisMatchID;
        public string thisHomeTeamID;
        public string thisAwayTeamID;

        public MatchViewModel(string matchID, string HomeTeamID, string AwayTeamID)
        {
            

            thisMatchID = matchID;
            thisHomeTeamID = HomeTeamID;
            thisAwayTeamID = AwayTeamID;

            //// Load Data
            //homeMatchRosters = databaseManger.SetMatchRosterByMatchByTeam(matchID, HomeTeamID);
            //awayMatchRosters = databaseManger.SetMatchRosterByMatchByTeam(matchID, AwayTeamID);

            //HomeSelectedData = new List<SelectableData<MatchRosterListView>>();

            //foreach(var player in homeMatchRosters)
            //{
            //    HomeSelectedData.Add(new SelectableData<MatchRosterListView>() { Data = player });
            //}

            //AwaySelectedData = new List<SelectableData<MatchRosterListView>>();

            //foreach (var player in awayMatchRosters)
            //{
            //    AwaySelectedData.Add(new SelectableData<MatchRosterListView>() { Data = player });
            //}

            //{
            //    new SelectableData<MatchRosterListView>() { Data = new ExampleData() { Name = "Test1", Description = "Description1" } },
            //    new SelectableData<MatchRosterListView>() { Data = new ExampleData() { Name = "Test2", Description = "Description2" } },
            //    new SelectableData<MatchRosterListView>() { Data = new ExampleData() { Name = "Test3", Description = "Description3" } },
            //    new SelectableData<MatchRosterListView>() { Data = new ExampleData() { Name = "Test4", Description = "Description4" } },
            //    new SelectableData<MatchRosterListView>() { Data = new ExampleData() { Name = "Test5", Description = "Description5" } }
            //};

            MessagingCenter.Unsubscribe<RosterViewModel, List<SelectableData<MatchRosterListView>>>(this, MessengerKeys.SetHomeTeamLineUp);
            MessagingCenter.Subscribe<RosterViewModel, List<SelectableData<MatchRosterListView>>>(this, MessengerKeys.SetHomeTeamLineUp, async (sender, arg) =>
            {

                HomeSelectedData = arg;
                OnAppearing();
            });

            MessagingCenter.Unsubscribe<RosterViewModel, List<SelectableData<MatchRosterListView>>>(this, MessengerKeys.SetAwayTeamLineUp);
            MessagingCenter.Subscribe<RosterViewModel, List<SelectableData<MatchRosterListView>>>(this, MessengerKeys.SetAwayTeamLineUp, async (sender, arg) =>
            {

                AwaySelectedData = arg;
                OnAppearing();
            });
        }

        public async void OnAppearing()
        {
            // Load Data
            //homeMatchRosters = await databaseManger.SetMatchRosterByMatchByTeam(thisMatchID, thisHomeTeamID);
            //awayMatchRosters = await databaseManger.SetMatchRosterByMatchByTeam(thisMatchID, thisAwayTeamID);

            HomeSelectedData = new List<SelectableData<MatchRosterListView>>();

            foreach (var player in homeMatchRosters)
            {
                HomeSelectedData.Add(new SelectableData<MatchRosterListView>() { Data = player });
            }

            AwaySelectedData = new List<SelectableData<MatchRosterListView>>();

            foreach (var player in awayMatchRosters)
            {
                AwaySelectedData.Add(new SelectableData<MatchRosterListView>() { Data = player });
            }

            HomeTeamDataSource = HomeSelectedData.Where(x => x.StarterSelected || x.SubSelected).ToList();
            var teamSheetGroups = HomeTeamDataSource.GroupBy(e => e.StarterSelected).Select(g => new Grouping<bool, SelectableData<MatchRosterListView>> (g.Key, g));
            TeamSheetGroups = new ObservableCollection<Grouping<bool, SelectableData<MatchRosterListView>>>(teamSheetGroups);
            //FixturesList.ItemsSource = TeamSheetGroups;

            if (HomeTeamDataSource.Count > 0)
                ShowHomeTeamSubmit = true;
            else
                ShowHomeTeamSubmit = false;

            OnPropertyChanged(nameof(TeamSheetGroups));

            AwayTeamDataSource = AwaySelectedData.Where(x => x.StarterSelected || x.SubSelected).ToList();

            if (AwayTeamDataSource.Count > 0)
                ShowAwayTeamSubmit = true;
            else
                ShowAwayTeamSubmit = false;

            OnPropertyChanged(nameof(AwayTeamDataSource));
        }

        public bool ShowHomeTeamSubmit
        {
            get
            {
                return _ShowHomeTeamSubmit;
            }
            set
            {
                _ShowHomeTeamSubmit = value;
                OnPropertyChanged(nameof(ShowHomeTeamSubmit));
            }
        }

        public bool ShowAwayTeamSubmit
        {
            get
            {
                return _ShowAwayTeamSubmit;
            }
            set
            {
                _ShowAwayTeamSubmit = value;
                OnPropertyChanged(nameof(ShowAwayTeamSubmit));
            }
        }

        public ICommand SelectHomeTeamCommand
        {
            get
            {
                return new Command( async () =>
                {
                    //MessagingCenter.Send(this, MessengerKeys.SetTeamLineUp, HomeSelectedData);
                    await PopupNavigation.Instance.PushAsync(new MatchRosterPopUpPage(thisMatchID, thisHomeTeamID, true, HomeSelectedData));
                    //App.NavPage.PushAsync(new MultiSelect(SelectedData));
                });
            }

        }

        public ICommand SelectAwayTeamCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //MessagingCenter.Send(this, MessengerKeys.SetTeamLineUp, AwaySelectedData);
                    await PopupNavigation.Instance.PushAsync(new MatchRosterPopUpPage(thisMatchID, thisAwayTeamID, false, AwaySelectedData));
                    //App.NavPage.PushAsync(new MultiSelect(SelectedData));
                });
            }

        }

        public ICommand SubmitHomeTeamCommand
        {
            get
            {
                return new Command(async () =>
                {
                    foreach(var player in HomeTeamDataSource)
                    {
                        if (player.StarterSelected || player.SubSelected)
                        {
                            try
                            {
                                //await databaseManger.SetMatchTeam(
                                //new MatchRoster
                                //{
                                //    //FixtureID = player.Data.FixtureID,
                                //    //Goals = player.Data.Goals,
                                //    //JerseyNumber = player.Data.JerseyNumber,
                                //    //ID = player.Data.ID,
                                //    //PlayerID = player.Data.PlayerID,
                                //    //RedCards = player.Data.RedCards,
                                //    //TeamID = player.Data.TeamID,
                                //    //YellowCards = player.Data.YellowCards,
                                //    //IsStarter = player.StarterSelected,
                                //});
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    //await PopupNavigation.Instance.PushAsync(new MatchRosterPopUpPage(thisMatchID, thisAwayTeamID, false));
                    //App.NavPage.PushAsync(new MultiSelect(SelectedData));
                });
            }

        }

        public ICommand SubmitAwayTeamCommand
        {
            get
            {
                return new Command(async () =>
                {
                    foreach (var player in AwayTeamDataSource)
                    {
                        if (player.StarterSelected || player.SubSelected)
                        {
                            try
                            {
                                //await databaseManger.SetMatchTeam(
                                //new MatchRoster
                                //{
                                //    //FixtureID = player.Data.FixtureID,
                                //    //Goals = player.Data.Goals,
                                //    //JerseyNumber = player.Data.JerseyNumber,
                                //    //ID = player.Data.ID,
                                //    //PlayerID = player.Data.PlayerID,
                                //    //RedCards = player.Data.RedCards,
                                //    //TeamID = player.Data.TeamID,
                                //    //YellowCards = player.Data.YellowCards,
                                //    //IsStarter = player.StarterSelected,
                                //});
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    //await PopupNavigation.Instance.PushAsync(new MatchRosterPopUpPage(thisMatchID, thisAwayTeamID, false));
                    //App.NavPage.PushAsync(new MultiSelect(SelectedData));
                });
            }

        }

    }
}
