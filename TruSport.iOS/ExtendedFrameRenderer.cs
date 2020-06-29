using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UIKit;
using TruSport.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System.Drawing;

[assembly: ExportRenderer(typeof(Frame), typeof(ExtendedFrameRenderer))]
namespace TruSport.iOS
{
    public class ExtendedFrameRenderer : VisualElementRenderer<Frame>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                SetupLayer();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.BorderColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.HasShadowProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.CornerRadiusProperty.PropertyName)
                SetupLayer();
        }

        void SetupLayer()
        {
            float cornerRadius = Element.CornerRadius;

            if (cornerRadius == -1f)
                cornerRadius = 5f; // default corner radius

            Layer.CornerRadius = cornerRadius;

            if (Element.BackgroundColor == Xamarin.Forms.Color.Default)
                Layer.BackgroundColor = UIColor.White.CGColor;
            else
                Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();

            if (Element.HasShadow)
            {
                Layer.ShadowRadius = 5;
                Layer.ShadowColor = UIColor.Gray.CGColor;
                Layer.ShadowOpacity = 0.4f;
                Layer.ShadowOffset = new SizeF();
            }
            else
                Layer.ShadowOpacity = 0;

            if (Element.BorderColor == Xamarin.Forms.Color.Default)
                Layer.BorderColor = UIColor.Clear.CGColor;
            else
            {
                Layer.BorderColor = Element.BorderColor.ToCGColor();
                Layer.BorderWidth = 1;
            }

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }
    }
}

