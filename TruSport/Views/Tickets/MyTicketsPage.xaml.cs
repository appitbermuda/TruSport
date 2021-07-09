using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class MyTicketsPage : ContentPage
    {
        MyTicketPageViewModel myTicketPageViewModel;

        public MyTicketsPage()
        {
            myTicketPageViewModel = new MyTicketPageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = myTicketPageViewModel;
        }
    }
}
