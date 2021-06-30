using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views.Cricket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CricketMasterDetailPage : MasterDetailPage
    {
        public CricketMasterDetailPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                IsGestureEnabled = false;

            MasterPage.OnTrackListView.ItemTapped += ListView_ItemTapped;
        }

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as CricketMasterDetailPageMenuItem;
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
                var page = (Page)Activator.CreateInstance(item.TargetType);
                App.Current.MainPage = page;
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
