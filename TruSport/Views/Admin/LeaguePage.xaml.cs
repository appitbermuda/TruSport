using System;
using System.Collections.Generic;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class LeaguePage : ContentPage
    {
        public LeaguePage()
        {
            InitializeComponent();
        }

        async void LeagueTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as League;

            await Navigation.PushAsync(new EditLeaguePage(item));
        }

        async void CloseClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
