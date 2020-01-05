using System;
using System.Collections.Generic;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        AuthenticationViewModel authenticationViewModel;

        public ForgotPasswordPage()
        {
            authenticationViewModel = new AuthenticationViewModel(Navigation, "Reset Password");

            this.BindingContext = authenticationViewModel;
            InitializeComponent();
        }

        private async void BackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
