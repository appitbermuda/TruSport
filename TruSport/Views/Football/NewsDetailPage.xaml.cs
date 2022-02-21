using System;
using System.Collections.Generic;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class NewsDetailPage : ContentPage
    {
        public NewsDetailPage()
        {
            InitializeComponent();
        }

        public NewsDetailPage(RssFeedItem feed)
        {
            if (feed.Creator.Contains("Bernews"))
                Title = "Bernews";
            else
                Title = feed.Creator;


            InitializeComponent();

            Web.Source = feed.Link;
        }
    }
}
