using System;
using System.Collections.Generic;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class OrderHistoryPage : ContentPage
    {
        OrderHistoryPageViewModel orderHistoryPageViewModel;

        public OrderHistoryPage()
        {
            orderHistoryPageViewModel = new OrderHistoryPageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = orderHistoryPageViewModel;
        }
    }
}
