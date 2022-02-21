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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //string Token = await SecureStorage.GetAsync("Token");
            //string email = await SecureStorage.GetAsync("Email");

            //if (String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(email))
            //{
            //    await Navigation.PushModalAsync(new SignInPage(), true);
            //}

            //appLinkEntry = new AppLinkEntry
            //{
            //    AppLinkUri = new Uri(Constants.ApplicationTicketURL),
            //    Description = "ONTRACK Match Ticketing",
            //    Title = "ONTRACK Tickets",
            //    IsLinkActive = true
            //};

            MessagingCenter.Send<AccountPage>(this, "Refresh");
        }
    }
}
