using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Golf;
using Xamarin.Forms;

namespace TruSport.Views.Golf
{
    public partial class FixtureDetailsPage : ContentPage
    {
        FixtureDetailPageViewModel fixtureDetailPageViewModel;

        public FixtureDetailsPage(GolfFixture fixture)
        {
            fixtureDetailPageViewModel = new FixtureDetailPageViewModel(Navigation, fixture);

            InitializeComponent();

            this.BindingContext = fixtureDetailPageViewModel;
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
