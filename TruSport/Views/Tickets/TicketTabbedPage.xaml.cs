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
        AppLinkEntry appLinkEntry;

        public TicketTabbedPage()
        {
            ticketTabbedPageViewModel = new TicketTabbedPageViewModel(Navigation);

            this.Children.Add(new PurchasePage());
            this.Children.Add(new ActiveTicketsPage());
            this.Children.Add(new AccountPage());

            InitializeComponent();

            BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"];
            BarTextColor = (Color)App.Current.Resources["navTextColor"];

            this.BindingContext = ticketTabbedPageViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            appLinkEntry = new AppLinkEntry
            {
                AppLinkUri = new Uri(Constants.ApplicationTicketURL),
                Description = "ONTRACK Match Ticketing",
                Title = "ONTRACK Tickets",
                IsLinkActive = true                
            };

            Application.Current.AppLinks.RegisterLink(appLinkEntry);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            appLinkEntry.IsLinkActive = false;
            Application.Current.AppLinks.RegisterLink(appLinkEntry);
        }
    }
}
