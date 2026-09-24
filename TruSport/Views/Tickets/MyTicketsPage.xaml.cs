using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModel.Shop;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class MyTicketsPage : ContentPage
    {
        MyTicketPageViewModel myTicketPageViewModel;

        public MyTicketsPage()
        {
            myTicketPageViewModel = new MyTicketPageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = myTicketPageViewModel;


            ActiveTicketsList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "EventTicket.SportEvent.TicketTitle",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as CustomerTicket);
                    return item.EventTicket.SportEvent.TicketTitle + item.EventTicket.SportEvent.Date;
                }
            });

            UpcomingTicketsList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "EventTicket.SportEvent.TicketTitle",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as CustomerTicket);
                    return item.EventTicket.SportEvent.TicketTitle + item.EventTicket.SportEvent.Date;
                }
            });
        }

        void pullToRefreshUpcoming_Refreshing(System.Object sender, System.EventArgs e)
        {
        }

        void pullToRefreshCurrent_Refreshing(System.Object sender, System.EventArgs e)
        {
        }
    }
}
