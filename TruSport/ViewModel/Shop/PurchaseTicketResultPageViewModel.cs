using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class PurchaseTicketResultPageViewModel : BaseViewModel
    {
        public PaymentResponse _paymentReponse;
        INavigation Navigation;

        public PurchaseTicketResultPageViewModel(INavigation navigation, PaymentResponse paymentResponse)
        {
            Navigation = navigation;

            GenerateSource(paymentResponse);

            BackCommand = new Command(async () => await Back());
        }

        public Command BackCommand { get; set; }

        public PaymentResponse Response
        {
            get { return _paymentReponse; }
            set { Set(ref _paymentReponse, value); }
        }

        internal async void GenerateSource(PaymentResponse paymentResponse)
        {
            try
            {
                Response = paymentResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase Ticket");
            }
        }

        async Task Back()
        {
            MessagingCenter.Send(this, "TicketPurchased", Response);
            await Navigation.PopModalAsync();
        }
    }
}
