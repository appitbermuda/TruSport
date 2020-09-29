using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private int _quantity;
        private decimal _price;
        private decimal _total;
        private decimal _processingFee;
        INavigation Navigation;

        MatchTicketService matchTicketService;
        SettingService settingService;

        public PurchaseTicketPageViewModel(INavigation navigation, FixtureProduct fixtureProduct)
        {
            Navigation = navigation;
            matchTicketService = new MatchTicketService();
            settingService = new SettingService();
            ContactTraces = new ObservableCollection<ContactTrace>();

            GenerateSource(fixtureProduct);

            PurchaseCommand = new Command(async () => await Purchase());
            CloseClickedCommand = new Command(async () => await Close());
            AddContactTraceCommand = new Command(async () => await AddContactTrace());
        }

        public Command PurchaseCommand { get; set; }
        public Command CloseClickedCommand { get; set; }
        public Command AddContactTraceCommand { get; set; }

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
            set { Set(ref _quantity, value); }
        }

        public decimal Price
        {
            get { return _price; }
            set { Set(ref _price, value); }
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

                    Quantity = 1;
                    Price = fixtureProduct.Product.Price;
                    ProcessingFee = processingFee ?? 0.00m;
                    Total = (Quantity * Price) + ProcessingFee;
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

        async Task Purchase()
        {
            IsBusy = true;
            try
            {
                if(CreditCard != null && !String.IsNullOrEmpty(CreditCard.CardNumber) && !String.IsNullOrEmpty(CreditCard.Expiry) && !String.IsNullOrEmpty(CreditCard.CVV))
                {
                    PaymentAuthorize paymentAuthorize = new PaymentAuthorize
                    {
                        CardNumber = CreditCard.CardNumber,
                        Expiry = CreditCard.Expiry,
                        CVV = CreditCard.CVV,
                        Amount = Convert.ToString(Total),
                        FixtureProductID = FixtureProduct.ID,
                        CustomerID = Customer.ID,
                        ContactTraces = ContactTraces.ToList()
                    };

                    PaymentResponse paymentResponse = await matchTicketService.Purchase(paymentAuthorize);

                    IsBusy = false;


                    MessagingCenter.Subscribe<PurchaseTicketResultPageViewModel, PaymentResponse>(this, "TicketPurchased", async (objs, product) =>
                    {
                        MessagingCenter.Unsubscribe<PurchaseTicketResultPageViewModel, PaymentResponse>(this, "TicketPurchased");

                        if(product.IsApproved)
                            await Navigation.PopAsync();
                    });

                    await Navigation.PushModalAsync(new PurchaseTicketResultPage(paymentResponse));

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

        async Task AddContactTrace()
        {
            try
            {
                ContactTraces.Add(new ContactTrace
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone
                });

                FirstName = String.Empty;
                LastName = String.Empty;
                Email = String.Empty;
                Phone = String.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Contact Trace");
            }
        }

        async Task DeleteContactTrace()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Remove Contact Trace");
            }
        }
    }
}
