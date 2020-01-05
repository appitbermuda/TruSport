using System;
using System.Collections.Generic;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class HelpPage : ContentPage
    {
        HelpPageViewModel helpPageViewModel;

        public HelpPage()
        {
            helpPageViewModel = new HelpPageViewModel();

            this.BindingContext = helpPageViewModel;

            InitializeComponent();
        }
    }
}
