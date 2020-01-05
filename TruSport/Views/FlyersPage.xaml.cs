using System;
using System.Collections.Generic;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class FlyersPage : ContentPage
    {
        FlyerViewModel flyerViewModel;

        public FlyersPage()
        {
            
            flyerViewModel = new FlyerViewModel();

            this.BindingContext = flyerViewModel;

            InitializeComponent();
        }
    }
}
