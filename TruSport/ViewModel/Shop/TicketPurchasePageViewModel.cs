using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views.Tickets;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class TicketPurchasePageViewModel : BaseViewModel
    {
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedEventTicketChangedCommand;
        public ObservableCollection<ContactTrace> _contactTraces;
        private ObservableCollection<EventTicket> _eventTicketCollection;
        private ObservableCollection<Product> _productCollection;
        private ObservableCollection<SportEvent> _sportEventCollection;
        private SportEvent _sportEvent;
        private EventTicket _eventTicket;
        private Product _product;
        private Customer _customer;
        private CreditCard _creditCard;
        private string _image;
        private string _discountCode;
        private string _customerName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _eventTicketName;
        private string _label;
        private string _importantMessage;
        private bool _isMember;
        private bool _isGuest;
        private bool _showCardDetail;
        private bool _showContactTracing;
        private bool _showEventSpot;
        private int _quantity;
        private int _memberTicketCount;
        private int _contactTracingHeight;
        private decimal _price;
        private decimal _price2;
        private decimal _subtotal;
        private decimal _total;
        private decimal _processingFee;
        private decimal _processingFeeAmount;
        INavigation Navigation;

        SportEventService sportEventService;
        CustomerTicketService customerTicketService;
        SettingService settingService;
        InventoryService inventoryService;

        public TicketPurchasePageViewModel(INavigation navigation, List<SportEvent> sportEvents, bool isGuest = false)
        {
            Navigation = navigation;
            sportEventService = new SportEventService();
            customerTicketService = new CustomerTicketService();
            settingService = new SettingService();
            inventoryService = new InventoryService();
            ContactTraces = new ObservableCollection<ContactTrace>();
            EventSpotCollection = new ObservableCollection<EventTicket>();

            if (isGuest)
            {
                IsGuest = isGuest;
                GenerateGuestSource(sportEvents);
            }
            else
                GenerateSource(sportEvents);

            PurchaseCommand = new Command(async () => await Purchase());
            UpdateQuantityCommand = new Command(async () => await UpdateQuantity());
            CloseClickedCommand = new Command(async () => await Close());
            AddContactTraceCommand = new Command(async () => await AddContactTrace());
            ContactTracingSelectedCommand = new Command<object>(ContactTracingSelected);

            EventTicketSpotSelectedCommand = new Command(async (obj) => await EventTicketSpotSelected(obj));

            SelectedEventTicketChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(EventTicketSelectionChanged);
        }

        public Command PurchaseCommand { get; set; }
        public Command UpdateQuantityCommand { get; set; }
        public Command CloseClickedCommand { get; set; }
        public Command AddContactTraceCommand { get; set; }
        public Command EventTicketSpotSelectedCommand { get; set; }

        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedEventTicketChangedCommand
        {
            get { return selectedEventTicketChangedCommand; }
            set { selectedEventTicketChangedCommand = value; }
        }

        private Command<Object> _contactTracingSelectedCommand;
        public Command<object> ContactTracingSelectedCommand
        {
            get { return _contactTracingSelectedCommand; }
            set { Set(ref _contactTracingSelectedCommand, value); }
        }

        public ObservableCollection<EventTicket> EventSpotCollection
        {
            get { return _eventTicketCollection; }
            set { Set(ref _eventTicketCollection, value); }
        }

        public ObservableCollection<ContactTrace> ContactTraces
        {
            get { return _contactTraces; }
            set { Set(ref _contactTraces, value); }
        }

        public ObservableCollection<Product> ProductCollection
        {
            get { return _productCollection; }
            set { Set(ref _productCollection, value); }
        }

        public ObservableCollection<SportEvent> SportEvents
        {
            get { return _sportEventCollection; }
            set { Set(ref _sportEventCollection, value); }
        }

        public SportEvent SportEvent
        {
            get { return _sportEvent; }
            set { Set(ref _sportEvent, value); }
        }

        public Product Product
        {
            get { return _product; }
            set { Set(ref _product, value); }
        }

        public EventTicket EventTicket
        {
            get { return _eventTicket; }
            set { Set(ref _eventTicket, value); }
        }

        public Customer Customer
        {
            get { return _customer; }
            set { Set(ref _customer, value); }
        }

        public CreditCard CreditCard
        {
            get { return _creditCard; }
            set { Set(ref _creditCard, value); }
        }

        //public string Image
        //{
        //    get { return _image; }
        //    set { Set(ref _image, value); }
        //}

        public string DiscountCode
        {
            get { return _discountCode; }
            set { Set(ref _discountCode, value); }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set { Set(ref _customerName, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public string ImportantMessage
        {
            get { return _importantMessage; }
            set { Set(ref _importantMessage, value); }
        }

        public string Phone
        {
            get { return _phone; }
            set { Set(ref _phone, value); }
        }

        public string EventTicketName
        {
            get { return _eventTicketName; }
            set { Set(ref _eventTicketName, value); }
        }

        public bool ShowCardDetail
        {
            get { return _showCardDetail; }
            set { Set(ref _showCardDetail, value); }
        }

        public bool ShowContactTracing
        {
            get { return _showContactTracing; }
            set { Set(ref _showContactTracing, value); }
        }

        public bool IsMember
        {
            get { return _isMember; }
            set { Set(ref _isMember, value); }
        }

        public bool IsGuest
        {
            get { return _isGuest; }
            set { Set(ref _isGuest, value); }
        }

        //public int MemberTicketCount
        //{
        //    get { return _memberTicketCount; }
        //    set
        //    {
        //        Set(ref _memberTicketCount, value);
        //    }
        //}

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                Set(ref _quantity, value);
                this.UpdatePrice();
            }
        }

        public int ContactTracingHeight
        {
            get { return _contactTracingHeight; }
            set {
                Set(ref _contactTracingHeight, value);
            }
        }

        public decimal Price2
        {
            get { return _price2; }
            set { Set(ref _price2, value); }
        }

        public decimal Price
        {
            get { return _price; }
            set { Set(ref _price, value); }
        }

        public decimal Subtotal
        {
            get { return _subtotal; }
            set { Set(ref _subtotal, value); }
        }

        public decimal ProcessingFeeAmount
        {
            get { return _processingFeeAmount; }
            set { Set(ref _processingFeeAmount, value); }
        }

        public decimal ProcessingFee
        {
            get { return _processingFee; }
            set { Set(ref _processingFee, value); }
        }

        public decimal Total
        {
            get { return _total; }
            set { Set(ref _total, value); }
        }

        internal async void GenerateSource(List<SportEvent> sportEvents)
        {
            IsBusy = true;

            try
            {
                string Email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(Email);

                if (Customer != null)
                {
                    var currentSportEvents = await sportEventService.GetCurrentSportEvents();
                    currentSportEvents = sportEvents.Where(e => sportEvents.Any(d => d.ID == e.ID)).ToList();
                    currentSportEvents.ForEach(e => e.EventTickets.Sort((x, y) => x.Product.Order - y.Product.Order));

                    ImportantMessage = await settingService.GetImportantMessage();
                    ShowContactTracing = await settingService.ShowContactTracing();

                    CreditCard = new CreditCard();
                    SportEvents = new ObservableCollection<SportEvent>(currentSportEvents);

                    if (SportEvents != null)
                    {
                        foreach (var sportEvent in SportEvents)
                        {
                            sportEvent.EventTickets = sportEvent.EventTickets.OrderBy(e => e.Product.Order).ToList();

                            if (sportEvent.EventTickets.Any(e => e.Product.Age.Contains("Spot #")))
                            {
                                sportEvent.ShowEventSpot = true;
                            }
                            else
                            {
                                sportEvent.ShowEventSpot = false;
                            }
                        }

                        UpdatePrice();
                    }

                    CustomerName = Customer.Name;

                    if (ShowContactTracing)
                    {
                        ContactTraces.Add(new ContactTrace
                        {
                            FirstName = Customer.FirstName,
                            LastName = Customer.LastName,
                            Phone = Customer.Phone
                        });

                        ContactTracingHeight = 40;
                    }
                }
                else
                {
                    await Navigation.PopAsync();

                    SecureStorage.RemoveAll();
                    await App.Database.SignOut();

                    Application.Current.MainPage = (new TicketFlyoutPage());
                }
            }
            catch (Exception ex)
            {
                await Navigation.PopAsync();
                Debug.WriteLine(ex.Message, "Purchase Ticket");
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal async void GenerateGuestSource(List<SportEvent> sportEvents)
        {
            IsBusy = true;

            try
            {
                var currentSportEvents = await sportEventService.GetCurrentSportEvents();
                currentSportEvents = sportEvents.Where(e => sportEvents.Any(d => d.ID == e.ID)).ToList();
                currentSportEvents.ForEach(e => e.EventTickets.Sort((x, y) => x.Product.Order - y.Product.Order));

                ImportantMessage = await settingService.GetImportantMessage();
                ShowContactTracing = await settingService.ShowContactTracing();

                Customer = new Customer();
                CreditCard = new CreditCard();
                SportEvents = new ObservableCollection<SportEvent>(currentSportEvents);
                decimal processingFee = 0.00m;

                if (SportEvents != null)
                {                    
                    foreach (var sportEvent in SportEvents)
                    {
                        sportEvent.EventTickets = sportEvent.EventTickets.OrderBy(e => e.Product.Order).ToList();

                        if (sportEvent.EventTickets.Any(e => e.Product.Age.Contains("Spot #")))
                        {
                            sportEvent.ShowEventSpot = true;
                        }
                        else
                        {
                            sportEvent.ShowEventSpot = false;
                        }
                    }               

                    UpdatePrice();
                }
            }
            catch (Exception ex)
            {
                await Navigation.PopAsync();
                Debug.WriteLine(ex.Message, "Purchase Guest Ticket");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// This method is used to update the price amount.
        /// </summary>
        private void UpdatePrice()
        {
            try
            {
                decimal thisSubtotal = 0.0m;
                decimal thisProcessingFee = 0.0m;

                foreach (var sportEvent in SportEvents)
                {
                    foreach (var eventTicket in sportEvent.EventTickets)
                    {
                        if (eventTicket.ID == sportEvent.SelectedEventSpotID)
                        {
                                if (eventTicket.Product?.Price > 0)
                                    thisProcessingFee += eventTicket.Product.Fee;

                                thisSubtotal += eventTicket.Product.Price;
                        }
                        else if (eventTicket.Quantity >= 0)
                        {
                            
                            if (EventSpotCollection.Any(e => e.ID == eventTicket.ID))
                            {
                                var removeEventTicket = EventSpotCollection.FirstOrDefault(e => e.ID == eventTicket.ID);

                                EventSpotCollection.Remove(removeEventTicket);
                            }

                            if (eventTicket.Quantity > 0)
                            {
                                eventTicket.SportEvent = sportEvent;
                                EventSpotCollection.Add(eventTicket);
                            }
                            
                            if (eventTicket.Product.Price > 0)
                                thisProcessingFee += (eventTicket.Quantity * eventTicket.Product.Fee);

                            thisSubtotal += (eventTicket.Quantity * eventTicket.Product.Price);
                        }
                    }
                }

                this.ProcessingFee = thisProcessingFee;
                this.Subtotal = thisSubtotal;
                this.Total = this.Subtotal + this.ProcessingFee;

                if (this.Total > 0.00m)
                    ShowCardDetail = true;                
                else
                    ShowCardDetail = false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Price");
            }
        }

        async Task Purchase()
        {
            IsBusy = true;
            int inventoryLevel = 0;
            bool spotAvailable = false;
            List<string> NoInventory = new List<string>();
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            try
            {
                if (SportEvents != null && SportEvents.Count > 0 && (CreditCard != null && !String.IsNullOrEmpty(CreditCard.CardNumber) && !String.IsNullOrEmpty(CreditCard.Expiry) && !String.IsNullOrEmpty(CreditCard.CVV))
                    && ((IsGuest && !String.IsNullOrEmpty(Customer.FirstName) && !String.IsNullOrEmpty(Customer.LastName) && !String.IsNullOrEmpty(Customer.Email) && !String.IsNullOrEmpty(Customer.Phone)) || !IsGuest))
                {
                    foreach (var sportEvent in SportEvents)
                    {
                        if (sportEvent.EventTickets.Any(d => d.Product.Age.Contains("Spot #") && d.ID == sportEvent.SelectedEventSpotID))
                        {
                            EventTicket eventTicket = sportEvent.EventTickets.FirstOrDefault(e => e.ID == sportEvent.SelectedEventSpotID);
                            spotAvailable = await inventoryService.CheckIfEventSpotAvailable(eventTicket.ID);
                        }
                        else
                        {
                            inventoryLevel = await inventoryService.EventTicketInventory(sportEvent.ID);
                        }

                        if ((!String.IsNullOrEmpty(sportEvent.SelectedEventSpotID) && !spotAvailable) || (sportEvent.EventTickets.Sum(e => e.Quantity) > inventoryLevel && sportEvent.EventTickets.Sum(e => e.Quantity) <= 0))
                        {
                            if ((!String.IsNullOrEmpty(sportEvent.SelectedEventSpotID) && !spotAvailable))
                            {
                                NoInventory.Add(sportEvent.Title + " - " + sportEvent.EventTickets.FirstOrDefault(e => e.ID == sportEvent.SelectedEventSpotID).Product.Age);
                            }
                            else
                            {
                                NoInventory.Add(sportEvent.Title + " - " + sportEvent.Date.ToString("MMM-dd hh:mm tt"));
                            }
                        }
                        else
                        {
                            foreach (var eventTicket in sportEvent.EventTickets)
                            {
                                if (eventTicket.Quantity > 0 || eventTicket.ID == sportEvent.SelectedEventSpotID)
                                {
                                    int quantity = eventTicket.ID == sportEvent.SelectedEventSpotID ? 1 : eventTicket.Quantity;

                                    orderDetails.Add(new OrderDetail
                                    {
                                        SportEventID = sportEvent.ID,
                                        EventTicketID = eventTicket.ID,
                                        Qty = quantity,
                                        Subtotal = quantity * (DiscountCode == eventTicket.Product.MemberCode ? (eventTicket.Product.MemberPrice ?? eventTicket.Product.Price) : eventTicket.Product.Price),
                                        IsMemberTicket = !String.IsNullOrEmpty(DiscountCode) && (DiscountCode == eventTicket.Product.MemberCode)
                                        //Fee = eventTicket.Quantity * eventTicket.Product.Fee
                                    });
                                }
                            }
                        }
                    }

                    if (NoInventory.Count == 0)
                    {
                        PaymentAuthorize paymentAuthorize = new PaymentAuthorize
                        {
                            NameOnCard = CustomerName ?? Customer.Name,
                            Amount = Convert.ToString(Total),
                            FixtureID = null,
                            CardNumber = CreditCard.CardNumber,
                            Expiry = CreditCard.Expiry,
                            CVV = CreditCard.CVV,
                            OrderDetails = orderDetails.ToList(),
                            Email = Customer.Email
                        };

                        if (ShowContactTracing)
                        {
                            paymentAuthorize.ContactTraces = ContactTraces.ToList();
                        }

                        if (IsGuest)
                        {
                            paymentAuthorize.FirstName = Customer.FirstName;
                            paymentAuthorize.LastName = Customer.LastName;
                            paymentAuthorize.Phone = Customer.Phone;
                        }

                        if (!String.IsNullOrEmpty(ImportantMessage))
                            await App.Current.MainPage.DisplayAlert("Important Message", ImportantMessage, "Okay");

                        bool confirmPayment = await App.Current.MainPage.DisplayAlert("Purchase Ticket", "You will be charged a total of $" + Total, "Purchase", "Cancel");

                        if (confirmPayment)
                        {
                            PaymentResponse paymentResponse = new PaymentResponse();

                            paymentResponse = await customerTicketService.GuestCheckoutPurchase(paymentAuthorize);

                            if (paymentResponse != null)
                            {
                                IsBusy = false;

                                MessagingCenter.Subscribe<PurchaseTicketResultPageViewModel, PaymentResponse>(this, "TicketPurchased", async (objs, product) =>
                                {
                                    MessagingCenter.Unsubscribe<PurchaseTicketResultPageViewModel, PaymentResponse>(this, "TicketPurchased");

                                    if (product.IsApproved)
                                        await Navigation.PopAsync();
                                });

                                await Navigation.PushModalAsync(new PurchaseTicketResultPage(paymentResponse));
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Transaction Error", "Looks like there was an issue processing your payment, please check your card details and try again.", "OK");
                            }
                        }

                    }
                    else
                    {
                        if (NoInventory.Count > 0)
                        {
                            if (NoInventory.Any(e => e.Contains("Spot #")))
                            {
                                await Application.Current.MainPage.DisplayAlert("Not Available", "Unfortunately, this spot is no longer available:" + string.Join(", ", NoInventory), "OK");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Not Available", "Unfortunately, there aren't any tickets left for:" + string.Join(", ", NoInventory), "OK");
                            }
                        }
                    }
                }
                else
                {
                    if (IsGuest && (String.IsNullOrEmpty(Customer.FirstName) || String.IsNullOrEmpty(Customer.LastName) || String.IsNullOrEmpty(Customer.Email) || String.IsNullOrEmpty(Customer.Phone)))
                    {
                        await Application.Current.MainPage.DisplayAlert("Personal Information", "Enter you personal details to receive your ticket.", "OK");
                    }

                    if (ShowCardDetail && CreditCard != null && (String.IsNullOrEmpty(CreditCard.CardNumber) || String.IsNullOrEmpty(CreditCard.Expiry) || String.IsNullOrEmpty(CreditCard.CVV)))
                    {
                        await Application.Current.MainPage.DisplayAlert("Card Details", "Please enter your card details.", "OK");
                    }
                                        
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task UpdateQuantity()
        {
            try
            {
                if(SportEvents.Select(e => e.EventTickets).Count() > 0)
                {
                    UpdatePrice();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Close");
            }
        }

        async Task Close()
        {
            try
            {
                MessagingCenter.Send(this, "ClearTicketList");
                MessagingCenter.Send(this, "TicketListPage");
                await Navigation.PopAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Close");
            }
        }

        async Task AddContactTrace()
        {
            try
            {
                if (!String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName) && !String.IsNullOrEmpty(Phone))
                {
                    if (!(ContactTraces.Count > 0 && ContactTraces.Any(e => e.FirstName == FirstName && e.LastName == LastName) || ContactTraces.Any(e => e.FirstName.Contains(FirstName) && e.LastName.Contains(LastName)) || ContactTraces.Any(e => FirstName.Contains(e.FirstName) && LastName.Contains(e.LastName))))
                    {
                        ContactTraces.Add(new ContactTrace
                        {
                            FirstName = FirstName,
                            LastName = LastName,
                            Phone = Phone
                        });

                        FirstName = String.Empty;
                        LastName = String.Empty;
                        Phone = String.Empty;

                        ContactTracingHeight = 40 * ContactTraces.Count;
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Contact Tracing", "You have already entered this name.", "OK");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Contact Tracing", "All fields are required.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Contact Trace");
            }
        }

        private async void ContactTracingSelected(object obj)
        {
            try
            {
                bool removeContactTrace = await Application.Current.MainPage.DisplayAlert("Contact Tracing", "Are you sure you want to remove this contact?", "Yes", "Cancel");

                if (removeContactTrace)
                {
                    var listView = obj as SfListView;
                    var contactTrace = listView.SelectedItem as ContactTrace;

                    ContactTraces.Remove(contactTrace);

                    ContactTracingHeight = 40 * ContactTraces.Count;
                    //DisplayAlert("Message", (listView.SelectedItem as SportEvent).ContactName + " is selected", "OK");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Remove Contact Tracing");
            }
        }

        private async Task EventTicketSpotSelected(object obj)
        {
            try
            {
                var selectedSportEvent = obj as SportEvent;

                MessagingCenter.Subscribe<TicketSpotListPageViewModel, EventTicket>(this, "SelectedEventSpot", async (objs, selectedSpot) =>
                {
                    MessagingCenter.Unsubscribe<TicketSpotListPageViewModel, EventTicket>(this, "SelectedEventSpot");

                    if (selectedSpot != null)
                    {
                        foreach (var sportEvent in SportEvents)
                        {
                            if (sportEvent.ID == selectedSpot.SportEventID)
                            {
                                sportEvent.SelectedEventSpotID = selectedSpot.ID;
                                selectedSpot.Quantity = 1;

                                if (EventSpotCollection.Any(e => e.SportEventID == selectedSpot.SportEventID))
                                {
                                    var eventTicket = EventSpotCollection.FirstOrDefault(e => e.SportEventID == selectedSpot.SportEventID);

                                    EventSpotCollection.Remove(eventTicket);
                                }

                                EventSpotCollection.Add(selectedSpot);

                                await Application.Current.MainPage.DisplayAlert("Spot Added", selectedSpot.Product.Age + " has been added to Order Summary below.", "OK");

                                UpdatePrice();
                                break;
                            }
                        }
                    }
                });


                await Navigation.PushModalAsync(new TicketSpotListPage(selectedSportEvent.ID));
                    //DisplayAlert("Message", (listView.SelectedItem as SportEvent).ContactName + " is selected", "OK");
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "EventTicketSpotSelected");
            }
        }

        private void EventTicketSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            try
            {
                var product = e.NewValue as Product;

                Product = product;
                EventTicketName = Product.Age;

                UpdatePrice();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
