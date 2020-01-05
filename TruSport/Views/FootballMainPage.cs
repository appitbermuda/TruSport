using System;
using TruSport.Views.Football;
using Xamarin.Forms;

namespace TruSport.Views
{
    public class FootballsMainPage : TabbedPage
    {
        public FootballsMainPage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            //BarBackgroundColor  = (Color)App.Current.Resources["primaryLightGray"];
            BarBackgroundColor = Color.FromHex("37474F");
            //BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;
            //BarBackgroundColor = (Color)App.Current.Resources["primaryBlue"];

            //Children.Add(new NavigationPage(new TeamsPage())
            //{
            //    BarBackgroundColor = Color.FromHex("37474F"),
            //    BarTextColor = Color.White
            //});

            Children.Add((new FixturePage()));
            Children.Add((new TeamPage()));
            Children.Add((new TablePage()));
            Children.Add((new FavouritePage()));
            Children.Add((new NewsPage()));

            //Children.Add(new NavigationPage(new FixturePage())
            //{
            //    BarBackgroundColor = (Color)App.Current.Resources["primaryLightGray"],
            //    BarTextColor = Color.White
            //});
            //Children.Add(new NavigationPage(new TeamPage())
            //{
            //    BarBackgroundColor = (Color)App.Current.Resources["primaryLightGray"],
            //    BarTextColor = Color.White
            //});
            //Children.Add(new NavigationPage(new TablePage())
            //{
            //    BarBackgroundColor = (Color)App.Current.Resources["primaryLightGray"],
            //    BarTextColor = Color.White
            //});
            //Children.Add(new NavigationPage(new FavouritePage())
            //{
            //    BarBackgroundColor = (Color)App.Current.Resources["primaryLightGray"],
            //    BarTextColor = Color.White
            //});
            //Children.Add(new NavigationPage(new NewsPage())
            //{
            //    BarBackgroundColor = (Color)App.Current.Resources["primaryLightGray"],
            //    BarTextColor = Color.White
            //});
        }
    }
}

