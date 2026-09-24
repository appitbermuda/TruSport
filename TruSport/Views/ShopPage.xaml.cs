using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class ShopPage : ContentPage
    {
        public ShopPage(bool backButtonRequired = false)
        {
            //Title = "Shop OnTrack";


            InitializeComponent();

            if (backButtonRequired)
                BackButton.IsVisible = true;

            Web.Source = new Uri("https://shop.ontrackbda.com");

            ActivityIndicator.IsVisible = true;

            Task.Delay(10000);

            ActivityIndicator.IsVisible = false;
        }

        void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

    }
}
