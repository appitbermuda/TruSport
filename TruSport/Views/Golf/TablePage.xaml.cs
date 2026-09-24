using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Golf;
using Xamarin.Forms;

namespace TruSport.Views.Golf
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

        public TablePage(League league)
        {
            tablePageViewModel = new TablePageViewModel(league);

            InitializeComponent();

            this.BindingContext = tablePageViewModel;
        }
    }
}
