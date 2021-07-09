using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TransferTicketPage : ContentPage
    {
        TransferTicketPageViewModel transferTicketPageViewModel;

        public TransferTicketPage(CustomerTicket customerTicket)
        {
            transferTicketPageViewModel = new TransferTicketPageViewModel(Navigation, customerTicket);

            InitializeComponent();

            this.BindingContext = transferTicketPageViewModel;
        }
    }
}
