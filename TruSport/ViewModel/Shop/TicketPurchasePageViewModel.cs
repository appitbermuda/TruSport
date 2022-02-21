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
        public ObservableCollection<ContactTrace> _contactTraces;
        private ObservableCollection<EventTicket> _eventTicketCollection;
        private SportEvent _sportEvent;
        private EventTicket _eventTicket;
        private Customer _customer;
        private CreditCard _creditCard;
        private string _customerName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _importantMessage;
        private bool _isMember;
        private bool _showCardDetail;
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

        public TicketPurchasePageViewModel(INavigation navigation, SportEvent sportEvent)
        {
            Navigation = navigation;
            sportEventService = new SportEventService();
            customerTicketService = new CustomerTicketService();
            settingService = new SettingService();
            inventoryService = new InventoryService();
            ContactTraces = new ObservableCollection<ContactTrace>();
            EventTicketCollection = new ObservableCollection<EventTicket>();

            GenerateSource(sportEvent);

            PurchaseCommand = new Command(async () => await Purchase());
            UpdateQuantityCommand = new Command(async () => await UpdateQuantity());
            CloseClickedCommand = new Command(async () => await Close());
            AddContactTraceCommand = new Command(async () => await AddContactTrace());
            ContactTracingSelectedCommand = new Command<object>(ContactTracingSelected);
        }

        public Command PurchaseCommand { get; set; }
        public Command UpdateQuantityCommand { get; set; }
        public Command CloseClickedCommand { get; set; }
        public Command AddContactTraceCommand { get; set; }

        private Command<Object> _contactTracingSelectedCommand;
        public Command<object> ContactTracingSelectedCommand
        {
            get { return _contactTracingSelectedCommand; }
            set { Set(ref _contactTracingSelectedCommand, value); }
        }

        public ObservableCollection<EventTicket> EventTicketCollection
        {
            get { return _eventTicketCollection; }
            set { Set(ref _eventTicketCollection, value); }
        }

        public ObservableCollection<ContactTrace> ContactTraces
        {
            get { return _contactTraces; }
            set { Set(ref _contactTraces, value); }
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

        public bool ShowCardDetail
        {
            get { return _showCardDetail; }
            set { Set(ref _showCardDetail, value); }
        }

        public bool IsMember
        {
            get { return _isMember; }
            set { Set(ref _isMember, value); }
        }

        public int MemberTicketCount
        {
            get { return _memberTicketCount; }
            set
            {
                Set(ref _memberTicketCount, value);
            }
        }

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

        internal async void GenerateSource(SportEvent sportEvent)
        {
            IsBusy = true;
            try
            {
                string Email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(Email);

                if (Customer != null)
                {
                    var eventTickets = await sportEventService.GetSportEventTickets(sportEvent.ID, Email);

                    ImportantMessage = await settingService.GetImportantMessage();

                    CreditCard = new CreditCard();
                    SportEvent = sportEvent;
                    decimal processingFee = 0.00m;

                    if (eventTickets != null && eventTickets.Count > 0)
                    {
                        IsMember = eventTickets.Any(e => e.Product.Age.Contains("Member"));


                        if (IsMember)
                        {
                            MemberTicketCount = eventTickets?.FirstOrDefault(e => e.Product.Age.Contains("Member"))?.Product?.MemberTicketCount ?? 0;
                            eventTickets.ForEach(e => e.Quantity = e.Product.Age.Contains("Member") ? 1 : 0);
                        }
                        else
                            eventTickets.ForEach(e => e.Quantity = e.Product.Age == "Adult" ? 1 : 0);

                        EventTicketCollection = new ObservableCollection<EventTicket>(eventTickets.ToList());

                        UpdatePrice();
                    }

                    CustomerName = Customer.Name;

                    ContactTraces.Add(new ContactTrace
                    {
                        FirstName = Customer.FirstName,
                        LastName = Customer.LastName,
                        Phone = Customer.Phone
                    });

                    ContactTracingHeight = 40;
                }
                else
                {
                    await Navigation.PopModalAsync();

                    SecureStorage.RemoveAll();
                    await App.Database.SignOut();

                    Application.Current.MainPage = (new TicketFlyoutPage());
                }
            }
            catch(Exception ex)
            {
                await Navigation.PopModalAsync();
                Debug.WriteLine(ex.Message, "Purchase Ticket");
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

                //check if any zero $ tickets

                //int zeroDollar = this.EventTicketCollection.Where(e => e.Product.Price == 0).Sum(e => e.Quantity);
                //this.ProcessingFeeAmount = 
                //this.ProcessingFee = ((this.EventTicketCollection.Sum(e => e.Quantity) - zeroDollar) * this.ProcessingFeeAmount);


                foreach (var eventTicket in EventTicketCollection)
                {
                    if (eventTicket.Quantity > 0)
                    {
                            if (eventTicket.Product.Price > 0)
                                thisProcessingFee += (eventTicket.Quantity * eventTicket.Product.Fee);

                            thisSubtotal += (eventTicket.Quantity * eventTicket.Product.Price);
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
            try
            {
                await UpdateQuantity();
                var inventoryLevel = await inventoryService.EventTicketInventory(SportEvent.ID);

                if (EventTicketCollection.Sum(e => e.Quantity) <= inventoryLevel)
                {
                    if (EventTicketCollection.Sum(e => e.Quantity) > 0 && EventTicketCollection.Sum(e => e.Quantity) <= ContactTraces.Count)
                    {
                        if ((ShowCardDetail && CreditCard != null && !String.IsNullOrEmpty(CreditCard.CardNumber) && !String.IsNullOrEmpty(CreditCard.Expiry) && !String.IsNullOrEmpty(CreditCard.CVV)) || !ShowCardDetail)
                        {
                            List<OrderDetail> orderDetails = new List<OrderDetail>();

                            foreach (var eventTicket in EventTicketCollection)
                            {
                                if (eventTicket.Quantity > 0)
                                {
                                    orderDetails.Add(new OrderDetail
                                    {
                                        EventTicketID = eventTicket.ID,
                                        Qty = eventTicket.Quantity,
                                        Subtotal = eventTicket.Quantity * eventTicket.Product.Price,
                                        IsMemberTicket = eventTicket.Product.Age.Contains("Member")
                                        //Fee = eventTicket.Quantity * eventTicket.Product.Fee
                                    });
                                }
                            }

                            PaymentAuthorize paymentAuthorize = new PaymentAuthorize
                            {
                                NameOnCard = CustomerName,
                                Quantity = EventTicketCollection.Sum(e => e.Quantity),
                                Amount = Convert.ToString(Total),
                                FixtureID = null,
                                //CustomerID = Customer.ID,
                                ContactTraces = ContactTraces.ToList(),
                                OrderDetails = orderDetails.ToList(),
                                SportEventID = SportEvent.ID
                            };

                            if (ShowCardDetail)
                            {
                                paymentAuthorize.CardNumber = CreditCard.CardNumber;
                                paymentAuthorize.Expiry = CreditCard.Expiry;
                                paymentAuthorize.CVV = CreditCard.CVV;
                            }

                            var hasStock = await inventoryService.CheckEventTicketInventory(SportEvent.ID);

                            if (hasStock)
                            {
                                if(!String.IsNullOrEmpty(ImportantMessage))
                                    await App.Current.MainPage.DisplayAlert("Important Message", ImportantMessage, "Okay");
                                
                                bool confirmPayment = await App.Current.MainPage.DisplayAlert("Purchase Ticket", "You will be charged a total of " + Total.ToString("C"), "Purchase", "Cancel");

                                if (confirmPayment)
                                {
                                    PaymentResponse paymentResponse = new PaymentResponse();

                                    if (ShowCardDetail)
                                    {
                                        //paymentResponse = await customerTicketService.PurchaseTest(paymentAuthorize);
                                        paymentResponse = await customerTicketService.Purchase(paymentAuthorize);
                                    }
                                    else
                                    {
                                        //paymentResponse = await customerTicketService.ZeroPurchaseTest(paymentAuthorize);
                                        paymentResponse = await customerTicketService.ZeroPurchase(paymentAuthorize);
                                    }

                                    if (paymentResponse != null)
                                    {
                                        IsBusy = false;

                                        MessagingCenter.Subscribe<PurchaseTicketResultPageViewModel, PaymentResponse>(this, "TicketPurchased", async (objs, product) =>
                                        {
                                            MessagingCenter.Unsubscribe<PurchaseTicketResultPageViewModel, PaymentResponse>(this, "TicketPurchased");

                                            if (product.IsApproved)
                                                await Navigation.PopModalAsync();
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
                                await Application.Current.MainPage.DisplayAlert("Out of Stock", "Sorry, there are no more tickets left for purchase.", "OK");
                                await Navigation.PopModalAsync();
                            }

                        }
                        else
                            await Application.Current.MainPage.DisplayAlert("Card Details", "Please enter your card details.", "OK");
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Contact Tracing", "You must provide the names for the match tickets for contact tracing.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Available Tickets", "Sorry, there " + (inventoryLevel == 0 ? "are no tickets" : inventoryLevel == 1 ? "is only 1 ticket" : "are only " + inventoryLevel + " tickets") + " available.", "OK");
                    await Navigation.PopModalAsync();
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
                if(EventTicketCollection.Count > 0)
                {

                    //Quantity2 = EventTicketCollection.Sum(e => e.Quantity);
                    //Price2 = EventTicketCollection.FirstOrDefault().Product.Price;

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
                MessagingCenter.Send(this, "TicketListPage");
                await Navigation.PopModalAsync(true);
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
    }
}
