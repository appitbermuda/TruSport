using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TruSport.Data;
using TruSport.Views.Football;
using TruSport.ViewModels;
using Xamarin.Essentials;
using System.Diagnostics;
using TruSport.ViewModel.Shop;
using System.Linq;

namespace TruSport.Views
{
    public partial class SignInPage : ContentPage
    {
        
        LoginViewModel loginViewModel;

        public SignInPage()
        {
            loginViewModel = new LoginViewModel(Navigation);
            this.BindingContext = loginViewModel;
            InitializeComponent();

            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                //BusyIndicator.IsVisible = true;

                loginViewModel.IsActivityIndicatorVisible = true;

                var userLoggedIn = await SecureStorage.GetAsync("UserLoggedIn");

                if (!String.IsNullOrEmpty(userLoggedIn) && Convert.ToBoolean(userLoggedIn))
                {
                    
                    await Navigation.PopAsync();
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LoginPageOnAppearing");
            }
            finally
            {
                loginViewModel.IsActivityIndicatorVisible = false;
            }
        }

        private void LoginPage_BindingContextChanged(object sender, EventArgs e)
        {
            loginViewModel.ErrorsChanged += LoginViewmodel_ErrorsChanged;
        }

        private void LoginViewmodel_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            var propHasErrors = (loginViewModel.GetErrors(e.PropertyName) as List<string>)?.Any() == true;
            switch (e.PropertyName)
            {
                case nameof(loginViewModel.Email):
                    EmailLabel.ErrorColor = propHasErrors
                    ? Color.Red : Color.Gray;
                    break;
                case nameof(loginViewModel.Password):
                    PasswordLabel.ErrorColor = propHasErrors
                    ? Color.Red : Color.Gray;
                    break;
                default:
                    break;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        void Menu_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.MainPage is MasterDetailPage mdp)
            {
                mdp.IsPresented = true;
            }
        }
    }
}
