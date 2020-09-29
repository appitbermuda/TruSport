using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
    public class AccountPageViewModel : BaseViewModel
    {
        private ObservableCollection<CreditCard> _creditCardCollection;
        private ObservableCollection<Order> _orderCollection;
        private Customer _customer;
        private bool _isWalletActivityIndicatorVisible;
        private bool _isOrderActivityIndicatorVisible;
        private bool _isActivityIndicatorVisible;

        INavigation Navigation;
        OrderService orderService;


        public AccountPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            orderService = new OrderService();
            CreditCardCollection = new ObservableCollection<CreditCard>();
            OrderCollection = new ObservableCollection<Order>();

            GenerateSource();

            CreditCardSelectedCommand = new Command<object>(CreditCardSelected);
            AddCreditCardCommand = new Command(async () => await AddNewCard());
            SignOutCommand = new Command(async () => await SignOut());
        }

        public Command SignOutCommand { get; set; }
        public Command AddCreditCardCommand { get; set; }

        private Command<Object> cardSelectionChangedCommand;
        public Command<object> CreditCardSelectedCommand
        {
            get { return cardSelectionChangedCommand; }
            set { cardSelectionChangedCommand = value; }
        }

        public Customer Customer
        {
            get { return _customer; }
            set { Set(ref _customer, value); }
        }

        public ObservableCollection<CreditCard> CreditCardCollection
        {
            get { return _creditCardCollection; }
            set { Set(ref _creditCardCollection, value); }
        }

        public ObservableCollection<Order> OrderCollection
        {
            get { return _orderCollection; }
            set { Set(ref _orderCollection, value); }
        }

        public bool IsWalletActivityIndicatorVisible
        {
            get { return _isWalletActivityIndicatorVisible; }
            set { Set(ref _isWalletActivityIndicatorVisible, value); }
        }

        public bool IsOrderActivityIndicatorVisible
        {
            get { return _isOrderActivityIndicatorVisible; }
            set { Set(ref _isOrderActivityIndicatorVisible, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;
            IsWalletActivityIndicatorVisible = true;
            IsOrderActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {

                var email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(email);

                var creditCards = await App.Database.GetCreditCards(email);
                if(creditCards != null)
                    CreditCardCollection = new ObservableCollection<CreditCard>(creditCards);
                IsWalletActivityIndicatorVisible = false;


                var orderHistory = await orderService.GetOrderHistory(email);
                if (orderHistory != null)
                    OrderCollection = new ObservableCollection<Order>(orderHistory);
                IsOrderActivityIndicatorVisible = false;

            }

            IsActivityIndicatorVisible = false;
        }

        private async void CreditCardSelected(object obj)
        {
            var listView = obj as SfListView;
            var selectedCard = listView.SelectedItem as CreditCard;

            MessagingCenter.Subscribe<CreditCardPageViewModel, CreditCard>(this, "UpdateCreditCard", async (objs, fixture) =>
            {
                var creditCards = await App.Database.GetCreditCards(Customer.Email);
                if (creditCards != null)
                    CreditCardCollection = new ObservableCollection<CreditCard>(creditCards);
                IsWalletActivityIndicatorVisible = false;
            });

            await Navigation.PushModalAsync(new CreditCardPage(selectedCard.ID));
            //DisplayAlert("Message", (listView.SelectedItem as Fixture).ContactName + " is selected", "OK");
        }

        async Task AddNewCard()
        {
            try
            {
                MessagingCenter.Subscribe<CreditCardPageViewModel, CreditCard>(this, "InsertCreditCard", async (objs, fixture) =>
                {
                    var creditCards = await App.Database.GetCreditCards(Customer.Email);
                    if (creditCards != null)
                        CreditCardCollection = new ObservableCollection<CreditCard>(creditCards);
                    IsWalletActivityIndicatorVisible = false;
                });

                await Navigation.PushModalAsync(new CreditCardPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task SignOut()
        {
            try
            {
                SecureStorage.RemoveAll();
                await App.Database.SignOut();

                if (Application.Current.MainPage is MasterDetailPage mdp)
                {
                    var page = (Page)Activator.CreateInstance(typeof(TicketTabbedPage));
                    page.Title = "Tickets";

                    mdp.Detail = new NavigationPage(page)
                    {
                        BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                        BarTextColor = (Color)App.Current.Resources["navTextColor"]
                    };
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sign Out");
            }
        }
    }
}
