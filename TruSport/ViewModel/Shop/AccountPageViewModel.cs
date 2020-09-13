using System;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Essentials;

namespace TruSport.ViewModel.Shop
{
    public class AccountPageViewModel : BaseViewModel
    {
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
                //var customer = 

                //var flyers = await flyerService.GetFlyers();

                //FlyerCollection = new ObservableCollection<Flyer>(flyers);

                //if (FlyerCollection.Count == 0)
                //    NoFlyers = true;

            }

            IsActivityIndicatorVisible = false;

            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }
    }
}
