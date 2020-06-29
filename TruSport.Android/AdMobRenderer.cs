using System;
using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using TruSport.Droid;
using TruSport.Model;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace TruSport.Droid
{
    public class AdMobRenderer : ViewRenderer
    {
        Context context;
        public AdMobRenderer(Context _context) : base(_context)
        {
            context = _context;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var adView = new AdView(Context);
                switch ((Element as AdMobView).Size)
                {
                    case AdMobView.Sizes.Standardbanner:
                        adView.AdSize = AdSize.Banner;
                        break;
                    case AdMobView.Sizes.LargeBanner:
                        adView.AdSize = AdSize.LargeBanner;
                        break;
                    case AdMobView.Sizes.MediumRectangle:
                        adView.AdSize = AdSize.MediumRectangle;
                        break;
                    case AdMobView.Sizes.FullBanner:
                        adView.AdSize = AdSize.FullBanner;
                        break;
                    case AdMobView.Sizes.Leaderboard:
                        adView.AdSize = AdSize.Leaderboard;
                        break;
                    case AdMobView.Sizes.SmartBannerPortrait:
                        adView.AdSize = AdSize.SmartBanner;
                        break;
                    default:
                        adView.AdSize = AdSize.Banner;
                        break;
                }
                // TODO: change this id to your admob id  
                //adView.AdUnitId = "Your AdMob id";
#if DEBUG
                // This is a string in the Resources/values/strings.xml that I added or you can modify it here. This comes from admob and contains a / in it
                adView.AdUnitId = Constants.AdMobDeveloperBannerID;

#else
                //adView.AdUnitId = Context.Resources.GetString(Resource.String.banner_ad_unit_id);
                adView.AdUnitId = Constants.AdMobAndroidProductionBannerID;
#endif

                var requestbuilder = new AdRequest.Builder();
                adView.LoadAd(requestbuilder.Build());
                SetNativeControl(adView);
            }
        }
    }
    //public AdMobRenderer(Context context) : base(context) { }
    //    public class AdMobRenderer : ViewRenderer
    //    {
    //        public AdMobRenderer(Context context) : base(context) { }

    //        string adUnitId = string.Empty;
    //        //Note you may want to adjust this, see further down.
    //        AdSize adSize = AdSize.SmartBanner;
    //        AdView adView;
    //        AdView CreateNativeAdControl()
    //        {
    //            if (adView != null)
    //                return adView;

    //#if DEBUG
    //            // This is a string in the Resources/values/strings.xml that I added or you can modify it here. This comes from admob and contains a / in it
    //            adUnitId = Context.Resources.GetString(Resource.String.banner_ad_unit_id);
    //#else
    //            adUnitId = Constants.AdMobDeveloperBannerID;
    //#endif




    //            adView = new AdView(Context);
    //            adView.AdSize = adSize;
    //            adView.AdUnitId = adUnitId;

    //            var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

    //            adView.LayoutParameters = adParams;

    //            adView.LoadAd(new AdRequest
    //                            .Builder()
    //                            .Build());
    //            return adView;
    //        }

    //        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
    //        {
    //            base.OnElementChanged(e);
    //            if (Control == null)
    //            {
    //                CreateNativeAdControl();
    //                SetNativeControl(adView);
    //            }
    //        }
    //    }
}
