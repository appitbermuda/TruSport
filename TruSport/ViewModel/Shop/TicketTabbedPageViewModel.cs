using System;
using System.Diagnostics;
using TruSport.ViewModels;
using TruSport.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class TicketTabbedPageViewModel : BaseViewModel
    {

        INavigation Navigation;

        public TicketTabbedPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            GenerateSource();
        }

        internal async void GenerateSource()
        {
            try
            {
                string Token = await SecureStorage.GetAsync("Token");

                if (String.IsNullOrEmpty(Token))
                {
                    await Navigation.PushAsync(new SignInPage(), true);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ticket");
            }
        }
    }
}
