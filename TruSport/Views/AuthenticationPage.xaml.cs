using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class AuthenticationPage : ContentPage
    {
        public AuthenticationPage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        async void SignInClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());
        }

        async void SignUpClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}
