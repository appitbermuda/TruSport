using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TruSport.Views;
using TruSport;
using TruSport.Data;
using TruSport.Model;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Newtonsoft.Json;
using Xamarin.Forms;
using TruSport.Services;
using Xamarin.Essentials;
using TruSport.Views.Tickets;

namespace TruSport.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedCountryChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedTeamChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedRoleChangedCommand;
        private Team team;
        private User user;
        private UserType userType;
        private string userTypeName;
        private string userTypeID;
        private string teamName;
        private string teamID;
        private string firstname;
        private string lastname;
        private string email;
        private string temporaryPassword;
        private string password;
        private string confirmPassword;
        private bool _isActivityIndicatorVisible;
        private bool buttonDisabled;
        private bool isTeamComboVisible;
        public bool firstNameError;
        public bool lastNameError;
        public bool emailError;
        public bool passwordError;
        public bool passwordResetRequestSent;
        public ObservableCollection<string> countryCollection;
        private ObservableCollection<Team> teamCollection;
        private ObservableCollection<UserType> userTypeCollection;
        INavigation Navigation;
        UserService userService;
        TeamService teamService;
        UserTypeService userTypeService;

        public AuthenticationViewModel()
        {
            
        }

        public AuthenticationViewModel(INavigation navigation)
        {
            Navigation = navigation;
            userService = new UserService();
            teamService = new TeamService();
            userTypeService = new UserTypeService();

            CountryCollection = new ObservableCollection<string>();

            GenerateSource();

            SelectedTeamChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(TeamSelectionChanged);
            SelectedRoleChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(RoleSelectionChanged);
            SignUpCommand = new Command(async () => await SignUp());
            SignInCommand = new Command(async () => await SignIn());
            PasswordResetClickedCommand = new Command(async () => await PasswordResetClicked());
        }



        public AuthenticationViewModel(INavigation navigation, string resetPW)
        {
            Navigation = navigation;
            userService = new UserService();
            
            GenerateSource("");

            SelectedTeamChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(TeamSelectionChanged);
            SelectedRoleChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(RoleSelectionChanged);
            ResetPasswordRequestCommand = new Command(async () => await ResetPasswordRequest());
            ResetPasswordCommand = new Command(async () => await ResetPassword());

        }

        public Command ResetPasswordRequestCommand { get; }
        public Command ResetPasswordCommand { get; }
        public Command PasswordResetClickedCommand { get; }
        

        public Command SignInCommand { get; }
        public Command SignUpCommand { get; }
        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedCountryChangedCommand
        {
            get { return selectedCountryChangedCommand; }
            set { selectedCountryChangedCommand = value; }
        }

        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedTeamChangedCommand
        {
            get { return selectedTeamChangedCommand; }
            set { selectedTeamChangedCommand = value; }
        }

        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedRoleChangedCommand
        {
            get { return selectedRoleChangedCommand; }
            set { selectedRoleChangedCommand = value; }
        }

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref teamCollection, value); }
        }

        public ObservableCollection<UserType> UserTypeCollection
        {
            get { return userTypeCollection; }
            set { Set(ref userTypeCollection, value); }
        }

        public ObservableCollection<string> CountryCollection
        {
            get { return countryCollection;  }
            set { Set(ref countryCollection, value); }
        }

        public bool IsTeamComboVisible
        {
            get { return isTeamComboVisible; }
            set { Set(ref isTeamComboVisible, value); }
        }


        public bool FirstNameError
        {
            get { return firstNameError; }
            set { Set(ref firstNameError, value); }
        }


        public bool LastNameError
        {
            get { return lastNameError; }
            set { Set(ref lastNameError, value); }
        }

        public bool EmailError
        {
            get { return emailError; }
            set { Set(ref emailError, value); }
        }

        public bool PasswordError
        {
            get { return passwordError; }
            set { Set(ref passwordError, value); }
        }

        public Team Team
        {
            get { return team; }
            set { Set(ref team, value); }
        }

        public User User
        {
            get { return user; }
            set { Set(ref user, value); }
        }

        public UserType UserType
        {
            get { return userType; }
            set { Set(ref userType, value); }
        }

        public string UserTypeName
        {
            get { return userTypeName; }
            set { Set(ref userTypeName, value); }
        }

        public string UserTypeID
        {
            get { return userTypeID; }
            set { Set(ref userTypeID, value); }
        }

        public string TeamName
        {
            get { return teamName; }
            set { Set(ref teamName, value); }
        }

        public string TeamID
        {
            get { return teamID; }
            set { Set(ref teamID, value); }
        }

        public string FirstName
        {
            get { return firstname; }
            set { Set(ref firstname, value); }
        }

        public string LastName
        {
            get { return lastname; }
            set { Set(ref lastname, value); }
        }

        public string Email
        {
            get { return email; }
            set { Set(ref email, value); }
        }

        public string TemporaryPassword
        {
            get { return temporaryPassword; }
            set { Set(ref temporaryPassword, value); }
        }

        public string Password
        {
            get { return password; }
            set { Set(ref password, value); }
        }

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { Set(ref confirmPassword, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool PasswordResetRequestSent
        {
            get { return passwordResetRequestSent; }
            set { Set(ref passwordResetRequestSent, value); }
        }

        public bool ButtonDisabled
        {
            get { return buttonDisabled; }
            set { Set(ref buttonDisabled, value); }
        }

        internal async void GenerateSource()
        {
            IsTeamComboVisible = false;

            var userTypes = await userTypeService.GetUserTypes();
            var teams = await teamService.GetTeams();
            teams.ForEach(e => e.Team.League = e.League);
            teams.ForEach(e => e.Team.LeagueID = e.LeagueID);

            var teamList = teams.Where(e => e.Season.IsCurrent).Select(e => e.Team).OrderBy(e => e.League.Name).ThenBy(e => e.Name).ToList();
            Team = teamList.FirstOrDefault();
            TeamName = Team.Name;
            

            var userTypeList = userTypes.ToList();
            UserType = userTypeList.FirstOrDefault();
            UserTypeName = UserType.Name;

            TeamCollection = new ObservableCollection<Team>(teamList);
            UserTypeCollection = new ObservableCollection<UserType>(userTypeList);
        }

        internal async void GenerateSource(string resetPW)
        {
            PasswordResetRequestSent = false;
            ButtonDisabled = true;

        }

        async Task SignIn()
        {
            try
            {
                IsActivityIndicatorVisible = true;
                ButtonDisabled = false;

                //Email = "shannon.burchall@gmail.com";
                //Password = "808state";

                if (Email != null && Password != null)
                {
                    if (Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                    {
                        if (Password.Length >= 8)
                        {

                            string Secret = Guid.NewGuid().ToString();
                            await SecureStorage.SetAsync("Secret", Secret.Replace("-",""));

                            UserAuthentication userAuthentication = new UserAuthentication();
                            userAuthentication.Email = Email;
                            userAuthentication.Password = Password;

                            User thisUser = await userService.SignIn(userAuthentication);

                            if (thisUser != null)
                            {
                                bool isEnabled = await Microsoft.AppCenter.Push.Push.IsEnabledAsync();
                                Guid DeviceID = Guid.Empty;
                                if (isEnabled)
                                {
                                    Guid? deviceID = await AppCenter.GetInstallIdAsync();
                                    DeviceID = deviceID.GetValueOrDefault();
                                    App.DeviceID = DeviceID.ToString();
                                }

                                //await App.Database.SaveUser(localUser);

                                await SecureStorage.SetAsync("Email", thisUser.Email);
                                await SecureStorage.SetAsync("Token", thisUser.Token);
                                await SecureStorage.SetAsync("UserLoggedIn", "true");
                                await SecureStorage.SetAsync("UserRole", thisUser.UserType.Name);

                                if(thisUser.TeamID != null)
                                    await SecureStorage.SetAsync("TeamID", thisUser.TeamID);

                                App.UserID = thisUser.ID;
                                App.UserFirstName = thisUser.FirstName;
                                App.UserLastName = thisUser.LastName;
                                App.UserFullName = thisUser.FirstName + " " + thisUser.LastName;
                                
                                //await App.Database.UpdateUser(thisUser);
                                IsActivityIndicatorVisible = false;

                                //Navigation.InsertPageBefore(new MapPage(), Navigation.NavigationStack.First());
                                //Navigation.InsertPageBefore(new FootballMasterDetailPage(), Navigation.NavigationStack.First());
                                //Navigation.InsertPageBefore(new AKVOMasterDetailPage(), Navigation.NavigationStack.First());
                                //await Navigation.PopToRootAsync();

                                //await Navigation.PopAsync();
                                //await Navigation.PushAsync(new NavigationPage(new MapPage()));

                                await Navigation.PopAsync();
                            }
                            else
                            {
                                IsActivityIndicatorVisible = false;
                                await Application.Current.MainPage.DisplayAlert("Sign In", "You have either entered an incorrect email or password, or your account has not been validated. Please try again later.", "Okay");
                            }
                        }
                    }
                    else
                    {
                        EmailError = true;
                    }
                }
                else
                {
                    IsActivityIndicatorVisible = false;
                    await Application.Current.MainPage.DisplayAlert("Sign In", "Please enter a valid email and password.", "Okay");
                }

                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                IsActivityIndicatorVisible = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        

        async Task SignUp()
        {
            try
            {
                IsActivityIndicatorVisible = true;
                ButtonDisabled = false;

                if (FirstName != null && LastName != null & Email != null && Password != null && UserTypeID != null && ((IsTeamComboVisible && TeamID != null) || !IsTeamComboVisible))
                {
                    if (Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                    {
                        if (Password != null && ConfirmPassword != null)
                        {
                            if (Password == ConfirmPassword)
                            {
                                if (Password.Length >= 8)
                                {
                                    bool userExists = await userService.UserExists(Email);

                                    if (!userExists)
                                    {
                                        bool isEnabled = await Microsoft.AppCenter.Push.Push.IsEnabledAsync();
                                        Guid DeviceID = Guid.Empty;
                                        if (isEnabled)
                                        {
                                            Guid? deviceID = await AppCenter.GetInstallIdAsync();
                                            DeviceID = deviceID.GetValueOrDefault();

                                        }

                                        UserRequest saveUser = new UserRequest
                                        {
                                            FirstName = FirstName,
                                            LastName = LastName,
                                            Email = Email,
                                            Password = Password,
                                            UserTypeID = UserTypeID,
                                            TeamID = TeamID
                                        };

                                        var userRegistered = await userService.SignUp(saveUser);

                                        if (userRegistered != null)
                                        {
                                            if (isEnabled)
                                            {
                                                App.DeviceID = DeviceID.ToString();
                                            }

                                            saveUser.ID = App.UserID;
                                            await App.Database.UpdateUser(userRegistered);

                                            App.IsLoggedIn = true;
                                            App.UserFirstName = saveUser.FirstName;
                                            App.UserLastName = saveUser.LastName;
                                            App.UserFullName = saveUser.FirstName + " " + saveUser.LastName;
                                            IsActivityIndicatorVisible = false;

                                            await Application.Current.MainPage.DisplayAlert("Sign Up", "Thanks for signing up! \n\n We will send you an email after we verify your information.", "Okay");

                                            //Navigation.InsertPageBefore(new FootballMasterDetailPage(), Navigation.NavigationStack.First());
                                            //await Navigation.PopToRootAsync();
                                            //App.Current.MainPage = new FootballMasterDetailPage();

                                            await Navigation.PopAsync();
                                        }
                                        else
                                        {
                                            await Application.Current.MainPage.DisplayAlert("Sign Up", "There was an issue registering, please try again.", "Okay");
                                        }

                                    }
                                    else
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Sign Up", "The email address already exists, please sign in.", "Okay");
                                    }

                                    ButtonDisabled = true;
                                    IsActivityIndicatorVisible = false;
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
                    }
                    else
                    {
                        IsActivityIndicatorVisible = false;
                        await Application.Current.MainPage.DisplayAlert("Sign Up", "Please enter a valid email.", "Okay");
                    }
                }
                else
                {
                    FirstNameError = true;
                    LastNameError = true;
                    EmailError = true;
                    PasswordError = true;
                }

                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        async Task PasswordResetClicked()
        {
            try
            {
                await Navigation.PushModalAsync(new ForgotPasswordPage());
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        async Task ResetPasswordRequest()
        {
            try
            {
                IsActivityIndicatorVisible = true;
                ButtonDisabled = false;

                if (Email != null)
                {
                    if (Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                    {
                        bool userExists = await userService.UserExists(Email);

                        if (userExists)
                        {
                            ForgotPassword forgotPassword = new ForgotPassword
                            {
                                Email = Email
                            };

                            await userService.ForgotPassword(forgotPassword);

                            await Application.Current.MainPage.DisplayAlert("Reset Password", "Please check your email.", "Okay");

                            PasswordResetRequestSent = true;

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Reset Password", "Please sign up for an account.", "Okay");
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid email address", "Okay");
                    }

                        ButtonDisabled = true;
                        IsActivityIndicatorVisible = false;
                    
                }
            }
            catch (Exception ex)
            {
                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
            }
            finally
            {
                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
            }
        }

        async Task ResetPassword()
        {
            try
            {
                IsActivityIndicatorVisible = true;
                ButtonDisabled = false;

                if (Email != null && TemporaryPassword != null && Password != null && ConfirmPassword != null)
                {

                    if (Regex.IsMatch(Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                    {
                        if (Password != null && ConfirmPassword != null)
                        {
                            if (Password == ConfirmPassword)
                            {
                                if (Password.Length >= 8)
                                {
                                    bool userExists = await userService.UserExists(Email);

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

                                        //await Navigation.PopModalAsync();

                                        //PasswordResetRequestSent = true;

                                        UserAuthentication userAuthentication = new UserAuthentication();
                                        userAuthentication.Email = Email;
                                        userAuthentication.Password = Password;

                                        User thisUser = await userService.SignIn(userAuthentication);

                                        if (thisUser != null)
                                        {
                                            bool isEnabled = await Microsoft.AppCenter.Push.Push.IsEnabledAsync();
                                            Guid DeviceID = Guid.Empty;
                                            if (isEnabled)
                                            {
                                                Guid? deviceID = await AppCenter.GetInstallIdAsync();
                                                DeviceID = deviceID.GetValueOrDefault();
                                                App.DeviceID = DeviceID.ToString();
                                            }

                                            //await App.Database.SaveUser(localUser);

                                            await SecureStorage.SetAsync("Email", thisUser.Email);
                                            await SecureStorage.SetAsync("Token", thisUser.Token);
                                            await SecureStorage.SetAsync("UserLoggedIn", "true");
                                            await SecureStorage.SetAsync("UserRole", thisUser.UserType.Name);

                                            App.UserID = thisUser.ID;
                                            App.UserFirstName = thisUser.FirstName;
                                            App.UserLastName = thisUser.LastName;
                                            App.UserFullName = thisUser.FirstName + " " + thisUser.LastName;

                                            //await App.Database.UpdateUser(thisUser);
                                            IsActivityIndicatorVisible = false;


                                            await Navigation.PopModalAsync();
                                        }
                                        else
                                        {
                                            IsActivityIndicatorVisible = false;
                                            await Application.Current.MainPage.DisplayAlert("Sign In", "You have either entered an incorrect email or password, or your account has not been validated. Please try again later.", "Okay");
                                        }

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

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid email address", "Okay");
                    }

                    ButtonDisabled = true;
                    IsActivityIndicatorVisible = false;

                }
            }
            catch (Exception ex)
            {
                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
            }
            finally
            {
                ButtonDisabled = true;
                IsActivityIndicatorVisible = false;
            }
        }

        private void RoleSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _role = e.Value as UserType;

            UserTypeName = _role.Name;
            UserType = _role;
            UserTypeID = _role.ID;

            if (_role.Name == "Team Administrator")
            {
                
                IsTeamComboVisible = true;
            }
            else
            {
                TeamName = null;
                TeamID = null;
                IsTeamComboVisible = false;
            }

        }

        private void TeamSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _team = e.Value as Team;

            TeamName = _team.Name;
            Team = _team;
            TeamID = _team.ID;
        }
    }
}
