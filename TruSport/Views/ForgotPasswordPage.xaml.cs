using System;
using System.Collections.Generic;
using TruSport.ViewModel;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        ForgotPasswordViewModel forgotPasswordViewModel;

        public ForgotPasswordPage()
        {
            forgotPasswordViewModel = new ForgotPasswordViewModel(Navigation);

            this.BindingContext = forgotPasswordViewModel;
            InitializeComponent();
        }

        private async void BackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
