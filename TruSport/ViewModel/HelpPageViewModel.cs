using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class HelpPageViewModel : BaseViewModel
    {
        private ObservableCollection<Help> helpCollection;

        private bool _isActivityIndicatorVisible;

        public HelpPageViewModel()
        {
            HelpCollection = new ObservableCollection<Help>();

            GenerateSource();
        }


        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public ObservableCollection<Help> HelpCollection
        {
            get { return helpCollection; }
            set { Set(ref helpCollection, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            HelpCollection.Add(new Help
            {
                Question = "Where can I find standings for a specific league?",
                Answer = "League Standings can be found a number of ways; 1. Selecting Standings in the menu. 2. Viewing a team of your choice and navigating to the Standing header. or 3. Selecting the league header on a fixture."
            });

            HelpCollection.Add(new Help
            {
                Question = "Where can I find a teams information?",
                Answer = "To look at a team of your choosing, go to Teams in the side menu, and select the team you want to view."
            });

            HelpCollection.Add(new Help
            {
                Question = "I do not see my teams previous fixtures?",
                Answer = "On your teams profile, if you select the fixtures tab, scroll up in the list and you will see all previous fixtures."
            });

            HelpCollection.Add(new Help
            {
                Question = "I am looking at the app but the scores aren't updated.",
                Answer = "Unfortunately, the live score feature is not fully active yet. Check back and refresh the scores by pulling down on the list. Some scores may be updated while select games are being played, otherwise all scores will be updated right after the game has ended."
            });

            HelpCollection.Add(new Help
            {
                Question = "I am looking at the live games but the scores aren't updated.",
                Answer = "Unfortunately, the live score feature is not fully active yet. Refresh the scores by pulling down on the list. Some scores may be updated while select games are being played, otherwise all scores will be updated right after the game has ended."
            });


            HelpCollection.Add(new Help
            {
                Question = "I think your app is missing something, how do I contact you?",
                Answer = "We are always open to new features that will benefit all users, if you have any ideas or concerns that could better help the app, DM us on twitter or Instagram at @ontrackbda or hit the 'Contact Us' link in the menu. We will be happy to hear from you and evaluate your request."
            });
            IsActivityIndicatorVisible = false;

            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }
    }
}
