using System;
using System.Collections.Generic;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Views
{
    public partial class NewsPage : ContentPage
    {
        public NewsPage()
        {
            InitializeComponent();

            //loader.Easing = Easing.Linear;
        }

        async void NewsTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as RssFeedItem;
            //Device.OpenUri(item.Link);
            await Navigation.PushAsync(new NewsDetailPage(item));

            //NewsList.SelectedItems.Clear();
        }

        async void AdminClicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushModalAsync(new AdminMainPage());
        }
    }
}
