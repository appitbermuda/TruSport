using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TruSport.Data;
using TruSport.Model;
using TruSport.Views.Football;
using TruSport.ViewModels;
using TruSport.ViewModel.Shop;
using System.Linq;

namespace TruSport.Views
{
    public partial class SignUpPage : ContentPage
    {
        bool firstNameValid = true;
        bool lastNameValid = true;
        bool emailValid = true;
        bool passwordValid = true;
        bool confirmPasswordValid = true;

        
        RegisterViewModel registerViewModel;

        public SignUpPage()
        {
            registerViewModel = new RegisterViewModel(Navigation);
            this.BindingContext = registerViewModel;
            InitializeComponent();

            
        }

        private void RegisterPage_BindingContextChanged(object sender, EventArgs e)
        {
            registerViewModel.ErrorsChanged += LoginViewmodel_ErrorsChanged;
        }

        private void LoginViewmodel_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            var propHasErrors = (registerViewModel.GetErrors(e.PropertyName) as List<string>)?.Any() == true;
            switch (e.PropertyName)
            {
                case nameof(registerViewModel.FirstName):
                    FirstNameLabel.TextColor = propHasErrors
                    ? Color.Red : Color.Gray;
                    break;
                case nameof(registerViewModel.LastName):
                    LastNameLabel.TextColor = propHasErrors
                    ? Color.Red : Color.Gray;
                    break;
                case nameof(registerViewModel.Email):
                    EmailLabel.TextColor = propHasErrors
                    ? Color.Red : Color.Gray;
                    break;
                case nameof(registerViewModel.Password):
                    PasswordLabel.TextColor = propHasErrors
                    ? Color.Red : Color.Gray;
                    break;
                case nameof(registerViewModel.ConfirmPassword):
                    ConfirmPasswordLabel.TextColor = propHasErrors
                    ? Color.Red : Color.Gray;
                    break;
                default:
                    break;
            }
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
