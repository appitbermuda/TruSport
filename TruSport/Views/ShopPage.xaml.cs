using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class ShopPage : ContentPage
    {
        public ShopPage()
        {
            Title = "Shop OnTrack";


            InitializeComponent();

            Web.Source = new Uri("https://shop.ontrackbda.com");

            ActivityIndicator.IsVisible = true;

            Task.Delay(10000);

            ActivityIndicator.IsVisible = false;
        }
    }
}
