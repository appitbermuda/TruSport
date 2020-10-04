using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class ActiveTicketsPage : ContentPage
    {
        MyTicketPageViewModel myTicketPageViewModel;

        public ActiveTicketsPage()
        {
            myTicketPageViewModel = new MyTicketPageViewModel(Navigation);

            this.BindingContext = myTicketPageViewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Send<ActiveTicketsPage>(this, "Refresh");
        }
    }
}
