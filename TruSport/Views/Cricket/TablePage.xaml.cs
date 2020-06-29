using System;
using System.Collections.Generic;
using TruSport.ViewModels.Cricket;
using TruSport.Views.Admin;
using Xamarin.Forms;

namespace TruSport.Views.Cricket
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
