using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using TruSport.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(SearchBar), typeof(ExtendedSearchBarRenderer))]
namespace TruSport.Droid
{
    public class ExtendedSearchBarRenderer : SearchBarRenderer
    {
        public ExtendedSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            SearchView searchView = (base.Control as SearchView);

            //Customize SearchView here

            int searchPlateId = searchView.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
            Android.Views.View searchPlateView = searchView.FindViewById(searchPlateId);
            searchPlateView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            var searchIconId = searchView.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            if (searchIconId > 0)
            {
                var searchPlateIcon = searchView.FindViewById(searchIconId);
                (searchPlateIcon as ImageView).SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcIn);
            }
        }
    }
}
