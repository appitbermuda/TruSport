using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class NotificationPageViewModel : BaseViewModel
    {
        private ObservableCollection<PushNotification> _pushNotificationCollection;
        private bool _isPushNotificationActivityIndicatorVisible;
        private bool _isActivityIndicatorVisible;

        INavigation Navigation;
        PushNotificationService pushNotificationService;

        public NotificationPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            pushNotificationService = new PushNotificationService();
            PushNotificationCollection = new ObservableCollection<PushNotification>();

            GenerateSource();
        }

        public ObservableCollection<PushNotification> PushNotificationCollection
        {
            get { return _pushNotificationCollection; }
            set { Set(ref _pushNotificationCollection, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var notifications = await pushNotificationService.GetPushNotifications();
                    if (notifications != null)
                        PushNotificationCollection = new ObservableCollection<PushNotification>(notifications);

                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "PushNotifications");
            }

            IsActivityIndicatorVisible = false;
        }
    }
}
