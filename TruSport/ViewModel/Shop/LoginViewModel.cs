using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.Tickets;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class LoginViewModel : BaseValidationViewModel
    {
        public CustomerAuthentication _userAuthentication;
        public string _email;
        public string _password;
        private bool _isActivityIndicatorVisible;

        public CustomerAuthentication Customer
        {
            get { return _userAuthentication; }
            set { Set(ref _userAuthentication, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public string Email
        {
            get => _email;
            set
            {
                Set(ref _email, value);
                Validate(() => !string.IsNullOrWhiteSpace(_email), "Please enter your email.");
                LoginCommand.ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                Set(ref _password, value);
                Validate(() => !string.IsNullOrWhiteSpace(_password), "Please enter your password.");
                LoginCommand.ChangeCanExecute();
            }
        }

        Command _loginCommand;
        public Command LoginCommand => _loginCommand ?? (_loginCommand = new Command(Login, CanLogin));

        public Command RegisterCommand { get; set; }
        INavigation Navigation;
        AuthenticationService authenticationService;

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            authenticationService = new AuthenticationService();

            Customer = new CustomerAuthentication();

            GenerateSource();

            RegisterCommand = new Command(async () => await Register());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        internal async void GenerateSource()
        {
            try
            {
                IsActivityIndicatorVisible = true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        async Task Register()
        {
            try
            {
                await Navigation.PushAsync(new SignUpPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void Login()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password))
                {
                    if (Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                    {
                        if (Password.Length >= 8)
                        {
                            string Secret = Guid.NewGuid().ToString();
                            await SecureStorage.SetAsync("Secret", Secret.Replace("-", ""));

                            Customer.Email = Email;
                            Customer.Password = Password;

                            Customer thisCustomer = await authenticationService.SignIn(Customer);

                            if (thisCustomer != null)
                            {
                                await App.Database.SignIn(thisCustomer);

                                await SecureStorage.SetAsync("Email", thisCustomer.Email);
                                await SecureStorage.SetAsync("Token", thisCustomer.Token);
                                await SecureStorage.SetAsync("UserLoggedIn", "true");

                                IsActivityIndicatorVisible = false;
                                App.IsLoggedIn = true;
                                                                
                                await Navigation.PopAsync();

                                if (Application.Current.MainPage is MasterDetailPage mdp)
                                {
                                    var page = (Page)Activator.CreateInstance(typeof(TicketTabbedPage));
                                    page.Title = "Tickets";

                                    mdp.Detail = new NavigationPage(page)
                                    {
                                        BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                                        BarTextColor = (Color)App.Current.Resources["navTextColor"]
                                    };
                                }
                            }
                            else
                            {
                                IsActivityIndicatorVisible = false;
                                await Application.Current.MainPage.DisplayAlert("Sign In", "You have either entered an incorrect email or password, or your account has not been validated. Please try again later.", "Okay");
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Login", "You have either entered an incorrect email or password.", "Okay");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Email", "Please enter a valid email.", "Okay");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        bool CanLogin() => !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !HasErrors;
    }
}
