using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruSport.ViewModel.Shop;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views.Tickets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketTabbedPage : TabbedPage
    {
        TicketTabbedPageViewModel ticketTabbedPageViewModel;

        public TicketTabbedPage()
        {
            ticketTabbedPageViewModel = new TicketTabbedPageViewModel(Navigation);

            this.Children.Add(new PurchasePage());
            this.Children.Add(new ActiveTicketsPage());
            this.Children.Add(new AccountPage());

            InitializeComponent();

            this.BindingContext = ticketTabbedPageViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
