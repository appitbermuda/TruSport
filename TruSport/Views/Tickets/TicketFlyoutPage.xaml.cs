using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TicketFlyoutPage : FlyoutPage
    {
        public TicketFlyoutPage()
        {
            InitializeComponent();

            flyoutPage.OnTrackListView.ItemTapped += ListView_ItemTapped;
        }

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as TicketMasterDetailPageMenuItem;
            if (item == null)
                return;

            if (item.Title == "Contact Us")
            {
                try
                {
                    List<string> toEmail = new List<string>();
                    toEmail.Add(Constants.OnTrackTicketingEmail);

                    var message = new EmailMessage
                    {
                        Subject = "Enquiry",
                        Body = "",
                        To = toEmail
                    };
                    await Email.ComposeAsync(message);
                }
                catch (FeatureNotSupportedException fbsEx)
                {
                    // Email is not supported on this device
                    await DisplayAlert("Not Supported", "It looks like we cannot email for you, please try from your email.", "Okay");
                }
                catch (Exception ex)
                {
                    // Some other exception occurred
                    await DisplayAlert("Error", "There was an issue email, please try again.", "Okay");
                }
            }
            else
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page)
                {
                    BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                    BarTextColor = (Color)App.Current.Resources["navTextColor"]
                };
            }

            IsPresented = false;

            flyoutPage.OnTrackListView.SelectedItem = null;
        }
    }
}
