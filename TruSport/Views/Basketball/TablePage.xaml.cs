using System;
using System.Collections.Generic;
using TruSport.ViewModels.Basketball;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class TablePage : ContentPage
    {
        TablePageViewModel tablePageViewModel;

        public TablePage()
        {
            tablePageViewModel = new TablePageViewModel();

            InitializeComponent();

            this.BindingContext = tablePageViewModel;
        }
    }
}
