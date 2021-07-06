using System;
using System.Collections.Generic;
using TruSport.ViewModels.Bowling;
using Xamarin.Forms;

namespace TruSport.Views.Bowling
{
    public partial class TablePage : ContentPage
    {
        TablePageViewModel tablePageViewModel;

        public TablePage()
        {
            tablePageViewModel = new TablePageViewModel(Navigation);

            InitializeComponent();

            this.BindingContext = tablePageViewModel;
        }
    }
}
