using System;
using System.Collections.Generic;
using System.Diagnostics;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class StatPage : ContentPage
    {
        StatPageViewModel statPageViewModel;

        public StatPage()
        {
            statPageViewModel = new StatPageViewModel();

            this.BindingContext = statPageViewModel;
            InitializeComponent();
        }


        void PremierDivisionTap_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                PremierDivision.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabel.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];


                PremierDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabelLB.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];

                PremierLeagueList.IsVisible = true;
                FirstDivisionList.IsVisible = false;
                CoronaLeagueList.IsVisible = false;
                WomenLeagueList.IsVisible = false;

                PremierPlayerGoalsList.IsVisible = true;
                FirstPlayerGoalsList.IsVisible = false;
                CoronaPlayerGoalsList.IsVisible = false;
                WomenPlayerGoalsList.IsVisible = false;

                PremierTeamScoredList.IsVisible = true;
                FirstTeamScoredList.IsVisible = false;
                CoronaTeamScoredList.IsVisible = false;
                WomenTeamScoredList.IsVisible = false;

                PremierTeamConcededList.IsVisible = true;
                FirstTeamConcededList.IsVisible = false;
                CoronaTeamConcededList.IsVisible = false;
                WomenTeamConcededList.IsVisible = false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void FirstDivisionTap_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                PremierDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstDivision.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabel.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];


                PremierDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabelLB.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];

                PremierLeagueList.IsVisible = false;
                FirstDivisionList.IsVisible = true;
                CoronaLeagueList.IsVisible = false;
                WomenLeagueList.IsVisible = false;

                PremierPlayerGoalsList.IsVisible = false;
                FirstPlayerGoalsList.IsVisible = true;
                CoronaPlayerGoalsList.IsVisible = false;
                WomenPlayerGoalsList.IsVisible = false;

                PremierTeamScoredList.IsVisible = false;
                FirstTeamScoredList.IsVisible = true;
                CoronaTeamScoredList.IsVisible = false;
                WomenTeamScoredList.IsVisible = false;

                PremierTeamConcededList.IsVisible = false;
                FirstTeamConcededList.IsVisible = true;
                CoronaTeamConcededList.IsVisible = false;
                WomenTeamConcededList.IsVisible = false;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void CoronaDivisionTap_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                PremierDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaDivision.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaLabel.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];


                PremierDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaLabelLB.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];

                PremierLeagueList.IsVisible = false;
                FirstDivisionList.IsVisible = false;
                CoronaLeagueList.IsVisible = true;
                WomenLeagueList.IsVisible = false;

                PremierPlayerGoalsList.IsVisible = false;
                FirstPlayerGoalsList.IsVisible = false;
                CoronaPlayerGoalsList.IsVisible = true;
                WomenPlayerGoalsList.IsVisible = false;

                PremierTeamScoredList.IsVisible = false;
                FirstTeamScoredList.IsVisible = false;
                CoronaTeamScoredList.IsVisible = true;
                WomenTeamScoredList.IsVisible = false;

                PremierTeamConcededList.IsVisible = false;
                FirstTeamConcededList.IsVisible = false;
                CoronaTeamConcededList.IsVisible = true;
                WomenTeamConcededList.IsVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void WomenDivisionTap_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                PremierDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenDivision.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];

                PremierLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenLabel.TextColor = (Color)App.Current.Resources["buttonTextColor"];


                PremierDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                CoronaDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                WomenDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];

                PremierLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                CoronaLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                WomenLabelLB.TextColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLeagueList.IsVisible = false;
                FirstDivisionList.IsVisible = false;
                CoronaLeagueList.IsVisible = false;
                WomenLeagueList.IsVisible = true;

                PremierPlayerGoalsList.IsVisible = false;
                FirstPlayerGoalsList.IsVisible = false;
                CoronaPlayerGoalsList.IsVisible = false;
                WomenPlayerGoalsList.IsVisible = true;

                PremierTeamScoredList.IsVisible = false;
                FirstTeamScoredList.IsVisible = false;
                CoronaTeamScoredList.IsVisible = false;
                WomenTeamScoredList.IsVisible = true;

                PremierTeamConcededList.IsVisible = false;
                FirstTeamConcededList.IsVisible = false;
                CoronaTeamConcededList.IsVisible = false;
                WomenTeamConcededList.IsVisible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
