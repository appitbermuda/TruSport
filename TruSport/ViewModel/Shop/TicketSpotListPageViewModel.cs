using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.Tickets;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class TicketSpotListPageViewModel : BaseViewModel
    {
        private ObservableCollection<EventTicket> _eventTicketCollection;
        private EventTicket _eventTicket;
        private SportEvent _sportEvent;
        private bool _noTickets;
        private string _image;
        private string _title;
        private bool _isActivityIndicatorVisible;

        SportEventService sportEventService;
        InventoryService inventoryService;
        AdService adService;

        INavigation Navigation;

        public TicketSpotListPageViewModel(INavigation navigation, string sportEventID)
        {
            Navigation = navigation;
            sportEventService = new SportEventService();
            inventoryService = new InventoryService();

            EventTicketCollection = new ObservableCollection<EventTicket>();
            EventTicket = new EventTicket();

            GenerateSource(sportEventID);

            CloseClickedCommand = new Command(async () => await Close());
            TicketSelectedCommand = new Command<object>(TicketSelected);
        }

        public Command CloseClickedCommand { get; set; }
        public Command CheckoutCommand { get; set; }
        public Command AdTappedCommand { get; }
        private Command<Object> _ticketSelectedCommand;
        public Command<object> TicketSelectedCommand
        {
            get { return _ticketSelectedCommand; }
            set { Set(ref _ticketSelectedCommand, value); }
        }

        public ObservableCollection<EventTicket> EventTicketCollection
        {
            get { return _eventTicketCollection; }
            set { Set(ref _eventTicketCollection, value); }
        }

        public SportEvent SportEvent
        {
            get { return _sportEvent; }
            set { Set(ref _sportEvent, value); }
        }

        public EventTicket EventTicket
        {
            get { return _eventTicket; }
            set { Set(ref _eventTicket, value); }
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public string Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
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

        //internal async void GenerateSource(List<EventTicket> eventTickets)
        //{
        //    try
        //    {
        //        NoTickets = false;
        //        IsActivityIndicatorVisible = true;

        //        if (eventTickets != null)
        //        {
        //            EventTicketCollection = new ObservableCollection<EventTicket>(eventTickets);

        //            if (EventTicketCollection.Count == 0)
        //                NoTickets = true;
        //        }
        //        else
        //            NoTickets = true;
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Match Tickets");
        //    }
        //    finally
        //    {

        //        IsActivityIndicatorVisible = false;
        //    }
        //}

        internal async void GenerateSource(string sportEventID)
        {
            try
            {
                NoTickets = false;
                IsActivityIndicatorVisible = true;

                var eventTickets = await sportEventService.GetSportEventTickets(sportEventID, null);

                if (eventTickets != null)
                {
                    Image = eventTickets.FirstOrDefault().Product.Image;
                    SportEvent = eventTickets.FirstOrDefault().SportEvent;

                    eventTickets = eventTickets.OrderBy(e => e.Product.Order).ToList();
                    EventTicketCollection = new ObservableCollection<EventTicket>(eventTickets);

                    if (EventTicketCollection.Count == 0)
                        NoTickets = true;
                }
                else
                    NoTickets = true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Tickets");
            }
            finally
            {

                IsActivityIndicatorVisible = false;
            }
        }

        private async void TicketSelected(object obj)
        {
            try
            {
                var listView = obj as SfListView;
                var eventTicket = listView.SelectedItem as EventTicket;

                if (eventTicket != null)
                {
                    //MessagingCenter.Subscribe<TicketPurchasePageViewModel>(this, "TicketListPage", async (objs) =>
                    //{
                    //    //Purchase tickets saved to local
                    //    //var creditCards = await App.Database.T(Customer.Email);
                    //    GenerateSource();
                    //});

                    var spotAvailable = await inventoryService.CheckIfEventSpotAvailable(eventTicket.ID);

                    if (spotAvailable)
                    {

                        MessagingCenter.Send<TicketSpotListPageViewModel,EventTicket>(this, "SelectedEventSpot", eventTicket);
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Spot Taken", "It looks like this spot is no longer available, please select a different spot.", "OK");
                        GenerateSource(eventTicket.SportEventID);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task Close()
        {
            try
            {
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Close");
            }
        }
    }
}
