using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class FixtureDetailsPage : ContentPage
    {
        FixtureDetailPageViewModel fixtureDetailPageViewModel;

        public FixtureDetailsPage()
        {
            InitializeComponent();
        }

        public FixtureDetailsPage(Fixture fixture)
        {
            fixtureDetailPageViewModel = new FixtureDetailPageViewModel(Navigation, fixture);

            this.BindingContext = fixtureDetailPageViewModel;


            InitializeComponent();


        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            //    if (e.Index == 0)
            //    {
            //        SquadLabel.TextColor = Color.White;
            //        SummaryLabel.TextColor = Color.Gray;
            //        H2HLabel.TextColor = Color.Gray;
            //    }
            //    else if (e.Index == 1)
            //    {
            //        SummaryLabel.TextColor = Color.White;
            //        H2HLabel.TextColor = Color.Gray;
            //        SquadLabel.TextColor = Color.Gray;
            //    }
            //    else if (e.Index == 2)
            //    {
            //        H2HLabel.TextColor = Color.White;
            //        SummaryLabel.TextColor = Color.Gray;
            //        SquadLabel.TextColor = Color.Gray;
            //    }
        }

        //void Handle_SummarySelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        //{
        //    if (e.Index == 0)
        //    {
        //        MatchSummaryLabel.TextColor = Color.White;
        //        MatchSummaryH2HLabel.TextColor = Color.Gray;
        //    }
        //    else if (e.Index == 1)
        //    {
        //        MatchSummaryH2HLabel.TextColor = Color.Gray;
        //        MatchSummaryLabel.TextColor = Color.Gray;
        //    }
        //}

        void Handle_HeadToHeadSelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            //if (e.Index == 0)
            //{
            //    HomeTeamLabel.TextColor = Color.White;
            //    AwayTeamLabel.TextColor = Color.Gray;
            //    HeadToHeadLabel.TextColor = Color.Gray;
            //}
            //else if (e.Index == 1)
            //{
            //    HeadToHeadLabel.TextColor = Color.White;
            //    HomeTeamLabel.TextColor = Color.Gray;
            //    AwayTeamLabel.TextColor = Color.Gray;
            //}
            //else if (e.Index == 2)
            //{
            //    AwayTeamLabel.TextColor = Color.White;
            //    HomeTeamLabel.TextColor = Color.Gray;
            //    HeadToHeadLabel.TextColor = Color.Gray;
            //}
        }

        //void Handle_MatchSummaryHeadToHeadSelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        //{
        //    if (e.Index == 0)
        //    {
        //        MatchSummaryHomeTeamLabel.TextColor = Color.White;
        //        MatchSummaryAwayTeamLabel.TextColor = Color.Gray;
        //        MatchSummaryHeadToHeadLabel.TextColor = Color.Gray;
        //    }
        //    else if (e.Index == 1)
        //    {
        //        MatchSummaryHeadToHeadLabel.TextColor = Color.White;
        //        MatchSummaryHomeTeamLabel.TextColor = Color.Gray;
        //        MatchSummaryAwayTeamLabel.TextColor = Color.Gray;
        //    }
        //    else if (e.Index == 2)
        //    {
        //        MatchSummaryAwayTeamLabel.TextColor = Color.White;
        //        MatchSummaryHomeTeamLabel.TextColor = Color.Gray;
        //        MatchSummaryHeadToHeadLabel.TextColor = Color.Gray;
        //    }
        //}

        void Handle_NoRosterSelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            //    if (e.Index == 0)
            //    {
            //        NoRosterHomeTeamLabel.TextColor = Color.White;
            //        NoRosterAwayTeamLabel.TextColor = Color.Gray;
            //        NoRosterHeadToHeadLabel.TextColor = Color.Gray;
            //    }
            //    else if (e.Index == 1)
            //    {
            //        NoRosterHeadToHeadLabel.TextColor = Color.White;
            //        NoRosterHomeTeamLabel.TextColor = Color.Gray;
            //        NoRosterAwayTeamLabel.TextColor = Color.Gray;
            //    }
            //    else if (e.Index == 2)
            //    {
            //        NoRosterAwayTeamLabel.TextColor = Color.White;
            //        NoRosterHomeTeamLabel.TextColor = Color.Gray;
            //        NoRosterHeadToHeadLabel.TextColor = Color.Gray;
            //    }
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
