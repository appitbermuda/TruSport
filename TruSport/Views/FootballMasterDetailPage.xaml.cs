using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
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
            Analytics.TrackEvent("Master detail page");

            InitializeComponent();
            MasterPage.MatchListView.ItemTapped += ListView_ItemTapped;
            MasterPage.TeamListView.ItemTapped += ListView_ItemTapped;
            MasterPage.AdminListView.ItemTapped += ListView_ItemTapped;
            MasterPage.OnTrackListView.ItemTapped += ListView_ItemTapped;

            if (Device.RuntimePlatform == Device.iOS)
                IsGestureEnabled = false;
        }

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
            else
            { 
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                //Detail = new NavigationPage(page)
                //{
                //    BarBackgroundColor = (Color)App.Current.Resources["primaryDarkBlue"],
                //    BarTextColor = Color.White
                //};

                Detail = new NavigationPage(page)
                {
                    BarBackgroundColor = (Color)App.Current.Resources["primaryBarBlue"],
                    BarTextColor = Color.White
                };
            }

            IsPresented = false;

            MasterPage.AdminListView.SelectedItem = null;
            MasterPage.MatchListView.SelectedItem = null;
            MasterPage.TeamListView.SelectedItem = null;
            MasterPage.OnTrackListView.SelectedItem = null;
        }
    }
}
