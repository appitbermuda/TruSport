using System;
using System.Collections.Generic;
using System.Diagnostics;
using TruSport.Model;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class PurchaseTicketPage : ContentPage
    {
        PurchaseTicketPageViewModel purchaseTicketPageViewModel;

        public PurchaseTicketPage(Fixture fixture)
        {
            purchaseTicketPageViewModel = new PurchaseTicketPageViewModel(Navigation, fixture);

            this.BindingContext = purchaseTicketPageViewModel;

            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                purchaseTicketPageViewModel.IsBusy = true;
                //tickets/terms
                await Navigation.PushModalAsync(new WebviewPage("https://www.ontrackbda.com/tickets/terms"));

                purchaseTicketPageViewModel.IsBusy = false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
