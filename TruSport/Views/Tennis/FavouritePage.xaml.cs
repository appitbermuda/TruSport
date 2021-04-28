using System;
using System.Collections.Generic;
using TruSport.ViewModels.Tennis;
using Xamarin.Forms;

namespace TruSport.Views.Tennis
{
    public partial class FavouritePage : ContentPage
    {
        FavouritePageViewModel favouritePageViewModel;

        public FavouritePage()
        {
            favouritePageViewModel = new FavouritePageViewModel();

            InitializeComponent();

            this.BindingContext = favouritePageViewModel;
        }
    }
}
