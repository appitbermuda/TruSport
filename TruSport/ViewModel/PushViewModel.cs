using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TruSport.Data;
using TruSport.Model;
using TruSport.Services;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class PushViewModel : BaseViewModel
    {
        private string name;
        private string title;
        private string body;

        private bool _isActivityIndicatorVisible;

        PushNotificationService pushNotificationService;

        public PushViewModel()
        {

            pushNotificationService = new PushNotificationService();

            GenerateSource();

            SendCommand = new Command(async () => await Send());
        }

        public Command SendCommand { get; }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }

        public string Body
        {
            get { return body; }
            set { Set(ref body, value); }
        }

        //public ObservableCollection<RssFeedItem> RSSFeed
        //{
        //    get { return rssFeed; }
        //    set { Set(ref rssFeed, value); }
        //}

        //public ObservableCollection<RssFeedItem> BerNewsFeed
        //{
        //    get { return berNewsFeed; }
        //    set { Set(ref berNewsFeed, value); }
        //}

        //public ObservableCollection<RssFeedItem> IStatsFeed
        //{
        //    get { return iStatsFeed; }
        //    set { Set(ref iStatsFeed, value); }
        //}

        //public ObservableCollection<RssFeedItem> RGFeed
        //{
        //    get { return rgFeed; }
        //    set { Set(ref rgFeed, value); }
        //}

        private RssFeedItem _selectedItem;

        public RssFeedItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                OnItemSelected(value);
                Set(ref _selectedItem, value);
            }
        }

        private void OnItemSelected(RssFeedItem item)
        {
            if (item != null)
            {
                Device.OpenUri(item.Link);
            }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;




            IsActivityIndicatorVisible = false;

        }

        async Task Send()
        {
            if (!String.IsNullOrWhiteSpace(Name) || !String.IsNullOrWhiteSpace(Title) || !String.IsNullOrWhiteSpace(Body))
            {
                try
                {
                    IsActivityIndicatorVisible = true;

                    //await Application.Current.MainPage.DisplayAlert("Success", String.Format("Push Sent - Name: {0} Title: {1} Message: {2}", Name, Title, Body), "Okay");

                    await pushNotificationService.Send(Name, Title, Body);

                    await Application.Current.MainPage.DisplayAlert("Success", String.Format("Push Sent - Name: {0}", Body), "Okay");

                    Name = String.Empty;
                    Title = String.Empty;
                    Body = String.Empty;

                    IsActivityIndicatorVisible = false;
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Issue sending notification, please try again.", "Okay");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill out all fields.", "Okay");
            }
        }
    }
}
