using System;
using System.Diagnostics;
using System.Linq;
using TruSport.Data;
using TruSport.ViewModels;
using TruSport.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class TicketTabbedPageViewModel : BaseViewModel
    {
        NotificationRegistrationService notificationRegistrationService;
        INavigation Navigation;

        public TicketTabbedPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            notificationRegistrationService = new NotificationRegistrationService();

            GenerateSource();
        }

        internal async void GenerateSource()
        {
            try
            {
                string Token = await SecureStorage.GetAsync("Token");
                string email = await SecureStorage.GetAsync("Email");

                if (String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(email))
                {
                    await Navigation.PushAsync(new SignInPage(), true);
                }
                else
                {
                    var tags = await App.Database.GetTags();
                    var tagsList = tags.ToList();
                    tagsList.Add(email);

                    tags = tagsList.ToArray();

                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ticket");
            }
        }
    }
}
