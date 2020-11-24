using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views.Tickets;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class MatchTicketPageViewModel : BaseViewModel
    {
        private ObservableCollection<AcceptTransfer> _transferRequestCollection;
        private ObservableCollection<Fixture> _matchTicketCollection;
        private ObservableCollection<Fixture> _fixtureCollection;
        private ObservableCollection<Product> _productCollection;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        FixtureProductService fixtureProductService;
        InventoryService inventoryService;
        AdService adService;

        INavigation Navigation;

        public MatchTicketPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            fixtureProductService = new FixtureProductService();
            inventoryService = new InventoryService();
            adService = new AdService();
            MatchTicketCollection = new ObservableCollection<Fixture>();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
            TicketSelectedCommand = new Command<object>(TicketSelected);

            MessagingCenter.Unsubscribe<PurchasePage, string>(this, "Refresh");
            MessagingCenter.Subscribe<PurchasePage>(this, "Refresh", async (obj) =>
            {
                GenerateSource();
            });
        }

        public Command AdTappedCommand { get; }
        private Command<Object> ticketSelectedCommand;
        public Command<object> TicketSelectedCommand
        {
            get { return ticketSelectedCommand; }
            set { Set(ref ticketSelectedCommand, value); }
        }

        public ObservableCollection<Fixture> MatchTicketCollection
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

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        internal async void GenerateSource()
        {
            try
            {
                NoTickets = false;
                IsActivityIndicatorVisible = true;

                await Task.Run(async () =>
                {
                    var ads = await adService.GetAds();

                    if (ads != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Ad = ads.Any(e => e.Sport == Constants.Bowling) ? ads.FirstOrDefault(e => e.Sport == Constants.Bowling) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                        });
                    }
                });

                var matchTickets = await fixtureProductService.GetProducts();
                MatchTicketCollection = new ObservableCollection<Fixture>(matchTickets);

                if (MatchTicketCollection.Count == 0)
                    NoTickets = true;
                
                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Tickets");
            }
        }

        private async void AdTapped()
        {
            try
            {
                await Task.Run(async () =>
                {
                    await adService.Impressions(Ad.ID);
                });

                await Launcher.OpenAsync(new Uri(Ad.URL));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Ad Tapped");
            }
        }

        private async void TicketSelected(object obj)
        {
            var listView = obj as SfListView;
            var fixture = listView.SelectedItem as Fixture;

            MessagingCenter.Subscribe<PurchaseTicketPageViewModel>(this, "MatchTicketPage", async (objs) =>
            {
                //Purchase tickets saved to local
                //var creditCards = await App.Database.T(Customer.Email);
                GenerateSource();
            });

            var hasStock = await inventoryService.CheckTicketInventory(fixture.ID);

            if (hasStock)
            {
                await Navigation.PushModalAsync(new PurchaseTicketPage(fixture));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Out of Stock", "Sorry, there are no more tickets left for purchase.", "OK");
            }
        }
    }
}
