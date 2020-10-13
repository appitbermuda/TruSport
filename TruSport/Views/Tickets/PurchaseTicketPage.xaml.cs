using System;
using System.Collections.Generic;
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
    }
}
