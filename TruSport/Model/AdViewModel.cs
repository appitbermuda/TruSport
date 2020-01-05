using System;
using Xamarin.Forms;

namespace TruSport.Model
{
    public class AdViewModel : BindableObject
    {

        public string AdUnitId { get; set; } = "ca-app-pub-3940256099942544/2934735716";
        //public string AdUnitId { get; set; } = "ca-app-pub-1338169805120312/7270444885";

        public void Test()
        {
            if (Device.RuntimePlatform == Device.iOS)
                AdUnitId = "ca-app-pub-1338169805120312/7270444885";
            else if (Device.RuntimePlatform == Device.Android)
                AdUnitId = "Android Key";
        }
    }
}
