using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class CreditCardPageViewModel : BaseViewModel
    {
        private CreditCard _creditCard;
        private Customer _customer;
        private bool _isActivityIndicatorVisible;

        INavigation Navigation;

        public CreditCardPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            GenerateSource();

            SaveCommand = new Command(async () => await Save());
        }

        public CreditCardPageViewModel(INavigation navigation, int creditCardID)
        {
            Navigation = navigation;


            GenerateSource(creditCardID);

            SaveCommand = new Command(async () => await Save());
        }

        public Command SaveCommand { get; set; }

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
                    CreditCard = new CreditCard();
                else
                    await Navigation.PopModalAsync();

            }

            IsActivityIndicatorVisible = false;
        }

        internal async void GenerateSource(int creditCardID)
        {
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {

                var email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(email);

                
                if (Customer != null)
                    CreditCard = await App.Database.GetCreditCard(creditCardID);
                else
                    await Navigation.PopModalAsync();

            }

            IsActivityIndicatorVisible = false;
        }

        async Task Save()
        {
            try
            {
                bool CardNumberValid = false;
                bool ExpiryValid = false;
                bool CvvValid = false;

                DateTime expiryDate = new DateTime(Convert.ToInt32(20 + CreditCard.Expiry.Substring(3, 2)), Convert.ToInt32(CreditCard.Expiry.Substring(0, 2)), 1);
                bool expired = DateTime.Now > expiryDate.AddMonths(1);

                CardNumberValid = (!String.IsNullOrEmpty(CreditCard.CardNumber) && CreditCard.CardNumber.Length == 16);
                ExpiryValid = (!String.IsNullOrEmpty(CreditCard.Expiry) && CreditCard.Expiry.Length == 4 && !expired);
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
        }
    }
}
