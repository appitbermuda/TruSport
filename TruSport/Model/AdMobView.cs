using System;
using Xamarin.Forms;

namespace TruSport.Model
{
    public class AdMobView : View
    {
        public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard, SmartBannerPortrait }
        public Sizes Size { get; set; }
        //public AdBanner()
        //{
            //this.BackgroundColor = Color.Accent;
        //}
        public AdMobView()
        {
            BackgroundColor = (Color)App.Current.Resources["primaryDarkBlueTwo"];
        }
    }
    //public class AdMobView : View
    //{
    //    public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
    //               nameof(AdUnitId),
    //               typeof(string),
    //               typeof(AdMobView),
    //               string.Empty);

    //    public string AdUnitId
    //    {
    //        get => (string)GetValue(AdUnitIdProperty);
    //        set => SetValue(AdUnitIdProperty, value);
    //    }
    //}
}
