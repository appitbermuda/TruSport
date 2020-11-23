using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class PurchaseTicketResultPage : ContentPage
    {
        PurchaseTicketResultPageViewModel purchaseTicketResultPageViewModel;

        public PurchaseTicketResultPage(PaymentResponse paymentResponse)
        {
            purchaseTicketResultPageViewModel = new PurchaseTicketResultPageViewModel(Navigation, paymentResponse);

            this.BindingContext = purchaseTicketResultPageViewModel;

            InitializeComponent();
        }
    }
}
