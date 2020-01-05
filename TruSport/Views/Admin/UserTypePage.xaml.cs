using System;
using System.Collections.Generic;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class UserTypePage : ContentPage
    {
        public UserTypePage()
        {
            InitializeComponent();
        }

        async void UserTypeTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as UserType;

            await Navigation.PushAsync(new EditUserTypePage(item));
        }

        async void CloseClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
