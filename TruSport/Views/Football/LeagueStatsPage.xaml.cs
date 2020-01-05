using System;
using System.Collections.Generic;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class LeagueStatsPage : ContentPage
    {
        LeagueStatPageViewModel leagueStatPageViewModel;
        public LeagueStatsPage()
        {
            leagueStatPageViewModel = new LeagueStatPageViewModel();

            this.BindingContext = leagueStatPageViewModel;
            InitializeComponent();
        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                PremierLabel.TextColor = Color.White;
                FirstLabel.TextColor = Color.Gray;
                CoronaLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = true;
                ////PremierLabel.TextColor = Color.Pink;
                //FirstSelected.IsVisible = false;
            }
            else if (e.Index == 1)
            {
                FirstLabel.TextColor = Color.White;
                PremierLabel.TextColor = Color.Gray;
                CoronaLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
            else if (e.Index == 2)
            {
                CoronaLabel.TextColor = Color.White;
                FirstLabel.TextColor = Color.Gray;
                PremierLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
        }

        void Handle_PremierSelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                PremierMostGoalsLabel.TextColor = Color.White;
                PremierMostGoalsScoredLabel.TextColor = Color.Gray;
                PremierMostGoalsConcededLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = true;
                ////PremierLabel.TextColor = Color.Pink;
                //FirstSelected.IsVisible = false;
            }
            else if (e.Index == 1)
            {
                PremierMostGoalsScoredLabel.TextColor = Color.White;
                PremierMostGoalsLabel.TextColor = Color.Gray;
                PremierMostGoalsConcededLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
            else if (e.Index == 2)
            {
                PremierMostGoalsConcededLabel.TextColor = Color.White;
                PremierMostGoalsLabel.TextColor = Color.Gray;
                PremierMostGoalsScoredLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
        }

        void Handle_FirstSelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                FirstMostGoalsLabel.TextColor = Color.White;
                FirstMostGoalsScoredLabel.TextColor = Color.Gray;
                FirstMostGoalsConcededLabel.TextColor = Color.Gray;
                //FirstSelected.IsVisible = true;
                ////FirstLabel.TextColor = Color.Pink;
                //FirstSelected.IsVisible = false;
            }
            else if (e.Index == 1)
            {
                FirstMostGoalsScoredLabel.TextColor = Color.White;
                FirstMostGoalsLabel.TextColor = Color.Gray;
                FirstMostGoalsConcededLabel.TextColor = Color.Gray;
                //FirstSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
            else if (e.Index == 2)
            {
                FirstMostGoalsConcededLabel.TextColor = Color.White;
                FirstMostGoalsLabel.TextColor = Color.Gray;
                FirstMostGoalsScoredLabel.TextColor = Color.Gray;
                //FirstSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
        }

        void Handle_CoronaSelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                CoronaMostGoalsLabel.TextColor = Color.White;
                CoronaMostGoalsScoredLabel.TextColor = Color.Gray;
                CoronaMostGoalsConcededLabel.TextColor = Color.Gray;
                //CoronaSelected.IsVisible = true;
                ////CoronaLabel.TextColor = Color.Pink;
                //FirstSelected.IsVisible = false;
            }
            else if (e.Index == 1)
            {
                CoronaMostGoalsScoredLabel.TextColor = Color.White;
                CoronaMostGoalsLabel.TextColor = Color.Gray;
                CoronaMostGoalsConcededLabel.TextColor = Color.Gray;
                //CoronaSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
            else if (e.Index == 2)
            {
                CoronaMostGoalsConcededLabel.TextColor = Color.White;
                CoronaMostGoalsLabel.TextColor = Color.Gray;
                CoronaMostGoalsScoredLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
        }
    }
}
