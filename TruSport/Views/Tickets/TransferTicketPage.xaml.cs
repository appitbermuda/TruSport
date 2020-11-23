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

        public TransferTicketPage(MatchTicket matchTicket)
        {
            transferTicketPageViewModel = new TransferTicketPageViewModel(Navigation, matchTicket);

            InitializeComponent();

            this.BindingContext = transferTicketPageViewModel;
        }
    }
}
