using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UIKit;
using TruSport.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace TruSport.iOS
{
	public class ExtendedTabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            //TabBar.TintColor = UIColor.FromRGB(42, 52, 141);
            ////TabBar.TintColor = UIColor.White;
            ////TabBar.BarTintColor = UIColor.White;
            //TabBar.BarTintColor = UIColor.FromRGB(248, 248, 248);
            ////TabBar.BarTintColor = UIColor.Clear;
            ////TabBar.BarTintColor = UIColor.FromRGB(29, 36, 98);
            ////TabBar.BackgroundColor = UIColor.White;
            ////TabBar.BackgroundColor = UIColor.White;
            ////TabBar.BackgroundColor = UIColor.Black;
            ////TabBar.BackgroundColor = UIColor.FromRGB(29, 36, 98);
            //TabBar.BackgroundColor = UIColor.FromRGB(248, 248, 248);
        }
    }
}

