using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using TruSport.Model;
using TruSport.Model.Ticket;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class CreditCardPageViewModel : BaseViewModel
    {
        private CreditCard _creditCard;
        private Wallet _card;
        private Customer _customer;
        private bool _isActivityIndicatorVisible;

        INavigation Navigation;
        WalletService walletService;

        public CreditCardPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            walletService = new WalletService();

            GenerateSource();

            SaveCommand = new Command(async () => await Save());
            CloseClickedCommand = new Command(async () => await Close());
        }

        public CreditCardPageViewModel(INavigation navigation, string cardID)
        {
            Navigation = navigation;


            GenerateSource(cardID);

            SaveCommand = new Command(async () => await Save());
            CloseClickedCommand = new Command(async () => await Close());
        }

        public Command SaveCommand { get; set; }
        public Command CloseClickedCommand { get; set; }

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

        public Wallet Card
        {
            get { return _card; }
            set { Set(ref _card, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {

                var email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(email);

                if (Customer != null)
                {
                    Card = new Wallet();

                    var wallet = await walletService.GetWallet();

                    if (wallet == null || wallet.Count == 0)
                        Card.IsDefault = true;
                }
                else
                    await Navigation.PopModalAsync();

            }

            IsActivityIndicatorVisible = false;
        }

        internal async void GenerateSource(string cardID)
        {
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {

                var email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(email);

                
                if (Customer != null)
                    Card = await walletService.GetCard(cardID);
                else
                    await Navigation.PopModalAsync();

            }

            IsActivityIndicatorVisible = false;
        }

        async Task Save()
        {
            IsActivityIndicatorVisible = true;
            try
            {
                bool CardNumberValid = false;
                bool ExpiryValid = false;
                bool CvvValid = false;

                CreditCard.CardNumber = CreditCard.CardNumber.Replace("-", "");
                CreditCard.Expiry = CreditCard.Expiry.Replace("/", "");

                DateTime expiryDate = new DateTime(Convert.ToInt32(20 + CreditCard.Expiry.Substring(2, 2)), Convert.ToInt32(CreditCard.Expiry.Substring(0, 2)), 1);
                bool expired = DateTime.Now > expiryDate.AddMonths(1);

                CardNumberValid = (!String.IsNullOrEmpty(CreditCard.CardNumber) && CreditCard.CardNumber.Replace("-", "").Length == 16);
                ExpiryValid = (!String.IsNullOrEmpty(CreditCard.Expiry) && CreditCard.Expiry.Replace("/","").Length == 4 && !expired);
                CvvValid = (!String.IsNullOrEmpty(CreditCard.CVV) && CreditCard.CVV.Length == 3);

                if (CardNumberValid && ExpiryValid && CvvValid)
                {
                    if (CreditCard.ID > 0)
                    {
                        await App.Database.Update(CreditCard);

                        MessagingCenter.Send(this, "UpdateCreditCard", CreditCard);
                    }
                    else
                    {
                        CreditCard.Email = Customer.Email;

                        await App.Database.Insert(CreditCard);

                        MessagingCenter.Send(this, "InsertCreditCard", CreditCard);
                    }

                    await Navigation.PopModalAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error","Please ensure you have entered valid card details.", "Okay");
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            IsActivityIndicatorVisible = false;
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
