using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class PurchasePage : ContentPage
    {
        MatchTicketPageViewModel matchTicketPageViewModel;

        public PurchasePage()
        {
            matchTicketPageViewModel = new MatchTicketPageViewModel(Navigation);

            this.BindingContext = matchTicketPageViewModel;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Send<PurchasePage>(this, "Refresh");
        }
    }
}
