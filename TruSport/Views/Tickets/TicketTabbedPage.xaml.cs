using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views.Tickets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketTabbedPage : TabbedPage
    {
        public TicketTabbedPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            string Token = await SecureStorage.GetAsync("Token");

            if (String.IsNullOrEmpty(Token))
            {
                await Navigation.PushAsync(new SignInPage(), true);
            }


        }
    }
}
