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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            try
            {
                base.OnDisappearing();

                MatchTicketsList.SelectedItems.Clear();
                CheckoutButton.IsVisible = false;
            }
            catch(Exception ex)
            {

            }
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.MainPage is FlyoutPage mdp)
            {
                mdp.IsPresented = true;
            }
        }

        void MatchTicketsList_SelectionChanged(System.Object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (MatchTicketsList.SelectedItems.Count > 0)
                {
                    MessagingCenter.Unsubscribe<TicketListPage, string>(this, "ClearTicketList");
                    MessagingCenter.Subscribe<TicketListPage>(this, "ClearTicketList", async (obj) =>
                    {
                        MatchTicketsList.SelectedItems.Clear();
                        CheckoutButton.IsVisible = false;
                    });

                    CheckoutButton.Text = String.Format("Checkout ({0})", MatchTicketsList.SelectedItems.Count);
                    CheckoutButton.IsVisible = true;
                }
                else
                    CheckoutButton.IsVisible = false;
            }
            catch(Exception ex)
            {

            }
        }
    }
}
