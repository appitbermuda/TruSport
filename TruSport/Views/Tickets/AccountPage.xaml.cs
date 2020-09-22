using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class AccountPage : ContentPage
    {
        AccountPageViewModel accountPageViewModel;

        public AccountPage()
        {
            accountPageViewModel = new AccountPageViewModel(Navigation);

            this.BindingContext = accountPageViewModel;
            InitializeComponent();
        }


    }
}
