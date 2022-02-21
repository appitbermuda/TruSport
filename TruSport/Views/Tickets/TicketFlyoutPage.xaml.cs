using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TicketFlyoutPage : FlyoutPage
    {
        AppLinkEntry appLinkEntry;
        public TicketFlyoutPage()
        {
            InitializeComponent();

            flyoutPage.OnTrackListView.ItemTapped += ListView_ItemTapped;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //string Token = await SecureStorage.GetAsync("Token");
            //string email = await SecureStorage.GetAsync("Email");

            //if (String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(email))
            //{
            //    await Navigation.PushAsync(new SignInPage(), true);
            //}

            appLinkEntry = new AppLinkEntry
            {
                AppLinkUri = new Uri(Constants.ApplicationTicketURL),
                Description = "ONTRACK Match Ticketing",
                Title = "ONTRACK Tickets",
                IsLinkActive = true
            };

            Application.Current.AppLinks.RegisterLink(appLinkEntry);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            appLinkEntry.IsLinkActive = false;
            Application.Current.AppLinks.RegisterLink(appLinkEntry);
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
            else if(item.Id == 1 || item.Id == 2 || item.Id == 3)
            {
                string Token = await SecureStorage.GetAsync("Token");
                string email = await SecureStorage.GetAsync("Email");

                var page = (Page)Activator.CreateInstance(item.TargetType);

                if (String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(email))
                {
                    page = (Page)Activator.CreateInstance(typeof(SignInPage), new object[] { item.TargetType });
                }

                page.Title = item.Title;

                Detail = new NavigationPage(page)
                {
                    BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                    BarTextColor = (Color)App.Current.Resources["navTextColor"]
                };
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
