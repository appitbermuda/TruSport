using System;
using Android.Content;
using Android.Support.Design.Widget;
using TruSport.Droid;
using TruSport.Views.Tickets;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

//[assembly: ExportRenderer(typeof(TicketTabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace TruSport.Droid
{
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer
    {
        public ExtendedTabbedPageRenderer(Context context) : base(context)
        {
        }

        //private TicketTabbedPage _page;
        //protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        //{
        //    base.OnElementChanged(e);
        //    if (e.NewElement != null)
        //    {
        //        _page = (TicketTabbedPage)e.NewElement;
        //    }
        //    else
        //    {
        //        _page = (TicketTabbedPage)e.OldElement;
        //    }

        //}
        //async void TabLayout.IOnTabSelectedListener.OnTabReselected(TabLayout.Tab tab)
        //{
        //    await _page.CurrentPage.Navigation.PopToRootAsync();
        //}
    }
}
