using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class RegisterViewModel : BaseValidationViewModel
    {
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedSportChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedRoleChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedTeamChangedCommand;
        public CustomerRequest _customerRequest;
        public string _firstName;
        public string _lastName;
        public string _email;
        public string _phone;
        public string _password;
        public string _confirmPassword;
        private bool _isSportSelected;
        private bool _isRoleSelected;

        public CustomerRequest Customer
        {
            get { return _customerRequest; }
            set { Set(ref _customerRequest, value); }
        }

        public bool IsSportSelected
        {
            get { return _isSportSelected; }
            set { Set(ref _isSportSelected, value); }
        }


        public bool IsRoleSelected
        {
            get { return _isRoleSelected; }
            set { Set(ref _isRoleSelected, value); }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                Set(ref _firstName, value);
                Validate(() => !string.IsNullOrWhiteSpace(_firstName), "Please provide your first name.");
                RegisterCommand.ChangeCanExecute();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                Set(ref _lastName, value);
                Validate(() => !string.IsNullOrWhiteSpace(_lastName), "Please provide your last name.");
                RegisterCommand.ChangeCanExecute();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                Set(ref _email, value);
                Validate(() => !string.IsNullOrWhiteSpace(_email), "Please enter your email.");
                RegisterCommand.ChangeCanExecute();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                Set(ref _phone, value);
                Validate(() => !string.IsNullOrWhiteSpace(_phone), "Please enter your phone number.");
                RegisterCommand.ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                Set(ref _password, value);
                Validate(() => !string.IsNullOrWhiteSpace(_password), "Please enter your password.");
                RegisterCommand.ChangeCanExecute();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                Set(ref _confirmPassword, value);
                Validate(() => !string.IsNullOrWhiteSpace(_confirmPassword), "Please re-enter your password");
                RegisterCommand.ChangeCanExecute();
            }
        }

        Command _registerCommand;
        public Command RegisterCommand => _registerCommand ?? (_registerCommand = new Command(Register, CanRegister));
        INavigation Navigation;
        AuthenticationService authenticationService;

        public RegisterViewModel(INavigation navigation)
        {
            Navigation = navigation;
            authenticationService = new AuthenticationService();

            IsRoleSelected = false;

            Customer = new CustomerRequest();

            GenerateSource();

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
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void Register()
        {
            IsBusy = true;
            var IsValid = true;

            try
            {
                if (FirstName != null && LastName != null && Email != null && Password != null && Phone != null)
                {

                    if (IsValid && Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                    {
                        if (Password != null && ConfirmPassword != null)
                        {
                            if (Password == ConfirmPassword)
                            {
                                if (Password.Length >= 8)
                                {
                                    bool customerExists = await authenticationService.CustomerExists(Email);

                                    if (!customerExists)
                                    {

                                        Customer.FirstName = FirstName;
                                        Customer.LastName = LastName;
                                        Customer.Email = Email;
                                        Customer.Phone = Phone;
                                        Customer.Password = Password;


                                        var customerRegistered = await authenticationService.SignUp(Customer);

                                        if (customerRegistered != null)
                                        {
                                            IsBusy = false;

                                            await Application.Current.MainPage.DisplayAlert("Register", "Thanks for signing up!", "Okay");


                                            await Navigation.PopAsync();
                                        }
                                        else
                                        {
                                            await Application.Current.MainPage.DisplayAlert("Register", "There was an issue registering, please try again.", "Okay");
                                        }

                                    }
                                    else
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Register", "The email address already exists, please sign in.", "Okay");
                                    }
                                    

                                    IsBusy = false;
                                }
                                else
                                {
                                    IsBusy = false;
                                    await Application.Current.MainPage.DisplayAlert("Register", "Password must be atleast 8 characters", "Okay");
                                }
                            }
                            else
                            {
                                IsBusy = false;
                                await Application.Current.MainPage.DisplayAlert("Register", "Your passwords do not match.", "Okay");
                            }
                        }
                    }
                    else
                    {
                        IsBusy = false;
                        await Application.Current.MainPage.DisplayAlert("Register", "Please enter a valid email.", "Okay");
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        bool CanRegister() => !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !HasErrors;
    }
}
