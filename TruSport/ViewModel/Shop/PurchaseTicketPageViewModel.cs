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
    public class PurchaseTicketPageViewModel : BaseViewModel
    {
        public ObservableCollection<ContactTrace> _contactTraces;
        private ObservableCollection<FixtureProduct> _fixtureProductCollection;
        private Fixture _fixture;
        private FixtureProduct _fixtureProduct;
        private FixtureProduct _fixtureProductTwo;
        private Customer _customer;
        private CreditCard _creditCard;
        private string _fixtureProductTwoName;
        private string _customerName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private int _quantity;
        private int _quantity2;
        private int _contactTracingHeight;
        private decimal _price;
        private decimal _price2;
        private decimal _subtotal;
        private decimal _total;
        private decimal _processingFee;
        private decimal _processingFeeAmount;
        INavigation Navigation;

        FixtureProductService fixtureProductService;
        MatchTicketService matchTicketService;
        SettingService settingService;
        InventoryService inventoryService;

        public PurchaseTicketPageViewModel(INavigation navigation, Fixture fixture)
        {
            Navigation = navigation;
            fixtureProductService = new FixtureProductService();
            matchTicketService = new MatchTicketService();
            settingService = new SettingService();
            inventoryService = new InventoryService();
            ContactTraces = new ObservableCollection<ContactTrace>();
            FixtureProductCollection = new ObservableCollection<FixtureProduct>();

            GenerateSource(fixture);

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

        public ObservableCollection<FixtureProduct> FixtureProductCollection
        {
            get { return _fixtureProductCollection; }
            set { Set(ref _fixtureProductCollection, value); }
        }

        public ObservableCollection<ContactTrace> ContactTraces
        {
            get { return _contactTraces; }
            set { Set(ref _contactTraces, value); }
        }

        public Fixture Fixture
        {
            get { return _fixture; }
            set { Set(ref _fixture, value); }
        }

        public FixtureProduct FixtureProductTwo
        {
            get { return _fixtureProductTwo; }
            set { Set(ref _fixtureProductTwo, value); }
        }

        public FixtureProduct FixtureProduct
        {
            get { return _fixtureProduct; }
            set { Set(ref _fixtureProduct, value); }
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

        public string FixtureProductTwoName
        {
            get { return _fixtureProductTwoName; }
            set { Set(ref _fixtureProductTwoName, value); }
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

        public string Phone
        {
            get { return _phone; }
            set { Set(ref _phone, value); }
        }

        public int Quantity2
        {
            get { return _quantity2; }
            set
            {
                Set(ref _quantity2, value);
                this.UpdatePrice();
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

        internal async void GenerateSource(Fixture fixture)
        {
            IsBusy = true;
            try
            {
                string Email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(Email);

                if (Customer != null)
                {
                    var fixtureProducts = await fixtureProductService.GetFixtureFixtureProducts(fixture.ID);

                    CreditCard = new CreditCard();
                    Fixture = fixture;
                    decimal? processingFee = await settingService.GetProcessingFee();
                    ProcessingFeeAmount = processingFee ?? 0.00m;

                    if (fixtureProducts != null && fixtureProducts.Count > 0)
                    {
                        fixtureProducts.ForEach(e => e.Quantity = e.Product.Age == "Adult" ? 1 : 0);

                        FixtureProductCollection = new ObservableCollection<FixtureProduct>(fixtureProducts.ToList());

                        UpdatePrice();
                        //FixtureProductCollection = new ObservableCollection<FixtureProduct>(fixtureProducts.Where(e => e.ID != fixtureProduct.ID).ToList());
                    }

                    

                    

                    CustomerName = Customer.Name;
                    //Quantity = 1;
                    //Price = fixtureProduct.Product.Price;
                    //Subtotal = Price;
                    
                    //ProcessingFee = ProcessingFeeAmount;
                    //Total = (Quantity * Price) + ProcessingFee;

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
                }
            }
            catch(Exception ex)
            {
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
                this.ProcessingFee = (this.FixtureProductCollection.Sum(e => e.Quantity) * this.ProcessingFeeAmount);

                foreach (var fixtureProduct in FixtureProductCollection)
                {
                    if (fixtureProduct.Quantity > 0)
                    {
                        thisSubtotal += fixtureProduct.Quantity * fixtureProduct.Product.Price;
                    }
                }

                this.Subtotal = thisSubtotal;
                this.Total = this.Subtotal + this.ProcessingFee;
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
                var inventoryLevel = await inventoryService.TicketInventory(Fixture.ID);

                if (FixtureProductCollection.Sum(e => e.Quantity) <= inventoryLevel)
                {
                    if (FixtureProductCollection.Sum(e => e.Quantity) > 0 && FixtureProductCollection.Sum(e => e.Quantity) <= ContactTraces.Count)
                    {
                        if (CreditCard != null && !String.IsNullOrEmpty(CreditCard.CardNumber) && !String.IsNullOrEmpty(CreditCard.Expiry) && !String.IsNullOrEmpty(CreditCard.CVV))
                        {
                            List<OrderDetail> orderDetails = new List<OrderDetail>();

                            foreach (var fixtureProduct in FixtureProductCollection)
                            {
                                if (fixtureProduct.Quantity > 0)
                                {
                                    orderDetails.Add(new OrderDetail
                                    {
                                        FixtureProductID = fixtureProduct.ID,
                                        Qty = fixtureProduct.Quantity,
                                        Subtotal = fixtureProduct.Quantity * fixtureProduct.Product.Price
                                    });
                                }
                            }

                            PaymentAuthorize paymentAuthorize = new PaymentAuthorize
                            {
                                NameOnCard = CustomerName,
                                CardNumber = CreditCard.CardNumber,
                                Expiry = CreditCard.Expiry,
                                CVV = CreditCard.CVV,
                                Quantity = FixtureProductCollection.Sum(e => e.Quantity),
                                Amount = Convert.ToString(Total),
                                FixtureID = Fixture.ID,
                                CustomerID = Customer.ID,
                                ContactTraces = ContactTraces.ToList(),
                                OrderDetails = orderDetails.ToList()
                            };

                            var hasStock = await inventoryService.CheckInventory(Fixture.ID);

                            if (hasStock)
                            {
                                bool confirmPayment = await App.Current.MainPage.DisplayAlert("Purchase Ticket", "You will be charged a total of " + Total.ToString("C"), "Purchase", "Cancel");

                                if (confirmPayment)
                                {
                                    PaymentResponse paymentResponse = await matchTicketService.Purchase(paymentAuthorize);

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
                if(FixtureProductCollection.Count > 0)
                {

                    //Quantity2 = FixtureProductCollection.Sum(e => e.Quantity);
                    //Price2 = FixtureProductCollection.FirstOrDefault().Product.Price;

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
                MessagingCenter.Send(this, "MatchTicketPage");
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
                    if (!(ContactTraces.Count > 0 && ContactTraces.Any(e => e.FirstName == FirstName && e.LastName == LastName)))
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
                    //DisplayAlert("Message", (listView.SelectedItem as Fixture).ContactName + " is selected", "OK");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Remove Contact Tracing");
            }
        }
    }
}
