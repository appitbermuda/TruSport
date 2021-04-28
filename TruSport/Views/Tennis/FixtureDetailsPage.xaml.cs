using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Tennis;
using Xamarin.Forms;

namespace TruSport.Views.Tennis
{
    public partial class FixtureDetailsPage : ContentPage
    {
        FixtureDetailPageViewModel fixtureDetailPageViewModel;

        public FixtureDetailsPage(TennisFixture tennisFixture)
        {
            fixtureDetailPageViewModel = new FixtureDetailPageViewModel(Navigation, tennisFixture);

            InitializeComponent();

            this.BindingContext = fixtureDetailPageViewModel;
        }

        void SfTabView_TabItemTapped(System.Object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            try
            {
                if ((e.TabItem.Title == "Head to Head" && (fixtureDetailPageViewModel.HeadToHeadFixtureCollection == null || fixtureDetailPageViewModel.HeadToHeadFixtureCollection.Count == 0)))
                    e.Cancel = true;
            }
            catch (Exception ex)
            {

            }
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
