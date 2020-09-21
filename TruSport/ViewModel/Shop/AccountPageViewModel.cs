using System;
using System.Collections.ObjectModel;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Essentials;

namespace TruSport.ViewModel.Shop
{
    public class AccountPageViewModel : BaseViewModel
    {
        private ObservableCollection<CreditCard> _creditCardCollection;
        private Customer _customer;
        private bool _isActivityIndicatorVisible;


        public AccountPageViewModel()
        {
            GenerateSource();
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
                var customer = App.Database.GetCustomerByIDAsync(email);

                var creditCards = App.Database.GetCreditCards(email);

            }

            IsActivityIndicatorVisible = false;
        }
    }
}
