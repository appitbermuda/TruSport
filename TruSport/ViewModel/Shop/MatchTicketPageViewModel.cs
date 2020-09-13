using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class MatchTicketPageViewModel : BaseViewModel
    {
        private ObservableCollection<MatchTicket> _matchTicketCollection;
        private ObservableCollection<Fixture> _fixtureCollection;
        private ObservableCollection<Product> _productCollection;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        MatchTicketService matchTicketService;

        INavigation Navigation;

        public MatchTicketPageViewModel(INavigation navigation)
        {
            matchTicketService = new MatchTicketService();

            GenerateSource();
        }

        public ObservableCollection<MatchTicket> MatchTicketCollection
        {
            get { return _matchTicketCollection; }
            set { this._matchTicketCollection = value; }
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
                MatchTicketCollection = new ObservableCollection<MatchTicket>(matchTickets);

                if (MatchTicketCollection.Count == 0)
                    NoTickets = true;
                
                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Tickets");
            }
        }
    }
}
