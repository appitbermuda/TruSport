using System;
using System.Collections.Generic;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class TablePage : ContentPage
    {
        TablePageViewModel tablePageViewModel;

        public TablePage()
        {
            tablePageViewModel = new TablePageViewModel();

            this.BindingContext = tablePageViewModel;

            InitializeComponent();
        }
    }
}
