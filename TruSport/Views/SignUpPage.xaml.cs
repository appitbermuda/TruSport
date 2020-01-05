using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using TruSport.Data;
using TruSport.Model;
using TruSport.Views.Football;
using TruSport.ViewModels;

namespace TruSport.Views
{
    public partial class SignUpPage : ContentPage
    {
        bool firstNameValid = true;
        bool lastNameValid = true;
        bool emailValid = true;
        bool passwordValid = true;
        bool confirmPasswordValid = true;

        
        AuthenticationViewModel authenticationViewModel;

        public SignUpPage()
        {
            authenticationViewModel = new AuthenticationViewModel(Navigation);
            this.BindingContext = authenticationViewModel;
            InitializeComponent();

            
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            //BusyIndicator.IsVisible = true;
            SignUpButton.IsEnabled = false;

            if (FirstNameEntry.Text == null)
            {
                firstNameValid = false;
                FirstNameEntry.TextColor = Color.Red;
            }

            if (LastNameEntry.Text == null)
            {
                lastNameValid = false;
                LastNameEntry.TextColor = Color.Red;
            }

            if (EmailEntry.Text == null)
            {
                emailValid = false;
                EmailEntry.TextColor = Color.Red;
            }

            if (PasswordEntry.Text == null)
            {
                passwordValid = false;
                PasswordEntry.TextColor = Color.Red;
            }

            if (ConfirmPasswordEntry.Text == null)
            {
                confirmPasswordValid = false;
                ConfirmPasswordEntry.TextColor = Color.Red;
            }

            if (PasswordEntry.Text != null && ConfirmPasswordEntry.Text != null)
            {
                if(PasswordEntry.Text != ConfirmPasswordEntry.Text)
                {
                    passwordValid = false;
                    confirmPasswordValid = false;

                    PasswordEntry.TextColor = Color.Red;
                    ConfirmPasswordEntry.TextColor = Color.Red;
                }
            }

            if(firstNameValid && lastNameValid && emailValid && passwordValid && confirmPasswordValid)
            {
                //var userID = await databaseManager.UserSignIn(EmailEntry.Text.ToLower(), PasswordEntry.Text);
                //var userTypeID = await databaseManager.GetUserTypeByName("Observer");

                //if (userID == null)
                //{
                //    User user = new User
                //    {
                //        FirstName = FirstNameEntry.Text,
                //        LastName = LastNameEntry.Text,
                //        Email = EmailEntry.Text,
                //        Password = PasswordEntry.Text,
                //        UserTypeID = userTypeID.ID,
                //        IsLoggedIn = true
                //    };

                //    await databaseManager.SaveUser(user);
                //    var newUser = await databaseManager.GetUserByEmail(user.Email);

                //    App.UserID = newUser.ID;
                //    BusyIndicator.IsVisible = false;
                //    await PopupNavigation.Instance.PopAllAsync();
                //    App.Current.MainPage = new FootballMainPage();
                //}
                //else
                //{
                //    BusyIndicator.IsVisible = false;
                //    await DisplayAlert("Sign In", "This user account already exist, please sign in.", "Okay");

                //}
            }

            SignUpButton.IsEnabled = true;

            //await PopupNavigation.Instance.PushAsync(_signInPopup);
        }

        private void FirstNameTextChanged(object sender, EventArgs e)
        {
            FirstNameEntry.TextColor = Color.Black;
        }

        private void LastNameTextChanged(object sender, EventArgs e)
        {
            LastNameEntry.TextColor = Color.Black;
        }

        private void EmailTextChanged(object sender, EventArgs e)
        {
            EmailEntry.TextColor = Color.Black;
        }

        private void PasswordTextChanged(object sender, EventArgs e)
        {
            PasswordEntry.TextColor = Color.Black;
        }

        private void ConfirmPasswordTextChanged(object sender, EventArgs e)
        {
            ConfirmPasswordEntry.TextColor = Color.Black;
        }

        private async void OnCloseButtonTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
            //await PopupNavigation.Instance.PushAsync(_signInPopup);
        }
    }
}
