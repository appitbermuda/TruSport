using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class TransferPage : ContentPage
    {
        TransferViewModel transferViewModel;

        public TransferPage()
        {
            transferViewModel = new TransferViewModel();

            this.BindingContext = transferViewModel;
            InitializeComponent();

            TransferList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "PreviousTeam",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Transfer);
                    return item.PreviousTeam;
                }
            });
        }
    }
}
