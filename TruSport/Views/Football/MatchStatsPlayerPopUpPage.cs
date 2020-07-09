using System;
using TruSport.Data;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public class MatchStatsPlayerPopUpPage : ContentPage
    {
        ScrollView MainScrollView;
        AbsoluteLayout MainAbsoluteLayout;
        Frame MainFrame;
        StackLayout MainStack;

        StackLayout StatTypeStack;
        Label StatLabel;

        StackLayout PlayerListStack;
        ListView PlayerList;

        Image GoalImage;
        Image YellowCardImage;
        Image RedCardImage;

        Image CloseButton;

        ContentView MainContentView;

        string thisStatType;
        string thisTeamID;

        
        //PlayerManager playerManager;
        public MatchStatsPlayerPopUpPage(string statType, string teamID)
        {
            thisStatType = statType;
            thisTeamID = teamID;
            //playerManager = PlayerManager.DefaultManager;
            

            MainScrollView = new ScrollView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            MainAbsoluteLayout = new AbsoluteLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            MainFrame = new Frame
            {
                WidthRequest = App.ScreenWidth - 100,
                HeightRequest = App.ScreenHeight - 200,
                Margin = new Thickness(15),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = (Color)App.Current.Resources["primaryGray"],
            };

            MainStack = new StackLayout
            {
                IsClippedToBounds = true,
                Padding = new Thickness(10,5),
                Spacing = 20
            };

            StatTypeStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center
            };

            PlayerListStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            MainContentView = new ContentView();
            var closeTapGestureRecognizer = new TapGestureRecognizer();
            closeTapGestureRecognizer.Tapped += async (sender, e) => 
            {
                
            };
            MainContentView.GestureRecognizers.Add(closeTapGestureRecognizer);

            PlayerList = new ListView
            {
                BackgroundColor = Color.Transparent,
                HasUnevenRows = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(CustomRosterCell))
            };

            GoalImage = new Image
            {
                Source = "FootballWhite48.png",
                HeightRequest = 40,
                WidthRequest = 40,
                HorizontalOptions = LayoutOptions.Center
            };

            YellowCardImage = new Image
            {
                Source = "YellowCard_75px.png",
                HeightRequest = 40,
                WidthRequest = 40,
                HorizontalOptions = LayoutOptions.Center
            };

            RedCardImage = new Image
            {
                Source = "RedCard_75px.png",
                HeightRequest = 40,
                WidthRequest = 40,
                HorizontalOptions = LayoutOptions.Center
            };

            CloseButton = new Image
            {
                Source = "Close_White64.png",
                HeightRequest = 20,
                WidthRequest = 20
            };

            StatLabel = new Label
            {
                Text = thisStatType,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            };

            BoxView boxView = new BoxView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 1,
                BackgroundColor = Color.White
            };

            StatTypeStack.Children.Add(StatLabel);

            PlayerListStack.Children.Add(PlayerList);

            MainStack.Children.Add(StatTypeStack);
            MainStack.Children.Add(boxView);
            MainStack.Children.Add(PlayerListStack);

            MainFrame.Content = MainStack;

            MainContentView.Content = CloseButton;

            MainAbsoluteLayout.Children.Add(MainFrame);
            MainAbsoluteLayout.Children.Add(MainContentView, new Rectangle(1,0,-1,-1),AbsoluteLayoutFlags.PositionProportional);

            //MainScrollView.Content = MainAbsoluteLayout;

            Content = MainAbsoluteLayout;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //PlayerList.ItemsSource = await databaseManager.GetPlayerByTeam(thisTeamID);
            //PlayerList.ItemsSource = await playerManager.GetPlayersByTeamIDAsync(thisTeamID, false);
        }

        public class CustomRosterCell : ViewCell
        {
            public CustomRosterCell()
            {
                this.StyleId = "none";

                StackLayout mainStackLayout = new StackLayout
                {
                    //BackgroundColor = (Color)App.Current.Resources["primaryListBlue"],
                    //BackgroundColor = Color.Red,
                    Padding = new Thickness(10, 5)
                };

                Frame mainFrame = new Frame
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    CornerRadius = 10,
                    HasShadow = false,
                    Padding = 0,
                    BackgroundColor = (Color)App.Current.Resources["primaryCell"],
                };

                Grid grid = new Grid
                {
                    Padding = new Thickness(0),
                    RowSpacing = 1,
                    ColumnSpacing = 1,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Star) },
                    //new RowDefinition { Height = new GridLength (40, GridUnitType.Absolute) },
                    //new RowDefinition { Height = new GridLength (24, GridUnitType.Absolute) },
                    },
                    ColumnDefinitions = {
                    //new ColumnDefinition { Width = new GridLength (40, GridUnitType.Absolute) },
                    //new ColumnDefinition { Width = new GridLength (2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    }
                };

                //Image CashImage = new Image()
                //{
                //    Source = "Cash32.png"
                //};

                Label PlayerNumberLabel = new Label()
                {
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White
                };
                PlayerNumberLabel.SetBinding(Label.TextProperty, "JerseyNumber");

                Label PlayerFirstNameLabel = new Label()
                {
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White
                };
                PlayerFirstNameLabel.SetBinding(Label.TextProperty, "FirstName");

                Label PlayerLastNameLabel = new Label()
                {
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White
                };
                PlayerLastNameLabel.SetBinding(Label.TextProperty, "LastName");



                var JerseyNumberStack = new StackLayout
                {
                    Padding = new Thickness(5, 2, 5, 2),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                };

                var PlayerNameStack = new StackLayout
                {
                    Padding = new Thickness(0),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal
                };


                JerseyNumberStack.Children.Add(PlayerNumberLabel);
                PlayerNameStack.Children.Add(PlayerFirstNameLabel);
                PlayerNameStack.Children.Add(PlayerLastNameLabel);

                //grid.Children.Add(JerseyNumberStack, 0, 0);
                grid.Children.Add(PlayerNameStack, 0, 0);

                //mainFrame.Content = grid;
                mainStackLayout.Children.Add(grid);

                // add to parent view
                View = mainStackLayout;
            }
        }
    }
}

