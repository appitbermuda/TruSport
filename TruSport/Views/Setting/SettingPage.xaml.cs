using System;
using System.Collections.Generic;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Setting
{
    public partial class SettingPage : ContentPage
    {
        SettingViewModel settingViewModel;

        public SettingPage()
        {
            settingViewModel = new SettingViewModel();

            this.BindingContext = settingViewModel;

            InitializeComponent();
        }

        async void ViewCell_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                MessagingCenter.Instance.Subscribe<string>(this, "SelectedSport", async (sport) =>
                {
                    settingViewModel.DefaultSport = sport;
                });
                await Navigation.PushAsync(new SportSettingPage());
            }
            catch(Exception ex)
            {

            }
        }
    }
}
