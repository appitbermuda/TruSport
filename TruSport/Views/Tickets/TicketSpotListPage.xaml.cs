using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TicketSpotListPage : ContentPage
    {
        TicketSpotListPageViewModel ticketSpotListPageViewModel;

        public TicketSpotListPage(string sportEventID)
        {
            ticketSpotListPageViewModel = new TicketSpotListPageViewModel(Navigation, sportEventID);

            InitializeComponent();

            this.BindingContext = ticketSpotListPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void TicketSpotsList_SelectionChanged(System.Object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
        }

        void TicketSpotsList_SelectionChanging(System.Object sender, Syncfusion.ListView.XForms.ItemSelectionChangingEventArgs e)
        {
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
        }
    }
}
