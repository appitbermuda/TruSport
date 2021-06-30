using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TicketListPage : ContentPage
    {
        TicketListPageViewModel ticketListPageViewModel;

        public TicketListPage()
        {
            ticketListPageViewModel = new TicketListPageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = ticketListPageViewModel;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.MainPage is FlyoutPage mdp)
            {
                mdp.IsPresented = true;
            }
        }
    }
}
