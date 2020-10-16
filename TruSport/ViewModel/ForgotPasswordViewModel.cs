using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TruSport.ViewModels;
using TruSport.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _tempPassword;
        private string _confirmPassword;
        private bool _isActivityIndicatorVisible;
        private bool buttonDisabled;
        private bool _isEmailSent;
        //DatabaseManager databaseManager;
        UserService userService;

        public INavigation Navigation { get; set; }
        public string RoleID;
        public bool emailError;

        public ForgotPasswordViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            userService = new UserService();

            GenerateSource();

            SubmitCommand = new Command(async () => await Submit());
        }

        public Command SubmitCommand { get; }

        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { Set(ref _confirmPassword, value); }
        }

        public string TemporaryPassword
        {
            get { return _tempPassword; }
            set { Set(ref _tempPassword, value); }
        }

        public bool EmailError
        {
            get { return emailError; }
            set { Set(ref emailError, value); }
        }

        public bool IsEmailSent
        {
            get { return _isEmailSent; }
            set { Set(ref _isEmailSent, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool ButtonDisabled
        {
            get { return buttonDisabled; }
            set { Set(ref buttonDisabled, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            IsEmailSent = false;

            IsActivityIndicatorVisible = false;
        }

        async Task Submit()
        {
            try
            {
                IsActivityIndicatorVisible = true;
                ButtonDisabled = false;

                if (!String.IsNullOrEmpty(Email) && !IsEmailSent)
                {

                }

                if (!String.IsNullOrEmpty(Email) && !IsEmailSent)
                {
                    var hasTempPassword = await userService.HasTemporaryPassword(Email);

                    if (!hasTempPassword)
                    {
                        if (Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                        {

                            bool forgotPasswordSent = await userService.ForgotPassword(Email);

                            if (forgotPasswordSent)
                            {
                                await App.Current.MainPage.DisplayAlert("Forgot Password", "Please check your email, we have sent you a temporary password.", "Okay");

                                IsEmailSent = true;

                                IsActivityIndicatorVisible = false;

                            }
                            else
                            {
                                IsActivityIndicatorVisible = false;
                                await Application.Current.MainPage.DisplayAlert("Sign In", "You have entered an incorrect email or password, or your account has not been validated yet.", "Okay");
                            }

                        }
                        else
                        {
                            EmailError = true;
                        }
                    }
                    else
                    {
                        IsEmailSent = true;
                    }
                }
                else if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password) && !String.IsNullOrEmpty(TemporaryPassword) && !String.IsNullOrEmpty(ConfirmPassword) && IsEmailSent)
                {
                    try
                    {
                        if (Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                        {
                            if (Password == ConfirmPassword)
                            {
                                if (Password.Length >= 8)
                                {
                                    bool userExists = await userService.DoesUserExist(Email);

                                    if (userExists)
                                    {
                                        PasswordReset passwordReset = new PasswordReset
                                        {
                                            Email = Email,
                                            Password = Password,
                                            TemporaryPassword = TemporaryPassword
                                        };

                                        await userService.ResetPassword(passwordReset);

                                        await Application.Current.MainPage.DisplayAlert("Success", "Password reset successfully!.", "Okay");

                                        UserAuthentication User = new UserAuthentication();
                                        User.Email = Email;
                                        User.Password = Password;

                                        User userAuthenticated = await userService.SignIn(User);

                                        if (userAuthenticated != null)
                                        {
                                            await App.Database.SaveUserAsync(userAuthenticated);

                                            await SecureStorage.SetAsync("UserEmail", userAuthenticated.Email);
                                            await SecureStorage.SetAsync("ProfileID", userAuthenticated.ProfileID);
                                            await SecureStorage.SetAsync("Role", userAuthenticated.RoleName);

                                            App.IsLoggedIn = true;
                                            App.UserRole = userAuthenticated.RoleName;

                                            IsActivityIndicatorVisible = false;

                                            Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                                            await Navigation.PopToRootAsync();

                                        }
                                        else
                                        {
                                            IsActivityIndicatorVisible = false;
                                            await Application.Current.MainPage.DisplayAlert("Sign In", "You have entered an incorrect email or password, or your account has not been validated yet.", "Okay");
                                        }

                                        Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                                        await Navigation.PopToRootAsync();

                                    }
                                    else
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Reset Password", "Please sign up for an account.", "Okay");
                                    }
                                }
                                else
                                {
                                    IsActivityIndicatorVisible = false;
                                    await Application.Current.MainPage.DisplayAlert("Sign Up", "Password must be atleast 8 characters", "Okay");
                                }
                            }
                            else
                            {
                                IsActivityIndicatorVisible = false;
                                await Application.Current.MainPage.DisplayAlert("Sign Up", "Your passwords do not match.", "Okay");
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid email address", "Okay");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "Password Reset");
                    }
                }
            }
            catch (Exception ex)
            {
                IsActivityIndicatorVisible = false;
                SecureStorage.RemoveAll();
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
            }
            finally
            {
                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
            }
        }

        async Task ForgotPassword()
        {
            await Navigation.PushModalAsync(new ForgotPasswordPage());
        }
    }
}
