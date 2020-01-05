using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using TruSport.Data;
using TruSport.Views.Football;
using TruSport.ViewModels;

namespace TruSport.Views
{
    public partial class SignInPage : ContentPage
    {
        
        AuthenticationViewModel authenticationViewModel;

        public SignInPage()
        {
            authenticationViewModel = new AuthenticationViewModel(Navigation);
            this.BindingContext = authenticationViewModel;
            InitializeComponent();

            
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            //BusyIndicator.IsVisible = true;
            SigninButton.IsEnabled = false;
            bool userNameValid = true;
            bool passwordValid = true;

            if (EmailEntry.Text == null || EmailEntry.Text == "")
                userNameValid = false;

            if (PasswordEntry.Text == null || PasswordEntry.Text == "")
                passwordValid = false;

            if(userNameValid && passwordValid)
            {
                //var userID = await databaseManager.UserSignIn(EmailEntry.Text.ToLower(), PasswordEntry.Text);

                //if (userID == null)
                //{
                //    //BusyIndicator.IsVisible = false;
                //    await DisplayAlert("Sign In", "The email or password is incorrect.", "Okay");
                //}
                //else
                //{
                //    App.UserID = userID;
                //    //BusyIndicator.IsVisible = false;
                //    await PopupNavigation.Instance.PopAllAsync();
                //    App.Current.MainPage = new FootballMainPage();
                //}
                    
            }
            else
            {
                //BusyIndicator.IsVisible = false;
            }

            SigninButton.IsEnabled = true;
            //var loadingPage = new LoadingPopupPage();
            //await Navigation.PushPopupAsync(loadingPage);
            //await Task.Delay(2000);
            //await Navigation.RemovePopupPageAsync(loadingPage);
            //await Navigation.PushPopupAsync(new LoginSuccessPopupPage());
            //await PopupNavigation.Instance.PushAsync(_signInPopup);
        }

        private async void OnCloseButtonTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
            //await PopupNavigation.Instance.PushAsync(_signInPopup);
        }
    }
}
