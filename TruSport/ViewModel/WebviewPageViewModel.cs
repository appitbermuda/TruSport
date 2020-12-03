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
    public class WebviewPageViewModel : BaseViewModel
    {
        private Uri _url;
        HtmlWebViewSource _helpSource;

        private bool _isActivityIndicatorVisible;

        INavigation Navigation;

        public WebviewPageViewModel(INavigation navigation, string url)
        {
            Navigation = navigation;

            GenerateSource(url);
            CloseClickedCommand = new Command(async () => await Close());

        }

        public Command CloseClickedCommand { get; set; }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public HtmlWebViewSource URLSource
        {
            get { return _helpSource; }
            set { Set(ref _helpSource, value); }
        }

        public Uri URL
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }

        internal async void GenerateSource(string url)
        {
            IsActivityIndicatorVisible = true;

            try
            {
                //HelpLink = new Uri("https://elereservice.azurewebsites.net/help");
                //URLSource = GetHtmlSourceWithCustomCss(url);
                URL = new Uri(url);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Webview");
            }

            await Task.Delay(5000);

            IsActivityIndicatorVisible = false;
        }

        async Task Close()
        {
            try
            {
                MessagingCenter.Send(this, "MatchTicketPage");
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Close");
            }
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
    }
}
