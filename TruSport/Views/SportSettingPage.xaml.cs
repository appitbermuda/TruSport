using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class SportSettingPage : ContentPage
    {
        SportSettingViewModel sportSettingViewModel;

        public SportSettingPage()
        {
            sportSettingViewModel = new SportSettingViewModel();

            this.BindingContext = sportSettingViewModel;

            InitializeComponent();
        }

        async void CricketCell_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                string sport = "Cricket";
                sportSettingViewModel.DefaultSport = sport;
                //await SecureStorage.SetAsync("DefaultSport", sport);

                Sport cricket = await App.Database.GetSport(sport);
                await App.Database.SetDefaultSport(cricket);

                MessagingCenter.Send<string>(sport, "SelectedSport");
                await Navigation.PopAsync();
                
            }
            catch(Exception ex)
            {

            }
        }

        async void FootballCell_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                string sport = "Football";
                sportSettingViewModel.DefaultSport = sport;

                Sport football = await App.Database.GetSport(sport);
                await App.Database.SetDefaultSport(football);

                //await SecureStorage.SetAsync("DefaultSport", sport);

                MessagingCenter.Send<string>(sport, "SelectedSport");
                await Navigation.PopAsync();

            }
            catch (Exception ex)
            {

            }
        }

        async void BowlingCell_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                string sport = "Bowling";
                sportSettingViewModel.DefaultSport = sport;
                //await SecureStorage.SetAsync("DefaultSport", sport);

                Sport bowling = await App.Database.GetSport(sport);
                await App.Database.SetDefaultSport(bowling);

                MessagingCenter.Send<string>(sport, "SelectedSport");
                await Navigation.PopAsync();

            }
            catch (Exception ex)
            {

            }
        }

        async void TennisCell_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                string sport = "Tennis";
                sportSettingViewModel.DefaultSport = sport;
                //await SecureStorage.SetAsync("DefaultSport", sport);

                Sport tennis = await App.Database.GetSport(sport);
                await App.Database.SetDefaultSport(tennis);

                MessagingCenter.Send<string>(sport, "SelectedSport");
                await Navigation.PopAsync();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
