using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UIKit;
using TruSport.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(ExtendedPageRenderer))]
namespace TruSport.iOS
{
	public class ExtendedPageRenderer : PageRenderer
	{
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			var contentPage = this.Element as ContentPage;
			if (contentPage == null || NavigationController == null)
			{
				return;
			}

			var itemsInfo = contentPage.ToolbarItems;

			var navigationItem = this.NavigationController.TopViewController.NavigationItem;
			var leftNativeButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
			var rightNativeButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
			var rightNativeButtonsNew = new UIBarButtonItem[] { }.ToList();

			rightNativeButtons.ForEach(nativeItem =>
			{
				// [Hack] Get Xamarin private field "item"
				var field = nativeItem.GetType().GetField("_item", BindingFlags.NonPublic | BindingFlags.Instance);
				if (field == null)
				{
					rightNativeButtonsNew.Add(nativeItem);
					return;
				}

				var info = field.GetValue(nativeItem) as ToolbarItem;
				if (info != null && info.Priority != 0)
				{
					rightNativeButtonsNew.Add(nativeItem);
					return;
				}

				//var info = GetButtonInfo(itemsInfo, nativeItem.Title);

				//if (info == null || info.Priority != 0)
				//{
				//	if (info.Priority == 1)
				//		nativeItem.Style = UIBarButtonItemStyle.Done;

				//	return;
				//}

				//rightNativeButtons.Remove(nativeItem);
				leftNativeButtons.Add(nativeItem);
			});

			navigationItem.RightBarButtonItems = rightNativeButtonsNew.ToArray();
			//navigationItem.RightBarButtonItems = rightNativeButtons.ToArray();
			navigationItem.LeftBarButtonItems = leftNativeButtons.ToArray();
		}

		private ToolbarItem GetButtonInfo(IList<ToolbarItem> items, string name)
		{
			if (string.IsNullOrEmpty(name) || items == null)
				return null;

			return items.ToList().Where(itemData => name.Equals(itemData.Name)).FirstOrDefault();
		}

        public override void WillMoveToParentViewController(UIViewController parent)
        {

            base.WillMoveToParentViewController(parent);

            try
            {
                if (parent != null)
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    {
                        parent.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                        parent.ModalInPresentation = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}

