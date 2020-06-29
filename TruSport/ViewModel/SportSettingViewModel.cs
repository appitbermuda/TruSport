using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TruSport.Data;
using TruSport.Model;
using TruSport.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class SportSettingViewModel : BaseViewModel
    {
        private ObservableCollection<string> _sportCollection;
        private string _defaultSport;
        private bool _footballAlert;
        private bool _cricketAlert;
        private bool _favouriteAlert;
        private bool _isActivityIndicatorVisible;
        INavigation Navigation;

        SportService sportService;

        public SportSettingViewModel()
        {
            sportService = new SportService();

            SportCollection = new ObservableCollection<string>();

            GenerateSource();

        }

        public SportSettingViewModel(INavigation navigation)
        {
            Navigation = navigation;

            GenerateSource();
        }

        public Command<string> SelectedSportCommand { get; }

        public ObservableCollection<string> SportCollection
        {
            get { return _sportCollection; }
            set { Set(ref _sportCollection, value); }
        }

        public string DefaultSport
        {
            get { return _defaultSport; }
            set { Set(ref _defaultSport, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var defaultSport = await App.Database.GetDefaultSport();
                DefaultSport = defaultSport.Sport;
            }
            catch(Exception ex)
            {

            }

            IsActivityIndicatorVisible = false;
        }

    }
}
