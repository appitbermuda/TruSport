using System;
using System.Collections.Generic;
using System.Diagnostics;
using TruSport.Model;
using TruSport.ViewModels.Cricket;
using Xamarin.Forms;

namespace TruSport.Views.Cricket
{
    public partial class StatPage : ContentPage
    {
        StatPageViewModel statPageViewModel;

        public StatPage(MatchType matchType)
        {
            statPageViewModel = new StatPageViewModel(matchType);

            this.BindingContext = statPageViewModel;

            Title = matchType.Name;

            InitializeComponent();
        }

        void PremierDivisionTap_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                PremierDivision.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstDivision.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabel.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];


                PremierDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLabelLB.TextColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];

                PremierLeagueList.IsVisible = true;
                FirstDivisionList.IsVisible = false;

                PremierPlayerMostRunsList.IsVisible = true;
                FirstPlayerMostRunsList.IsVisible = false;

                PremierPlayerMostWicketsList.IsVisible = true;
                FirstPlayerMostWicketsList.IsVisible = false;
            }
            catch (Exception ex)
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

                PremierLabel.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabel.TextColor = (Color)App.Current.Resources["buttonTextColor"];


                PremierDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonTextColor"];
                FirstDivisionLB.BackgroundColor = (Color)App.Current.Resources["buttonBackgroundColor"];

                PremierLabelLB.TextColor = (Color)App.Current.Resources["buttonBackgroundColor"];
                FirstLabelLB.TextColor = (Color)App.Current.Resources["buttonTextColor"];

                PremierLeagueList.IsVisible = false;
                FirstDivisionList.IsVisible = true;

                PremierPlayerMostRunsList.IsVisible = false;
                FirstPlayerMostRunsList.IsVisible = true;

                PremierPlayerMostWicketsList.IsVisible = false;
                FirstPlayerMostWicketsList.IsVisible = true;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
