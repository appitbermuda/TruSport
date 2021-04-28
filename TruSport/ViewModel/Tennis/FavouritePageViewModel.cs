using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using System.Threading.Tasks;
using TruSport.Services;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;

namespace TruSport.ViewModels.Tennis
{
    public class FavouritePageViewModel : BaseViewModel
    {
        #region Fields
        private Favourite tappedInfo;
        private ObservableCollection<TennisFixture> favouriteFixturesCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool _isTeamActivityIndicatorVisible;
        private bool _isFixtureActivityIndicatorVisible;
        private bool noTeamFavourites;
        private bool noFixtureFavourites;
        private bool noConnectivity;
        
        #endregion

        #region Constructor

        public FavouritePageViewModel()
        {
            FavouriteFixturesCollection = new ObservableCollection<TennisFixture>();

            GenerateSource();

            DeleteFixtureFavouriteCommand = new Command<object>(DeleteFixtureFavourite);
        }

        #endregion

        #region Properties

        public Command<object> DeleteFixtureFavouriteCommand { get; }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ItemTapCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }

        public ObservableCollection<TennisFixture> FavouriteFixturesCollection
        {
            get { return favouriteFixturesCollection; }
            set { Set(ref favouriteFixturesCollection, value); }
        }

        public bool NoTeamFavourites
        {
            get { return noTeamFavourites; }
            set { Set(ref noTeamFavourites, value); }
        }

        public bool NoFixtureFavourites
        {
            get { return noFixtureFavourites; }
            set { Set(ref noFixtureFavourites, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool IsTeamActivityIndicatorVisible
        {
            get { return _isTeamActivityIndicatorVisible; }
            set { Set(ref _isTeamActivityIndicatorVisible, value); }
        }

        public bool IsFixtureActivityIndicatorVisible
        {
            get { return _isFixtureActivityIndicatorVisible; }
            set { Set(ref _isFixtureActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            try
            {

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    IsFixtureActivityIndicatorVisible = true;

                    var favouriteFixtures = await App.Database.GetTennisFixtureFavourites();

                    if (favouriteFixtures != null && favouriteFixtures.Count > 0)
                    {
                        NoFixtureFavourites = false;
                        FavouriteFixturesCollection = new ObservableCollection<TennisFixture>(favouriteFixtures.Select(e => e.TennisFixture));
                    }
                    else
                        NoFixtureFavourites = true;

                    IsFixtureActivityIndicatorVisible = false;
                }
                else
                    NoConnectivity = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Favourite");
            }

            IsFixtureActivityIndicatorVisible = false;
            IsTeamActivityIndicatorVisible = false;
            IsActivityIndicatorVisible = false;
        }

        public async Task RefreshFixtureFavourites()
        {
            try
            {
                var favouriteFixtures = await App.Database.GetTennisFixtureFavourites();

                if (favouriteFixtures != null && favouriteFixtures.Count > 0)
                {
                    NoFixtureFavourites = false;
                    FavouriteFixturesCollection = new ObservableCollection<TennisFixture>(favouriteFixtures.Select(e => e.TennisFixture));
                }
                else
                {
                    NoFixtureFavourites = true;
                    FavouriteFixturesCollection = new ObservableCollection<TennisFixture>();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshFixtureFavourites");
            }
        }

        public async void DeleteFixtureFavourite(object obj)
        {
            try
            {
                var fixture = obj as TennisFixture;

                await App.Database.DeleteTennisFixtureFavourite(fixture.ID);

                await RefreshFixtureFavourites();

                //DisplayDataDeletedPromt();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "DeleteFixtureFavourite");
            }
        }

        #endregion


        #region Player Info

        FixtureListView[] Player = new FixtureListView[]
         {
            
         };

        string[] Agents = new string[]
        {
            "LOCAL",
            "INTERNATIONAL"
        };

        string[] SyncTitles = new string[]
        {
            "First Division",
            "Premier Division",
            "Corona League"
        };

        #endregion
    }
}
