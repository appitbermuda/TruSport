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
    public class AwardViewModel : BaseViewModel
    {
        
        private ObservableCollection<Award> playerAwardCollection;
        private bool noConnectivity;
        private bool noAwards;
        private bool _isActivityIndicatorVisible;

        AwardService playerAwardService;

        public AwardViewModel()
        {
            playerAwardService = new AwardService();

            AwardCollection = new ObservableCollection<Award>();

            GenerateSource();
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool NoAwards
        {
            get { return noAwards; }
            set { Set(ref noAwards, value); }
        }

        public ObservableCollection<Award> AwardCollection
        {
            get { return playerAwardCollection; }
            set { Set(ref playerAwardCollection, value); }
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

                var sport = await SecureStorage.GetAsync("Sport");

                var awards = await playerAwardService.GetAwardsBySportType(sport);

                AwardCollection = new ObservableCollection<Award>(awards);

                if (AwardCollection.Count == 0)
                    NoAwards = true;

            }
            else
                NoConnectivity = true;

            IsActivityIndicatorVisible = false;

            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

    }
}
