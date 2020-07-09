using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Cricket;
using Xamarin.Forms;

namespace TruSport.Views.Cricket
{
    public partial class FixtureDetailsPage : ContentPage
    {
        FixtureDetailPageViewModel fixtureDetailPageViewModel;

        public FixtureDetailsPage()
        {
            InitializeComponent();
        }

        public FixtureDetailsPage(CricketFixture fixture)
        {
            fixtureDetailPageViewModel = new FixtureDetailPageViewModel(Navigation, fixture);

            this.BindingContext = fixtureDetailPageViewModel;

            InitializeComponent();
        }

        void SfTabView_TabItemTapped(System.Object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            try
            {
                if ((e.TabItem.Title == "Table" && (fixtureDetailPageViewModel.TableCollection == null || fixtureDetailPageViewModel.TableCollection.Count == 0)) || (e.TabItem.Title == "Head to Head" && (fixtureDetailPageViewModel.HeadToHeadFixtureCollection == null || fixtureDetailPageViewModel.HeadToHeadFixtureCollection.Count == 0)))
                    e.Cancel = true;
            }
            catch(Exception ex)
            {

            }
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
