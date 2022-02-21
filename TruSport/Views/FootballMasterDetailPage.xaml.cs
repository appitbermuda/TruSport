using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using TruSport.Views.Football;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMasterDetailPage : MasterDetailPage
    {
        public FootballMasterDetailPage()
        {
            InitializeComponent();

            MasterPage.OnTrackListView.ItemTapped += ListView_ItemTapped;

            if (Device.RuntimePlatform == Device.iOS)
                IsGestureEnabled = false;

            //var defaultSport = SecureStorage.GetAsync("DefaultSport").Result;

            //if (String.IsNullOrEmpty(defaultSport))
            //{
            //    defaultSport = "Football";
            //    SecureStorage.SetAsync("DefaultSport", defaultSport);
            //}

            //var sport = SecureStorage.GetAsync("Sport").Result;

            ////if (defaultSport != sport)
            ////    SecureStorage.SetAsync("Sport", sport);

            //if (sport == "Cricket")
            //{
            //    Detail = new NavigationPage(new Cricket.FixturePage())
            //    {
            //        BarBackgroundColor = (Color)App.Current.Resources["primaryBarBlue"],
            //        BarTextColor = Color.White
            //    };
            //}
            //else
            //{
            //    Detail = new NavigationPage(new Football.FixturePage())
            //    {
            //        BarBackgroundColor = (Color)App.Current.Resources["primaryBarBlue"],
            //        BarTextColor = Color.White
            //    };
            //}
        }

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    var sport = await SecureStorage.GetAsync("Sport");

        //    if (sport == "Cricket")
        //    {
        //        Detail = new NavigationPage(new Cricket.FixturePage())
        //        {
        //            BarBackgroundColor = (Color)App.Current.Resources["primaryBarBlue"],
        //            BarTextColor = Color.White
        //        };
        //    }
        //    else
        //    {
        //        Detail = new NavigationPage(new Football.FixturePage())
        //        {
        //            BarBackgroundColor = (Color)App.Current.Resources["primaryBarBlue"],
        //            BarTextColor = Color.White
        //        };
        //    }
        //}

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as FootballMasterDetailPageMenuItem;
            if (item == null)
                return;

            if (item.Title == "Contact Us")
            {
                try
                {
                    List<string> toEmail = new List<string>();
                    toEmail.Add(Constants.OnTrackEmail);

                    var message = new EmailMessage
                    {
                        Subject = "Enquiry",
                        Body = "",
                        To = toEmail,
                        //Cc = ccRecipients,
                        //Bcc = bccRecipients
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
            else if(item.Title == "Tickets")
            {
                //var page = (Page)Activator.CreateInstance(item.TargetType);
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

            MasterPage.OnTrackListView.SelectedItem = null;
        }
    }
}
