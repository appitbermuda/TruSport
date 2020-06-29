using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TruSport.Data;
using TruSport.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class HelpPageViewModel : BaseViewModel
    {
        private ObservableCollection<Help> helpCollection;
        private Uri _url;
        HtmlWebViewSource _helpSource;

        private bool _isActivityIndicatorVisible;

        public HelpPageViewModel()
        {
            IsActivityIndicatorVisible = true;
            URL = new Uri("https://www.ontrackbda.com/help");

            Task.Delay(5000).ConfigureAwait(true);
            IsActivityIndicatorVisible = false;
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public HtmlWebViewSource HelpSource
        {
            get { return _helpSource; }
            set { Set(ref _helpSource, value); }
        }

        public Uri URL
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                //HelpLink = new Uri("https://elereservice.azurewebsites.net/help");
                HelpSource = GetHtmlSourceWithCustomCss("https://www.ontrackbda.com/help");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Help");
            }

            await Task.Delay(5000);

            IsActivityIndicatorVisible = false;
        }

        public HtmlWebViewSource GetHtmlSourceWithCustomCss(string url)
        {
            var htmlSource = new HtmlWebViewSource();
            string htmlCode;

            try
            {
                using (WebClient client = new WebClient())
                {
                    htmlCode = client.DownloadString(url);
                }

                htmlSource.Html = htmlCode;

                // Replace css
                var customCss = @"
            <head>
                <style>
                body { background-color: [bgcolor] }
                h1 { color: [txtcolor] !important; }
                table   { color: [txtcolor] !important; }
                </style>
            </head>
            ";

                customCss = customCss.Replace("[bgcolor]", GetHexString((Color)App.Current.Resources["backgroundColor"]));
                customCss = customCss.Replace("[txtcolor]", GetHexString((Color)App.Current.Resources["TextPrimaryColor"]));

                htmlCode = customCss + htmlCode;

                htmlSource.Html = htmlCode;

                return htmlSource;
            }
            catch (Exception ex)
            {

            }

            return htmlSource;
        }

        public string GetHexString(Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            //var alpha = (int)(color.A * 255);
            var hex = $"#{red:X2}{green:X2}{blue:X2}";

            return hex;
        }

        //internal async void GenerateSource()
        //{
        //    IsActivityIndicatorVisible = true;

        //    //HelpCollection.Add(new Help
        //    //{
        //    //    Question = "Where can I find standings for a specific league?",
        //    //    Answer = "League Standings can be found a number of ways; 1. Selecting Standings in the menu. 2. Viewing a team of your choice and navigating to the Standing header. or 3. Selecting the league header on a fixture."
        //    //});

        //    //HelpCollection.Add(new Help
        //    //{
        //    //    Question = "Where can I find a teams information?",
        //    //    Answer = "To look at a team of your choosing, go to Teams in the side menu, and select the team you want to view."
        //    //});

        //    //HelpCollection.Add(new Help
        //    //{
        //    //    Question = "I do not see my teams previous fixtures?",
        //    //    Answer = "On your teams profile, if you select the fixtures tab, scroll up in the list and you will see all previous fixtures."
        //    //});

        //    //HelpCollection.Add(new Help
        //    //{
        //    //    Question = "I am looking at the app but the scores aren't updated.",
        //    //    Answer = "Unfortunately, the live score feature is not fully active yet. Check back and refresh the scores by pulling down on the list. Some scores may be updated while select games are being played, otherwise all scores will be updated right after the game has ended."
        //    //});

        //    //HelpCollection.Add(new Help
        //    //{
        //    //    Question = "I am looking at the live games but the scores aren't updated.",
        //    //    Answer = "Unfortunately, the live score feature is not fully active yet. Refresh the scores by pulling down on the list. Some scores may be updated while select games are being played, otherwise all scores will be updated right after the game has ended."
        //    //});


        //    //HelpCollection.Add(new Help
        //    //{
        //    //    Question = "I think your app is missing something, how do I contact you?",
        //    //    Answer = "We are always open to new features that will benefit all users, if you have any ideas or concerns that could better help the app, DM us on twitter or Instagram at @ontrackbda or hit the 'Contact Us' link in the menu. We will be happy to hear from you and evaluate your request."
        //    //});

        //    IsActivityIndicatorVisible = false;
        //}
    }
}
