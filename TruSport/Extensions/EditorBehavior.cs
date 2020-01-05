using System;
using System.ComponentModel;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Extensions
{
    public class EditorBehavior : Behavior<Editor>
    {
        private void OnEditorUnfocused(object sender, FocusEventArgs e)
        {
            var editor = sender as Editor;
            var bindingContext = editor.BindingContext as Player;

            int jerseyNumber = 0;
            bool parseNumber = Int32.TryParse(editor.Text, out jerseyNumber);

            if(parseNumber)
                bindingContext.JerseyNumber = jerseyNumber;
            else
                bindingContext.JerseyNumber = 0;
        }
    }

    public class ListViewBehavior : Behavior<SfListView>
    {
        string oldName;
        Editor Editor;
        Label Name;
        SfListView SfListView;
        protected override void OnAttachedTo(SfListView bindable)
        {
            SfListView = bindable;
            base.OnAttachedTo(bindable);
            SfListView.ItemDoubleTapped += SfListView_ItemDoubleTapped;
            SfListView.ItemHolding += SfListView_ItemHolding;
        }

        private void SfListView_ItemHolding(object sender, ItemHoldingEventArgs e)
        {
            SfListView.SelectedItemTemplate = new DataTemplate(() =>
            {
                return new ViewCell { View = CreateGroupHeaderDefaultTemplate() };
            });
        }

        private void SfListView_ItemDoubleTapped(object sender, ItemDoubleTappedEventArgs e)
        {
            SfListView.SelectedItemTemplate = new DataTemplate(() =>
            {
                return new ViewCell { View = CreateGroupHeaderDefaultTemplate() };
            });
        }

        private View CreateGroupHeaderDefaultTemplate()
        {
            var grid = new Grid();
            grid.ColumnSpacing = 10;
            var column0 = new ColumnDefinition() { Width = 40 };
            var column1 = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
            var column2 = new ColumnDefinition() { Width = 50 };
            var column3 = new ColumnDefinition() { Width = 50 };
            grid.ColumnDefinitions.Add(column0);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);
            grid.ColumnDefinitions.Add(column3);
            var okButton = new Button() { Text = "", TextColor = Color.DarkGreen, FontSize = 30, FontFamily = (string)App.Current.Resources["fontAwesomeSolidFamily"] };
            okButton.Clicked += Button_Clicked;
            var cancelbutton = new Button() { Text = "", TextColor = Color.DarkRed, FontSize = 30, FontFamily = (string)App.Current.Resources["fontAwesomeSolidFamily"] };
            cancelbutton.Clicked += Cancelbutton_Clicked;
            Editor = new Editor();
            Editor.VerticalOptions = LayoutOptions.CenterAndExpand;
            Editor.HorizontalOptions = LayoutOptions.FillAndExpand;
            Editor.Keyboard = Keyboard.Numeric;
            Editor.SetBinding(Editor.TextProperty, new Binding("JerseyNumber", BindingMode.Default));
            Editor.PropertyChanged += OnEditorPropertyChanged;
            Editor.Focused += Bindable_Focused;
            Editor.Unfocused += OnEditorUnfocused;
            Name = new Label();
            Name.VerticalOptions = LayoutOptions.CenterAndExpand;
            Name.TextColor = Color.White;
            Name.SetBinding(Label.TextProperty, new Binding("Name"));
            grid.Children.Add(Editor, 0, 0);
            grid.Children.Add(Name, 1, 0);
            grid.Children.Add(okButton, 2, 0);
            grid.Children.Add(cancelbutton, 3, 0);
            return grid;
        }

        private View CreateSelectedItemTemplate()
        {
            var grid = new Grid();
            grid.ColumnSpacing = 10;
            var column0 = new ColumnDefinition() { Width = 40 };
            var column1 = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
            var column2 = new ColumnDefinition() { Width = 50 };
            grid.ColumnDefinitions.Add(column0);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);
            var selectedLabel = new Label() { Text = "", TextColor = Color.HotPink, FontSize = 30, FontFamily = (string)App.Current.Resources["fontAwesomeSolidFamily"] };
            var jerseyNumber = new Label();
            jerseyNumber.HorizontalTextAlignment = TextAlignment.Center;
            jerseyNumber.TextColor = Color.White;
            jerseyNumber.VerticalOptions = LayoutOptions.CenterAndExpand;
            jerseyNumber.HorizontalOptions = LayoutOptions.CenterAndExpand;
            jerseyNumber.SetBinding(Label.TextProperty, new Binding("JerseyNumber", BindingMode.Default));
            Name = new Label();
            Name.VerticalOptions = LayoutOptions.CenterAndExpand;
            Name.TextColor = Color.White;
            Name.SetBinding(Label.TextProperty, new Binding("Name"));
            grid.Children.Add(Editor, 0, 0);
            grid.Children.Add(Name, 1, 0);
            grid.Children.Add(selectedLabel, 2, 0);
            return grid;
        }

        private void Cancelbutton_Clicked(object sender, EventArgs e)
        {
            Editor.Text = oldName;
            SfListView.SelectedItems.Clear();
            SfListView.SelectedItemTemplate = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //SfListView.SelectedItems.Clear();
            SfListView.SelectedItemTemplate = new DataTemplate(() =>
            {
                return new ViewCell { View = CreateSelectedItemTemplate() };
            });
        }

        private void Bindable_Focused(object sender, FocusEventArgs e)
        {
            var editor = sender as Editor;
            oldName = editor.Text;
        }

        private void OnEditorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Renderer")
                (sender as Editor).Focus();
        }

        private void OnEditorUnfocused(object sender, FocusEventArgs e)
        {
            var editor = sender as Editor;
            var bindingContext = editor.BindingContext as Player;

            int jerseyNumber = 0;
            Int32.TryParse(editor.Text, out jerseyNumber);

            bindingContext.JerseyNumber = jerseyNumber;
        }

        protected override void OnDetachingFrom(SfListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.PropertyChanged -= OnEditorPropertyChanged;
            bindable.Unfocused -= OnEditorUnfocused;
        }
    }
}
