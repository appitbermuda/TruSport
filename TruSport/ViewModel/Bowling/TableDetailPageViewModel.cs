using System;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Bowling
{
    public class TableDetailPageViewModel : BaseViewModel
    {
        private BowlingLeagueStanding _standing;
        private bool _isActivityIndicatorVisible;
        INavigation Navigation;

        public TableDetailPageViewModel(INavigation navigation, BowlingLeagueStanding standing)
        {
            Navigation = navigation;

            Standing = standing;
        }

        public BowlingLeagueStanding Standing
        {
            get { return _standing; }
            set { Set(ref this._standing, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }
    }
}
