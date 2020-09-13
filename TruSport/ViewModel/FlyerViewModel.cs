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
    public class FlyerViewModel : BaseViewModel
    {
        private ObservableCollection<Flyer> flyerCollection;
        private string title;
        private double screenWidth;
        private bool noConnectivity;
        private bool noFlyers;
        private bool _isActivityIndicatorVisible;

        FlyerService flyerService;

        public FlyerViewModel()
        {
            ScreenWidth = App.ScreenWidth - 20;

            flyerService = new FlyerService();

            FlyerCollection = new ObservableCollection<Flyer>();

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

        public bool NoFlyers
        {
            get { return noFlyers; }
            set { Set(ref noFlyers, value); }
        }

        public ObservableCollection<Flyer> FlyerCollection
        {
            get { return flyerCollection; }
            set { Set(ref flyerCollection, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public double ScreenWidth
        {
            get { return screenWidth; }
            set { Set(ref screenWidth, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                var flyers = await flyerService.GetFlyers();

                FlyerCollection = new ObservableCollection<Flyer>(flyers);

                if (FlyerCollection.Count == 0)
                    NoFlyers = true;

            }
            else
                NoConnectivity = true;

            IsActivityIndicatorVisible = false;

            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

    }
}
