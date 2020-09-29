using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views.Tickets;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class MatchTicketPageViewModel : BaseViewModel
    {

        private ObservableCollection<FixtureProduct> _matchTicketCollection;
        private ObservableCollection<Fixture> _fixtureCollection;
        private ObservableCollection<Product> _productCollection;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        MatchTicketService matchTicketService;

        INavigation Navigation;

        public MatchTicketPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            matchTicketService = new MatchTicketService();
            MatchTicketCollection = new ObservableCollection<FixtureProduct>();

            GenerateSource();

            TicketSelectedCommand = new Command<object>(TicketSelected);
        }

        private Command<Object> ticketSelectedCommand;
        public Command<object> TicketSelectedCommand
        {
            get { return ticketSelectedCommand; }
            set { Set(ref ticketSelectedCommand, value); }
        }

        public ObservableCollection<FixtureProduct> MatchTicketCollection
        {
            get { return _matchTicketCollection; }
            set { Set(ref _matchTicketCollection, value); }
        }

        public bool NoTickets
        {
            get { return _noTickets; }
            set { Set(ref _noTickets, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            try
            {
                NoTickets = false;
                IsActivityIndicatorVisible = true;

                var matchTickets = await matchTicketService.GetMatchTickets();
                MatchTicketCollection = new ObservableCollection<FixtureProduct>(matchTickets);

                if (MatchTicketCollection.Count == 0)
                    NoTickets = true;
                
                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Tickets");
            }
        }

        private async void TicketSelected(object obj)
        {
            var listView = obj as SfListView;
            var fixtureProduct = listView.SelectedItem as FixtureProduct;

            MessagingCenter.Subscribe<CreditCardPageViewModel, FixtureProduct>(this, "TicketPurchased", async (objs, product) =>
            {
                //Purchase tickets saved to local
                //var creditCards = await App.Database.T(Customer.Email);
                GenerateSource();
            });

            await Navigation.PushModalAsync(new PurchaseTicketPage(fixtureProduct));
            //DisplayAlert("Message", (listView.SelectedItem as Fixture).ContactName + " is selected", "OK");
        }
    }
}
