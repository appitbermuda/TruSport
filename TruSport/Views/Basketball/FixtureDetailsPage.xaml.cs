using System;
using System.Collections.Generic;
using TruSport.Model;
using TruSport.ViewModels.Basketball;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class FixtureDetailsPage : ContentPage
    {
        FixtureDetailPageViewModel fixtureDetailPageViewModel;

        public FixtureDetailsPage(BasketballFixture fixture)
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
