using System;
using System.Collections.Generic;
using TruSport.ViewModels.Tennis;
using Xamarin.Forms;

namespace TruSport.Views.Tennis
{
    public partial class RankingPage : ContentPage
    {
        RankingPageViewModel rankingPageViewModel;

        public RankingPage()
        {
            rankingPageViewModel = new RankingPageViewModel();

            InitializeComponent();

            this.BindingContext = rankingPageViewModel;
        }
    }
}
