using System;
using CoreAnimation;
using UIKit;
using TruSport.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(ExtendedNavigationPageRenderer))]
namespace TruSport.iOS
{
    public class ExtendedNavigationPageRenderer : NavigationRenderer
    {
        public override void PushViewController(UIViewController viewController, bool animated)
        {
            if (animated)
            {
                // Alternative way with different set of trannsition
                /*
                UIView.Animate(0.75, () =>
                {
                    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
                    base.PushViewController(viewController, false);
                    UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, this.View, false);
                });
                 */
                var transition = CATransition.CreateAnimation();
                //transition.Duration = 0.75;
                transition.Type = CAAnimation.TransitionFromBottom;

                View.Layer.AddAnimation(transition, null);
                base.PushViewController(viewController, false);
            }
            else
            {
                base.PushViewController(viewController, false);
            }
        }

        public override UIViewController PopViewController(bool animated)
        {
            if (animated)
            {
                // Alternative way with different set of trannsition
                /*                UIView.Animate(0.75, () =>
                {
                    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
                    UIView.SetAnimationTransition(UIViewAnimationTransition.CurlDown, this.View, false);
                });
                */

                var transition = CATransition.CreateAnimation();
                //transition.Duration = 0.75;
                transition.Type = CAAnimation.TransitionFromTop;

                View.Layer.AddAnimation(transition, null);

                return base.PopViewController(false);
            }
            else
            {
                return base.PopViewController(false);
            }
        }

        public override void WillMoveToParentViewController(UIViewController parent)
        {

            base.WillMoveToParentViewController(parent);

            try
            {
                if(parent != null)
                {
                    if(UIDevice.CurrentDevice.CheckSystemVersion(13,0))
                    {
                        parent.ModalInPresentation = true;
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}