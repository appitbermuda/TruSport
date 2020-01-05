using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using TruSport.Data;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class RosterViewModel
    {
        
        MatchViewModel MatchViewModel;

        public RosterViewModel(List<SelectableData<MatchRosterListView>> rosterList)
        {
            

            //MessagingCenter.Unsubscribe<RosterViewModel, List<SelectableData<MatchRosterListView>>>(this, MessengerKeys.SetTeamLineUp);
            //MessagingCenter.Subscribe<RosterViewModel, List<SelectableData<MatchRosterListView>>>(this, MessengerKeys.SetTeamLineUp, async (sender, arg) =>
            //{

            //    RosterList = arg;
            //});

            RosterList = rosterList;

            //if(RosterList == null)
            //{
            //    List<MatchRosterListView> matchRoster = databaseManager.SetMatchRosterByMatchByTeam(matchID, TeamID);

            //        var list = new List<SelectableData<MatchRosterListView>>();

            //        foreach (var item in matchRoster)
            //            list.Add(new SelectableData<MatchRosterListView>() { Data = item });

            //        RosterList = list;
            //}

            //if (HomeRosterList == null && AwayRosterList == null)
            //{
            //    List<MatchRosterListView> matchRoster = databaseManager.SetMatchRosterByMatchByTeam(matchID, TeamID);

            //    var list = new List<SelectableData<MatchRosterListView>>();

            //    foreach (var item in matchRoster)
            //        list.Add(new SelectableData<MatchRosterListView>() { Data = item });

            //    RosterList = list;
            //}
            //else if(HomeRosterList == null && AwayRosterList != null)
            //{
            //    var thisAwayList = AwayRosterList.Where(e => e.Data.TeamID == TeamID && e.Data.MatchID == matchID);
            //    if(thisAwayList == null)
            //    {
            //        List<MatchRosterListView> matchRoster = databaseManager.SetMatchRosterByMatchByTeam(matchID, TeamID);

            //        var list = new List<SelectableData<MatchRosterListView>>();

            //        foreach (var item in matchRoster)
            //            list.Add(new SelectableData<MatchRosterListView>() { Data = item });

            //        RosterList = list;
            //    }
            //    else
            //    {
            //        RosterList = thisAwayList.ToList();
            //    }

            //}
            //else if (HomeRosterList != null && AwayRosterList == null)
            //{
            //    var thisHomeList = HomeRosterList.Where(e => e.Data.TeamID == TeamID && e.Data.MatchID == matchID);
            //    if (thisHomeList == null)
            //    {
            //        List<MatchRosterListView> matchRoster = databaseManager.SetMatchRosterByMatchByTeam(matchID, TeamID);

            //        var list = new List<SelectableData<MatchRosterListView>>();

            //        foreach (var item in matchRoster)
            //            list.Add(new SelectableData<MatchRosterListView>() { Data = item });

            //        RosterList = list;
            //    }
            //    else
            //    {
            //        RosterList = thisHomeList.ToList();
            //    }
            //}
            //else
            //{
            //    var thisHomeList = HomeRosterList.Where(e => e.Data.TeamID == TeamID && e.Data.MatchID == matchID);
            //    var thisAwayList = AwayRosterList.Where(e => e.Data.TeamID == TeamID && e.Data.MatchID == matchID);
            //    if (thisHomeList != null)
            //    {
            //        RosterList = thisHomeList.ToList();
            //    }
            //    else if (thisAwayList != null)
            //    {
            //        RosterList = thisAwayList.ToList();
            //    }
            //}


        }

        // As example if you need to convert
        //private void LoadData(List<MatchRosterListView> data)
        //{
        //  var list = new List<SelectableData<MatchRosterListView>>();

        //  foreach (var item in data)
        //      list.Add(new SelectableData<MatchRosterListView>() { Data = item });

        //  RosterList = list;
        //}

        public List<SelectableData<MatchRosterListView>> RosterList { get; set; }
        public List<SelectableData<MatchRosterListView>> HomeRosterList { get; set; }
        public List<SelectableData<MatchRosterListView>> AwayRosterList { get; set; }

        public List<SelectableData<MatchRosterListView>> GetNewHomeTeamData()
        {
            var list = new List<SelectableData<MatchRosterListView>>();

            foreach (var data in RosterList)
                list.Add(new SelectableData<MatchRosterListView>() { Data = data.Data.Clone(), StarterSelected = data.StarterSelected, SubSelected = data.SubSelected });

            return list;
        }

        public List<SelectableData<MatchRosterListView>> GetNewAwayTeamData()
        {
            var list = new List<SelectableData<MatchRosterListView>>();

            foreach (var data in RosterList)
                list.Add(new SelectableData<MatchRosterListView>() { Data = data.Data.Clone(), StarterSelected = data.StarterSelected, SubSelected = data.SubSelected });

            return list;
        }

        public ICommand FinishHomeTeamCommand
        {
            get
            {
                return new Command(async () =>
                {
                    HomeRosterList = GetNewHomeTeamData();
                    MessagingCenter.Send(this,MessengerKeys.SetHomeTeamLineUp, GetNewHomeTeamData());

                    //MatchViewModel.HomeSelectedData = GetNewHomeTeamData();
                    //MatchViewModel.AwaySelectedData = GetNewAwayTeamData();
                    await PopupNavigation.Instance.PopAsync();
                });
            }

        }

        public ICommand FinishAwayTeamCommand
        {
            get
            {
                return new Command(async () =>
                {
                    AwayRosterList = GetNewAwayTeamData();
                    MessagingCenter.Send(this, MessengerKeys.SetAwayTeamLineUp, GetNewAwayTeamData());

                    //MatchViewModel.HomeSelectedData = GetNewHomeTeamData();
                    //MatchViewModel.AwaySelectedData = GetNewAwayTeamData();
                    await PopupNavigation.Instance.PopAsync();
                });
            }

        }
    }
}
