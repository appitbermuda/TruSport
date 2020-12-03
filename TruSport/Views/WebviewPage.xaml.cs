using System;
using System.Collections.Generic;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class WebviewPage : ContentPage
    {
        WebviewPageViewModel webviewPageViewModel;

        public WebviewPage(string url)
        {
            webviewPageViewModel = new WebviewPageViewModel(Navigation, url);

            InitializeComponent();

            this.BindingContext = webviewPageViewModel;
        }
    }
}
