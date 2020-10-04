using System;
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
        private FixtureProduct _fixtureProduct;
        private Customer _customer;
        private CreditCard _creditCard;
        private string _customerName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private int _quantity;
        private int _contactTracingHeight;
        private decimal _price;
        private decimal _subtotal;
        private decimal _total;
        private decimal _processingFee;
        private decimal _processingFeeAmount;
        INavigation Navigation;

        MatchTicketService matchTicketService;
        SettingService settingService;
        InventoryService inventoryService;

        public PurchaseTicketPageViewModel(INavigation navigation, FixtureProduct fixtureProduct)
        {
            Navigation = navigation;
            matchTicketService = new MatchTicketService();
            settingService = new SettingService();
            inventoryService = new InventoryService();
            ContactTraces = new ObservableCollection<ContactTrace>();

            GenerateSource(fixtureProduct);

            PurchaseCommand = new Command(async () => await Purchase());
            CloseClickedCommand = new Command(async () => await Close());
            AddContactTraceCommand = new Command(async () => await AddContactTrace());
            ContactTracingSelectedCommand = new Command<object>(ContactTracingSelected);
        }

        public Command PurchaseCommand { get; set; }
        public Command CloseClickedCommand { get; set; }
        public Command AddContactTraceCommand { get; set; }

        private Command<Object> _contactTracingSelectedCommand;
        public Command<object> ContactTracingSelectedCommand
        {
            get { return _contactTracingSelectedCommand; }
            set { Set(ref _contactTracingSelectedCommand, value); }
        }

        public ObservableCollection<ContactTrace> ContactTraces
        {
            get { return _contactTraces; }
            set { Set(ref _contactTraces, value); }
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

        internal async void GenerateSource(FixtureProduct fixtureProduct)
        {
            IsBusy = true;
            try
            {
                string Email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(Email);

                if (Customer != null)
                {
                    CreditCard = new CreditCard();
                    FixtureProduct = fixtureProduct;

                    decimal? processingFee = await settingService.GetProcessingFee();

                    CustomerName = Customer.Name;
                    Quantity = 1;
                    Price = fixtureProduct.Product.Price;
                    Subtotal = Price;
                    ProcessingFeeAmount = processingFee ?? 0.00m;
                    ProcessingFee = ProcessingFeeAmount;
                    Total = (Quantity * Price) + ProcessingFee;
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
                this.ProcessingFee = (this.Quantity * this.ProcessingFeeAmount);
                this.Subtotal = (this.Quantity * this.Price);
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
                if (Quantity <= ContactTraces.Count)
                {
                    if (CreditCard != null && !String.IsNullOrEmpty(CreditCard.CardNumber) && !String.IsNullOrEmpty(CreditCard.Expiry) && !String.IsNullOrEmpty(CreditCard.CVV))
                    {
                        PaymentAuthorize paymentAuthorize = new PaymentAuthorize
                        {
                            NameOnCard = CustomerName,
                            CardNumber = CreditCard.CardNumber,
                            Expiry = CreditCard.Expiry,
                            CVV = CreditCard.CVV,
                            Quantity = Quantity,
                            Amount = Convert.ToString(Total),
                            FixtureProductID = FixtureProduct.ID,
                            CustomerID = Customer.ID,
                            ContactTraces = ContactTraces.ToList()
                        };

                        var hasStock = await inventoryService.CheckInventory(FixtureProduct.ProductID);

                        if (hasStock)
                        {
                            PaymentResponse paymentResponse = await matchTicketService.Purchase(paymentAuthorize);

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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase");
            }
            finally
            {
                IsBusy = false;
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
                //if (Quantity > ContactTraces.Count)
                //{
                    if (!String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName) && !String.IsNullOrEmpty(Phone))
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
                    }

                    ContactTracingHeight = 40 * ContactTraces.Count;
                //}
                //else
                //    await Application.Current.MainPage.DisplayAlert("Contact Tracing", "You have reached the number of tickets you have selected", "OK");
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
