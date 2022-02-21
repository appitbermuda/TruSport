using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using TruSport.Data;
using TruSport.Model;
using TruSport.Model.Ticket;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.Basketball;
using TruSport.Views.Bowling;
using TruSport.Views.Cricket;
using TruSport.Views.Tennis;
using TruSport.Views.Tickets;
using TruSport.Views.Triathlon;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class AccountPageViewModel : BaseViewModel
    {
        private ObservableCollection<Wallet> _walletCollection;
        private ObservableCollection<CreditCard> _creditCardCollection;
        private Customer _customer;
        private bool _hasCards;
        private bool _isWalletActivityIndicatorVisible;
        private bool _isOrderActivityIndicatorVisible;
        private bool _isActivityIndicatorVisible;

        INavigation Navigation;
        NotificationRegistrationService notificationRegistrationService;
        WalletService walletService;


        public AccountPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            walletService = new WalletService();
            notificationRegistrationService = new NotificationRegistrationService();
            CreditCardCollection = new ObservableCollection<CreditCard>();
            WalletCollection = new ObservableCollection<Wallet>();

            GenerateSource();

            CreditCardSelectedCommand = new Command<object>(CreditCardSelected);
            AddCreditCardCommand = new Command(async () => await AddNewCard());
            SignOutCommand = new Command(async () => await SignOut());

            MessagingCenter.Unsubscribe<AccountPage, string>(this, "Refresh");
            MessagingCenter.Subscribe<AccountPage>(this, "Refresh", async (obj) =>
            {
                GenerateSource();
            });
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

        public ObservableCollection<Wallet> WalletCollection
        {
            get { return _walletCollection; }
            set { Set(ref _walletCollection, value); }
        }

        public bool HasCards
        {
            get { return _hasCards; }
            set { Set(ref _hasCards, value); }
        }

        public bool IsWalletActivityIndicatorVisible
        {
            get { return _isWalletActivityIndicatorVisible; }
            set { Set(ref _isWalletActivityIndicatorVisible, value); }
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

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {

                var email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(email);

                var wallet = await walletService.GetWallet();
                if(wallet != null)
                    WalletCollection = new ObservableCollection<Wallet>(wallet);

                IsWalletActivityIndicatorVisible = false;
            }

            IsActivityIndicatorVisible = false;
        }

        private async void CreditCardSelected(object obj)
        {
            var listView = obj as SfListView;
            var selectedCard = listView.SelectedItem as Wallet;

            var delete = await App.Current.MainPage.DisplayAlert("Delete Card", "Would you like to delete this card?", "Delete", "Cancel");

            if (delete)
            {
                await walletService.Delete(selectedCard.ID);

                var wallet = await walletService.GetWallet();
                if (wallet != null)
                {
                    HasCards = true;
                    WalletCollection = new ObservableCollection<Wallet>(wallet);
                }
            }
            //DisplayAlert("Message", (listView.SelectedItem as Fixture).ContactName + " is selected", "OK");
        }

        //private async void CreditCardSelected(object obj)
        //{
        //    var listView = obj as SfListView;
        //    var selectedCard = listView.SelectedItem as Wallet;

        //    MessagingCenter.Subscribe<CreditCardPageViewModel, Wallet>(this, "UpdateCreditCard", async (objs, fixture) =>
        //    {

        //        var wallet = await walletService.GetWallet();
        //        if (wallet != null)
        //            WalletCollection = new ObservableCollection<Wallet>(wallet);
        //    });

        //    await Navigation.PushModalAsync(new CreditCardPage(selectedCard.ID));
        //    //DisplayAlert("Message", (listView.SelectedItem as Fixture).ContactName + " is selected", "OK");
        //}

        async Task AddNewCard()
        {
            try
            {
                MessagingCenter.Subscribe<CreditCardPageViewModel, Wallet>(this, "InsertCreditCard", async (objs, fixture) =>
                {
                var wallet = await walletService.GetWallet();
                if (wallet != null)
                    WalletCollection = new ObservableCollection<Wallet>(wallet);
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

                try
                {
                    await notificationRegistrationService.DeregisterDeviceAsync();
                }
                catch (Exception ex)
                { }

                //App.Current.MainPage = new NavigationPage(new TicketFlyoutPage());

                if (Application.Current.MainPage is FlyoutPage mdp)
                {
                    var page = (Page)Activator.CreateInstance(typeof(SignInPage), new object[] { typeof(AccountPage) });

                    mdp.Detail = new NavigationPage(page)
                    {
                        BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                        BarTextColor = (Color)App.Current.Resources["navTextColor"]
                    };
                }

                //App.Current.MainPage = new NavigationPage(new MainPage());

                //// Handle when your app starts
                //if (App.Database != null)
                //{
                //    var sport = await App.Database.GetDefaultSport();

                //    if (sport == null || String.IsNullOrEmpty(sport.Sport))
                //        App.Current.MainPage = new NavigationPage(new MainPage());
                //    else
                //    {
                //        if (sport.Sport == Constants.Cricket)
                //            App.Current.MainPage = new CricketMasterDetailPage();
                //        else if (sport.Sport == Constants.Bowling)
                //            App.Current.MainPage = new BowlingMasterDetailPage();
                //        else if (sport.Sport == Constants.Tennis)
                //            App.Current.MainPage = new TennisMasterDetailPage();
                //        else if (sport.Sport == Constants.Basketball)
                //            App.Current.MainPage = new BasketballMasterDetailPage();
                //        else if (sport.Sport == Constants.Triathlon)
                //            App.Current.MainPage = new TriathlonFlyoutPage();
                //        else
                //            App.Current.MainPage = new FootballMasterDetailPage();
                //    }
                //}
                //else
                //{
                //    App.Current.MainPage = new NavigationPage(new MainPage());
                //}
                //if (Application.Current.MainPage is MasterDetailPage mdp)
                //{
                //    var page = (Page)Activator.CreateInstance(typeof(TicketTabbedPage));
                //    page.Title = "Tickets";

                //    mdp.Detail = new NavigationPage(page)
                //    {
                //        BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                //        BarTextColor = (Color)App.Current.Resources["navTextColor"]
                //    };
                //}
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sign Out");
            }
        }
    }
}
