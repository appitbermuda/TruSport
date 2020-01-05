using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.Views.Admin;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class TeamPage : ContentPage
    {
        public TeamPage()
        {
            InitializeComponent();

            //loader.Easing = Easing.Linear;
        }

        //void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        //{
        //    if(e.Index == 0)
        //    {
        //        PremierLabel.TextColor = (Color)App.Current.Resources["primaryPink"];
        //        FirstLabel.TextColor = Color.White;
        //        //PremierSelected.IsVisible = true;
        //        //FirstSelected.IsVisible = false;
        //    }
        //    else if (e.Index == 1)
        //    {
        //        FirstLabel.TextColor = (Color)App.Current.Resources["primaryPink"];
        //        PremierLabel.TextColor = Color.White;
        //        //PremierSelected.IsVisible = false;
        //        //FirstSelected.IsVisible = true;
        //    }
        //}

        //async void FirstDivisionSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        //{
        //    var items = e.AddedItems;

        //    if (items.Count == 0) return;

        //    var item = (Team)FirstDivisionList.SelectedItem;

        //    await Navigation.PushAsync(new TeamProfilePage(item));
        //    //await Navigation.PushModalAsync(new NavigationPage(new TeamProfilePage(item))
        //    //{
        //    //    BarTextColor = Color.White,
        //    //    BarBackgroundColor = Color.Transparent
        //    //});

        //    FirstDivisionList.SelectedItems.Clear();
        //}

        async void TeamSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Team)TeamsList.SelectedItem;

            await Navigation.PushAsync(new TeamProfilePage(item));

            TeamsList.SelectedItems.Clear();
        }

        //async void PremierDivisionSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        //{
        //    var items = e.AddedItems;

        //    if (items.Count == 0) return;

        //    var item = (Team)PremierDivisionList.SelectedItem;

        //    await Navigation.PushAsync(new TeamProfilePage(item));

        //    PremierDivisionList.SelectedItems.Clear();
        //}

        async void CoronaDivisionSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            //var item = (Team)CoronaDivisionList.SelectedItem;

            //await Navigation.PushAsync(new TeamProfilePage(item));
            //await Navigation.PushModalAsync(new NavigationPage(new TeamProfilePage(item))
            //{
            //    BarTextColor = Color.White,
            //    BarBackgroundColor = Color.Transparent
            //});

            //CoronaDivisionList.SelectedItems.Clear();
        }

        async void AdminClicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushModalAsync(new AdminMainPage());
        }
    }
}
