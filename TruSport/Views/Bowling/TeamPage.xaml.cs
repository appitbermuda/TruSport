using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using TruSport.Model;
using TruSport.ViewModels.Bowling;
using Xamarin.Forms;

namespace TruSport.Views.Bowling
{
    public partial class TeamPage : ContentPage
    {
        TeamPageViewModel teamPageViewModel;

        public TeamPage()
        {
            teamPageViewModel = new TeamPageViewModel();

            InitializeComponent();

            this.BindingContext = teamPageViewModel;

            TeamsList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "League.Name",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Team);
                    return item.League.Order;
                }
            });
        }

        async void TeamSelected(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var items = e.AddedItems;

            if (items.Count == 0) return;

            var item = (Team)TeamsList.SelectedItem;

            await Navigation.PushAsync(new TeamProfilePage(item));

            TeamsList.SelectedItems.Clear();
        }
    }
}
