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
    public class AlertSettingViewModel : BaseViewModel
    {
        private bool _isActivityIndicatorVisible;
        INavigation Navigation;

        public AlertSettingViewModel()
        { 

            GenerateSource();
        }

        public AlertSettingViewModel(INavigation navigation)
        {
            Navigation = navigation;
            GenerateSource();
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            IsActivityIndicatorVisible = false;
        }

    }
}
