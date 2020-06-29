using System;
using TruSport.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(ExtendedSearchBarRenderer))]
namespace TruSport.iOS
{
	public class ExtendedSearchBarRenderer : SearchBarRenderer
	{
        #region Properties

        private UIColor BorderColor = UIColor.Black;
        private int BorderWidth = 1;

        #endregion

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Text")			{

                if (App.AppTheme == "dark")
                {
                    Control.ShowsCancelButton = false;
                    Control.TintColor = UIColor.White;
                }
                else
                {
                    Control.ShowsCancelButton = false;
                    Control.TintColor = UIColor.DarkGray;
                }

                
			}
		}

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);
            var newElement = ((SearchBar)e.NewElement);
            //BorderColor = newElement..BorderColor.ToUIColor();
            //if (newElement.BorderWidth != 0)
            //{
            //    BorderWidth = newElement.BorderWidth;
            //}
            var searchbar = (UISearchBar)Control;
            if (e.NewElement != null)
            {
                //UITextField txSearchField = (UITextField)Control.ValueForKey(new Foundation.NSString("searchField"));

                if(App.AppTheme == "dark")
                    searchbar.SetImageforSearchBarIcon(UIImage.FromBundle("searchmini"), UISearchBarIcon.Search, UIControlState.Normal);
                //searchbar.BackgroundColor = UIColor.Clear;
                //var searchTextField = searchbar.ValueForKey((Foundation.NSString)"_searchField") as UITextField;
                //var clearButton = searchTextField.ValueForKey((Foundation.NSString)"_clearButton") as UIButton;
                //clearButton.SetImage(UIImage.FromBundle("SearchCloseIcon.png"), UIControlState.Normal);

                Foundation.NSString _searchField = new Foundation.NSString("searchField");
                var textFieldInsideSearchBar = (UITextField)searchbar.ValueForKey(_searchField);

                if (App.AppTheme == "dark")
                {
                    textFieldInsideSearchBar.BackgroundColor = UIColor.FromRGB(10, 10, 10);
                    searchbar.Layer.BackgroundColor = UIColor.FromRGB(0, 0, 0).CGColor;
                    textFieldInsideSearchBar.TextColor = UIColor.White;
                    searchbar.ShowsCancelButton = false;
                    searchbar.TintColor = UIColor.White;
                }
                else
                {
                    textFieldInsideSearchBar.BackgroundColor = UIColor.FromRGB(245, 245, 245);
                    searchbar.Layer.BackgroundColor = UIColor.FromRGB(255, 255, 255).CGColor;
                    textFieldInsideSearchBar.TextColor = UIColor.Black;
                    searchbar.ShowsCancelButton = false;
                    searchbar.TintColor = UIColor.DarkGray;
                    searchbar.BarTintColor = UIColor.DarkGray;
                }

                
                textFieldInsideSearchBar.TextAlignment = UITextAlignment.Left;
                //textFieldInsideSearchBar.BorderStyle = UITextBorderStyle.RoundedRect;
                //textFieldInsideSearchBar.Layer.BorderColor = UIColor.White.CGColor;
                //textFieldInsideSearchBar.Layer.BorderWidth = 1;
                //textFieldInsideSearchBar.Layer.CornerRadius = 10;

                //searchbar.TintColor = UIColor.White;
                //searchbar.BarTintColor = UIColor.White;

                //searchbar.BarTintColor = UIColor.White;
                //searchbar.SetImageforSearchBarIcon(UIImage.)
                //searchbar.Layer.CornerRadius = 0;
                //searchbar.Layer.BorderWidth = BorderWidth;
                //searchbar.Layer.BorderColor = BorderColor.CGColor;
                //searchbar.ShowsCancelButton = false;
            }
        }
	}
}
