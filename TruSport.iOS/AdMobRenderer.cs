using System;
using System.ComponentModel;
using CoreGraphics;
using Google.MobileAds;
using iAd;
using TruSport.iOS;
using TruSport.Model;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace TruSport.iOS
{
    public class AdMobRenderer : ViewRenderer
    {
        BannerView adView;
        bool viewOnScreen;

        //        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        //        {
        //            base.OnElementChanged(e);

        //            if (e.NewElement == null)
        //                return;

        //            if (e.OldElement == null)
        //            {
        //                adView = new BannerView(AdSizeCons.SmartBannerPortrait)
        //                {
        //#if DEBUG
        //                    AdUnitID = Constants.AdMobDeveloperBannerID,
        //#else
        //                    AdUnitID = Constants.AdMobProductionBannerID,
        //#endif
        //                    RootViewController = GetRootViewController()
        //                };

        //                adView.AdReceived += (sender, args) =>
        //                {
        //                    if (!viewOnScreen) this.AddSubview(adView);
        //                    viewOnScreen = true;
        //                };


        //                var request = Request.GetDefaultRequest();
        //                e.NewElement.HeightRequest = GetSmartBannerDpHeight();
        //                e.NewElement.BackgroundColor = Color.Transparent;
        //                //(Color)App.Current.Resources["primaryDarkBlue"];
        //                adView.LoadRequest(request);

        //                base.SetNativeControl(adView);

        //            }
        //        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                BannerView bannerView = null;

                switch ((Element as AdMobView).Size)
                {
                    case AdMobView.Sizes.Standardbanner:
                        bannerView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                    case AdMobView.Sizes.LargeBanner:
                        bannerView = new BannerView(AdSizeCons.LargeBanner, new CGPoint(0, 0));
                        break;
                    case AdMobView.Sizes.MediumRectangle:
                        bannerView = new BannerView(AdSizeCons.MediumRectangle, new CGPoint(0, 0));
                        break;
                    case AdMobView.Sizes.FullBanner:
                        bannerView = new BannerView(AdSizeCons.FullBanner, new CGPoint(0, 0));
                        break;
                    case AdMobView.Sizes.Leaderboard:
                        bannerView = new BannerView(AdSizeCons.Leaderboard, new CGPoint(0, 0));
                        break;
                    case AdMobView.Sizes.SmartBannerPortrait:
                        bannerView = new BannerView(AdSizeCons.SmartBannerPortrait, new CGPoint(0, 0));
                        break;
                    default:
                        bannerView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                }

#if DEBUG
                bannerView.AdUnitID = Constants.AdMobDeveloperBannerID;
                //AdUnitID = Constants.AdMobDeveloperBannerID,
#else
                bannerView.AdUnitId = Constants.AdMobiOSProductionBannerID;
                //AdUnitID = Constants.AdMobProductionBannerID;
#endif

                foreach (UIWindow uiWindow in UIApplication.SharedApplication.Windows)
                {
                    if (uiWindow.RootViewController != null)
                    {
                        bannerView.RootViewController = uiWindow.RootViewController;
                    }
                }
                var request = Request.GetDefaultRequest();
                bannerView.LoadRequest(request);
                SetNativeControl(bannerView);
            }

        }

        private UIViewController GetRootViewController()
        {
            foreach (UIWindow window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController != null)
                {
                    return window.RootViewController;
                }
            }

            return null;
        }

        private int GetSmartBannerDpHeight()
        {
            var dpHeight = (double)UIScreen.MainScreen.Bounds.Height;

            if (dpHeight <= 400) return 32;
            if (dpHeight > 400 && dpHeight <= 720) return 50;
            return 90;
        }
    }
}
