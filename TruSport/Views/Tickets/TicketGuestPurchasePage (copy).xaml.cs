using System;
using System.Collections.Generic;
using System.Diagnostics;
using TruSport.Model;
using TruSport.ViewModel.Shop;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TicketGuestPurchasePage_Backup : ContentPage
    {
        TicketPurchasePageViewModel ticketPurchasePageViewModel;

        public TicketGuestPurchasePage_Backup(SportEvent sportEvent)
        {
            //ticketPurchasePageViewModel = new TicketPurchasePageViewModel(Navigation, sportEvent, true);

            InitializeComponent();

            this.BindingContext = ticketPurchasePageViewModel;
        }

        public TicketGuestPurchasePage_Backup(List<SportEvent> sportEvent)
        {
            ticketPurchasePageViewModel = new TicketPurchasePageViewModel(Navigation, sportEvent, true);

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


        void OpenEventTicketPicker(object sender, System.EventArgs e)
        {
            EventTicketPicker.IsOpen = !EventTicketPicker.IsOpen;
        }

        async void MatchSpotImageTapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                var param = (string)((TappedEventArgs)e).Parameter;

                bool showImage = await DisplayAlert("Show Image", "Would you like to open the image in the browser?", "Show", "Cancel");

                if (showImage)
                {
                    await Browser.OpenAsync(Constants.ImageEndPoint + param);
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
