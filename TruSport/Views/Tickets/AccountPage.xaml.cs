using System;
using System.Collections.Generic;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class AccountPage : ContentPage
    {
        AccountPageViewModel accountPageViewModel;

        public AccountPage()
        {
            accountPageViewModel = new AccountPageViewModel(Navigation);

            this.BindingContext = accountPageViewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Send<AccountPage>(this, "Refresh");
        }

        void Menu_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.MainPage is MasterDetailPage mdp)
            {
                var page = (Page)Activator.CreateInstance(typeof(Tickets.TicketTabbedPage));
                page.Title = "Tickets";

                mdp.Detail = new NavigationPage(page)
                {
                    BarBackgroundColor = (Color)App.Current.Resources["navBackgroundColor"],
                    BarTextColor = (Color)App.Current.Resources["navTextColor"]
                };
            }
        }
    }
}
