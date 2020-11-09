using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TruSport.Data;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class TransferTicketPageViewModel : BaseViewModel
    {
        public MatchTicket _matchTicket;
        public TransferRequest _transferRequest;
        public Customer _customer;
        INavigation Navigation;
        private bool _isActivityIndicatorVisible;

        MatchTicketService matchTicketService;
        PushNotificationService pushNotificationService;

        public TransferTicketPageViewModel(INavigation navigation, MatchTicket matchTicket)
        {
            Navigation = navigation;
            matchTicketService = new MatchTicketService();
            pushNotificationService = new PushNotificationService();

            GenerateSource(matchTicket);

            BackCommand = new Command(async () => await Back());
            TransferCommand = new Command(async () => await Transfer());
        }

        public Command BackCommand { get; set; }
        public Command TransferCommand { get; set; }

        public TransferRequest TransferRequest
        {
            get { return _transferRequest; }
            set { Set(ref _transferRequest, value); }
        }

        public MatchTicket MatchTicket
        {
            get { return _matchTicket; }
            set { Set(ref _matchTicket, value); }
        }

        public Customer Customer
        {
            get { return _customer; }
            set { Set(ref _customer, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource(MatchTicket matchTicket)
        {
            try
            {
                string Email = await SecureStorage.GetAsync("Email");
                Customer = await App.Database.GetCustomerByIDAsync(Email);

                MatchTicket = matchTicket;

                TransferRequest = new TransferRequest();
                TransferRequest.MatchTicketID = matchTicket.ID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer Ticket");
            }
        }

        async Task Back()
        {
            //MessagingCenter.Send(this, "TicketPurchased", Response);
            await Navigation.PopModalAsync();
        }

        async Task Transfer()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                if (TransferRequest != null && !String.IsNullOrEmpty(TransferRequest.Email))
                {
                    string transferred = await matchTicketService.Transfer(TransferRequest);

                    try
                    {
                        await pushNotificationService.Send(new NotificationRequest
                        {
                            Text = Customer.FirstName + "has transferred you a match ticket, please go to 'My Tickets' to accept.",
                            Silent = false,
                            Tags = new string[] { TransferRequest.Email }
                        });
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "Transfer Notification");
                    }

                    await App.Current.MainPage.DisplayAlert("Ticket Transfer", transferred, "Okay");
                }

                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {

            }

            IsActivityIndicatorVisible = false;
        }
    }
}
