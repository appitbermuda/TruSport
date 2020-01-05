using TruSport.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ExtendedViewCellRenderer))]
namespace TruSport.iOS
{
    public class ExtendedViewCellRenderer : ViewCellRenderer
    {

        public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            var view = item as ViewCell;
            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = UIColor.FromRGB(0, 145, 234)
            };
            switch (item.StyleId)
            {
                case "none":
                    cell.Accessory = UIKit.UITableViewCellAccessory.None;
                    cell.SelectionStyle = UIKit.UITableViewCellSelectionStyle.None;
                    break;
                case "checkmark":
                    cell.Accessory = UIKit.UITableViewCellAccessory.Checkmark;
                    break;
                case "detail-button":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DetailButton;
                    break;
                case "detail-disclosure-button":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DetailDisclosureButton;
                    break;
                case "disclosure":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DisclosureIndicator;
                    //cell.BackgroundColor = UIKit.UIColor.Gray;
                    break;
                case "none-selected":
                default:
                    cell.Accessory = UIKit.UITableViewCellAccessory.None;
                    break;
            }
            return cell;
        }

    }
}
