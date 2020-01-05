using System;
using System.Collections.Generic;
using TruSport.Views.Admin;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class TablePage : ContentPage
    {
        public TablePage()
        {
            InitializeComponent();

            //loader.Easing = Easing.Linear;
        }

        void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                PremierLabel.TextColor = Color.White;
                FirstLabel.TextColor = Color.Gray;
                CoronaLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = true;
                ////PremierLabel.TextColor = Color.Pink;
                //FirstSelected.IsVisible = false;
            }
            else if (e.Index == 1)
            {
                FirstLabel.TextColor = Color.White;
                PremierLabel.TextColor = Color.Gray;
                CoronaLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
            else if (e.Index == 2)
            {
                CoronaLabel.TextColor = Color.White;
                FirstLabel.TextColor = Color.Gray;
                PremierLabel.TextColor = Color.Gray;
                //PremierSelected.IsVisible = false;
                //FirstSelected.IsVisible = true;
            }
        }

        async void AdminClicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushModalAsync(new AdminMainPage());
        }
    }
}
