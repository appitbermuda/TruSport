using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class CreditCardPage : ContentPage
    {
        CreditCardPageViewModel creditCardPageViewModel;

        public CreditCardPage()
        {
            creditCardPageViewModel = new CreditCardPageViewModel(Navigation);

            this.BindingContext = creditCardPageViewModel;

            InitializeComponent();
        }

        //public CreditCardPage(int ID)
        //{
        //    creditCardPageViewModel = new CreditCardPageViewModel(Navigation, ID);

        //    this.BindingContext = creditCardPageViewModel;

        //    InitializeComponent();
        //}
    }
}
