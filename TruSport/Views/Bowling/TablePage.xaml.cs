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

        void MenuButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.MainPage is MasterDetailPage mdp)
            {
                mdp.IsPresented = true;
            }
        }
    }
}
