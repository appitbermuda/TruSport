using System;
using System.Collections.Generic;
using System.Diagnostics;
using TruSport.Model;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TicketPurchasePage : ContentPage
    {
        TicketPurchasePageViewModel ticketPurchasePageViewModel;

        public TicketPurchasePage(SportEvent sportEvent)
        {
            ticketPurchasePageViewModel = new TicketPurchasePageViewModel(Navigation, sportEvent);

            InitializeComponent();

            this.BindingContext = ticketPurchasePageViewModel;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                ticketPurchasePageViewModel.IsBusy = true;
                //tickets/terms
                await Navigation.PushModalAsync(new WebviewPage("https://www.ontrackbda.com/tickets/terms"));

                ticketPurchasePageViewModel.IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void SfNumericUpDown_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            if (e.Value != null)
            {
                ticketPurchasePageViewModel.UpdateQuantityCommand.Execute(null);
            }
        }
    }
}
