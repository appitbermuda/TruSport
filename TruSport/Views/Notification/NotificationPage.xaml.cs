using System;
using System.Collections.Generic;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.Notification
{
    public partial class NotificationPage : ContentPage
    {

        NotificationPageViewModel notificationPageViewModel;

        public NotificationPage()
        {
            notificationPageViewModel = new NotificationPageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = notificationPageViewModel;
        }
    }
}
