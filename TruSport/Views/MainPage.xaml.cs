using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruSport.Data;
using TruSport.Services;
using TruSport.ViewModel;
using TruSport.Views;
using TruSport.Views.Cricket;
using TruSport.Views.Football;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport
{
    public partial class MainPage : ContentPage
    {
        readonly INotificationRegistrationService _notificationRegistrationService;
        FixtureService fixtureService;

        MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            mainPageViewModel = new MainPageViewModel(Navigation);
            fixtureService = new FixtureService();

            
            InitializeComponent();

            this.BindingContext = mainPageViewModel;
            _notificationRegistrationService =
        PushServiceContainer.Resolve<INotificationRegistrationService>();
            //loader.Easing = Easing.Linear;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            
        }

        void ShowAlert(string message)
        => MainThread.BeginInvokeOnMainThread(()
        => DisplayAlert("Notification", message, "OK").ContinueWith((task)
            => { if (task.IsFaulted) throw task.Exception; }));
    }
}
