using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using TruSport.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class TransferViewModel : BaseViewModel
    {
        
        private ObservableCollection<Transfer> transferCollection;
        private string title;
        private bool noConnectivity;
        private bool _isActivityIndicatorVisible;

        TransferService transferService;

        public TransferViewModel()
        {
            transferService = new TransferService();

            TransferCollection = new ObservableCollection<Transfer>();

            GenerateSource();
        }

        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }


        public ObservableCollection<Transfer> TransferCollection
        {
            get { return transferCollection; }
            set { Set(ref transferCollection, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                var transfers = await transferService.GetTransfers();

                var transfer = transfers.FirstOrDefault();
                Title = "Transfer " + transfer.Date;

                TransferCollection = new ObservableCollection<Transfer>(transfers);
            }
            else
                NoConnectivity = true;

            IsActivityIndicatorVisible = false;

            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

    }
}
