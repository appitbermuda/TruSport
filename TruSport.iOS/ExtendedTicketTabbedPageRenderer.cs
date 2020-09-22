using System;
using TruSport.iOS;
using TruSport.Views.Tickets;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TicketTabbedPage), typeof(ExtendedTicketTabbedPageRenderer))]
namespace TruSport.iOS
{
    public class ExtendedTicketTabbedPageRenderer : TabbedRenderer
    {
        private TicketTabbedPage _page;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _page = (TicketTabbedPage)e.NewElement;
            }
            else
            {
                _page = (TicketTabbedPage)e.OldElement;
            }

            try
            {
                var tabbarController = (UITabBarController)this.ViewController;
                if (null != tabbarController)
                {
                    tabbarController.ViewControllerSelected += OnTabbarControllerItemSelected;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void OnTabbarControllerItemSelected(object sender, UITabBarSelectionEventArgs eventArgs)
        {
            if (_page?.CurrentPage?.Navigation != null && _page.CurrentPage.Navigation.NavigationStack.Count > 0)
            {
                await _page.CurrentPage.Navigation.PopToRootAsync();
            }

        }
    }
}
