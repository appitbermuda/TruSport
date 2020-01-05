using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class PushNotifications : ContentPage
    {
        PushViewModel pushViewModel;

        PushNoti pushNoti;
        public PushNotifications()
        {
            pushNoti = new PushNoti();
            pushViewModel = new PushViewModel();

            this.BindingContext = pushViewModel;

            InitializeComponent();
        }

        //async void SendNotification(object sender, System.EventArgs e)
        //{
        //    if (NameEntry.Text.Length > 0 && TitleEntry.Text.Length > 0 && BodyEntry.Text.Length > 0)
        //    {
        //        var sendNoti = await DisplayAlert("Send Notification", "Are you sure you want to send this notification? Please verify spelling before send.", "Send", "Cancel");

        //        if (sendNoti)
        //        {
        //            var response = await pushNoti.Notify(NameEntry.Text, TitleEntry.Text, BodyEntry.Text);

        //            //var response = await pushNoti.Notify("Push from Mobile", "Push from Mobile", "This is a push from Mobile");

        //            await DisplayAlert("Notification", response, "Okay");

        //            NameEntry.Text = String.Empty;
        //            TitleEntry.Text = String.Empty;
        //            BodyEntry.Text = String.Empty;
        //        }
        //    }
        //}
    }
}
