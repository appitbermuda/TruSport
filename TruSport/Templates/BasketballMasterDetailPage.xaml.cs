using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class BasketballMasterDetailPage : FlyoutPage
    {
        public BasketballMasterDetailPage()
        {
            InitializeComponent();

            flyoutPage.OnTrackListView.ItemTapped += ListView_ItemTapped;
        }

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as BasketballMasterDetailPageMenuItem;
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
            else if (item.Title == "Tickets")
            {
                //string Token = await SecureStorage.GetAsync("Token");
                //string email = await SecureStorage.GetAsync("Email");

                //if (String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(email))
                //{
                //    App.Current.MainPage = new NavigationPage(new MainPage());
                //}
                //else
                //{
                //    var page = (Page)Activator.CreateInstance(item.TargetType);
                //    page.Title = item.Title;

                //    Detail = new NavigationPage(page)
                //    {
                //        BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                //        BarTextColor = (Color)App.Current.Resources["navTextColor"]
                //    };
                //}

                App.Current.MainPage = new Tickets.TicketFlyoutPage();
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
