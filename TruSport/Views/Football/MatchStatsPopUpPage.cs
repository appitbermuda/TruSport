using System;
using TruSport.Data;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public class MatchStatsPopUpPage : ContentPage
    {
        ScrollView MainScrollView;
        AbsoluteLayout MainAbsoluteLayout;
        Frame MainFrame;
        StackLayout MainStack;
        Grid StatGrid;
        Grid GoalButtonGrid;
        Grid YellowButtonGrid;
        Grid RedButtonGrid;

        StackLayout PlayerStack;
        Label PlayerName;

        Frame GoalFrame;
        Frame YellowCardFrame;
        Frame RedCardFrame;

        Image GoalImage;
        Image YellowCardImage;
        Image RedCardImage;

        Label GoalLabel;
        Label YellowCardLabel;
        Label RedCardLabel;

        Button GoalButton;
        Button YellowCardButton;
        Button RedCardButton;

        Image CloseButton;

        ContentView MainContentView;

        PlayerListView thisPlayer;
        MatchListView thisMatch;

        

        public MatchStatsPopUpPage(PlayerListView player, MatchListView match, bool HomeTeam)
        {
            thisPlayer = player;
            thisMatch = match;

            

            StatGrid = new Grid()
            {
                //Padding = new Thickness(10, 10, 10, 10),
                RowSpacing = 10,
                //ColumnSpacing = 5,
                //BackgroundColor = (Color)App.Current.Resources["primaryDarkBlue"],
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    //new ColumnDefinition { Width = new GridLength (40, GridUnitType.Absolute)},
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star)}
                }
            };

            GoalButtonGrid = new Grid()
            {
                ColumnSpacing = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (40, GridUnitType.Absolute)},
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star)}
                }
            };

            YellowButtonGrid = new Grid()
            {
                ColumnSpacing = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (40, GridUnitType.Absolute)},
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star)}
                }
            };

            RedButtonGrid = new Grid()
            {
                ColumnSpacing = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (40, GridUnitType.Absolute)},
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star)}
                }
            };

            MainScrollView = new ScrollView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            MainAbsoluteLayout = new AbsoluteLayout();

            MainFrame = new Frame
            {
                
                Margin = 15,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = (Color)App.Current.Resources["primaryGray"],
            };

            GoalFrame = new Frame
            {
                Padding = new Thickness(10, 0, 10, 0),
                HasShadow = false,
                CornerRadius = 25,
                IsClippedToBounds = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = (Color)App.Current.Resources["primaryButtonBlue"],
            };

            YellowCardFrame = new Frame
            {
                Padding = new Thickness(10, 0, 10, 0),
                HasShadow = false,
                CornerRadius = 25,
                IsClippedToBounds = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = (Color)App.Current.Resources["primaryButtonBlue"],
            };

            RedCardFrame = new Frame
            {
                Padding = new Thickness(10,0,10,0),
                HasShadow = false,
                CornerRadius = 25,
                IsClippedToBounds = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = (Color)App.Current.Resources["primaryButtonBlue"],
            };

            MainStack = new StackLayout
            {
                IsClippedToBounds = true,
                Padding = new Thickness(10,5),
                Spacing = 20
            };

            PlayerStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center
            };

            MainContentView = new ContentView();
            var closeTapGestureRecognizer = new TapGestureRecognizer();
            closeTapGestureRecognizer.Tapped += async (sender, e) => 
            {
                //await PopupNavigation.Instance.PopAsync();
            };
            MainContentView.GestureRecognizers.Add(closeTapGestureRecognizer);

            GoalImage = new Image
            {
                Source = "FootballWhite48.png",
                HeightRequest = 40,
                WidthRequest = 40,
                HorizontalOptions = LayoutOptions.Center
            };

            GoalLabel = new Label
            {
                Text = "GOAL",
                FontSize = 15,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            GoalButton = new Button
            {
                Text = "GOAL",
                FontSize = 15,
                TextColor = Color.White,
                BackgroundColor = (Color)App.Current.Resources["primaryButtonBlue"],
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            var goalTapGesture = new TapGestureRecognizer();
            goalTapGesture.Tapped += async (sender, e) => 
            {
                thisPlayer.Goals = thisPlayer.Goals + 1;

                if (HomeTeam)
                    thisMatch.HomeTeamScore = thisMatch.HomeTeamScore + 1;
                else
                    thisMatch.AwayTeamScore = thisMatch.AwayTeamScore + 1;

                //databaseManager.UpdatePlayer(thisPlayer);
                //databaseManager.UpdateMatch(thisMatch);

                //await PopupNavigation.Instance.PopAsync();
            };

            GoalFrame.GestureRecognizers.Add(goalTapGesture);
            //GoalImage.GestureRecognizers.Add(goalTapGesture);

            YellowCardImage = new Image
            {
                Source = "YellowCard_75px.png",
                HeightRequest = 40,
                WidthRequest = 40,
                HorizontalOptions = LayoutOptions.Center
            };

            YellowCardLabel = new Label
            {
                Text = "YELLOW CARD",
                FontSize = 15,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            YellowCardButton = new Button
            {
                Text = "YELLOW CARD",
                FontSize = 15,
                TextColor = Color.White,
                BackgroundColor = (Color)App.Current.Resources["primaryButtonBlue"],
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            var yellowCardTapGesture = new TapGestureRecognizer();
            yellowCardTapGesture.Tapped += async (sender, e) =>
            {
                thisPlayer.YellowCards = thisPlayer.YellowCards + 1;

                if (HomeTeam)
                    thisMatch.HomeYellowCards = thisMatch.HomeYellowCards + 1;
                else
                    thisMatch.AwayYellowCards = thisMatch.AwayYellowCards + 1;

                //databaseManager.UpdatePlayer(thisPlayer);
                //databaseManager.UpdateMatch(thisMatch);

                //await PopupNavigation.Instance.PopAsync();
            };

            YellowCardLabel.GestureRecognizers.Add(yellowCardTapGesture);
            YellowCardImage.GestureRecognizers.Add(yellowCardTapGesture);

            RedCardImage = new Image
            {
                Source = "RedCard_75px.png",
                HeightRequest = 40,
                WidthRequest = 40,
                HorizontalOptions = LayoutOptions.Center
            };

            RedCardLabel = new Label
            {
                Text = "RED CARD",
                FontSize = 15,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            RedCardButton = new Button
            {
                Text = "RED CARD",
                FontSize = 15,
                TextColor = Color.White,
                BackgroundColor = (Color)App.Current.Resources["primaryButtonBlue"],
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            var redCardTapGesture = new TapGestureRecognizer();
            redCardTapGesture.Tapped += async (sender, e) =>
            {
                thisPlayer.Goals = thisPlayer.Goals + 1;

                if (HomeTeam)
                    thisMatch.HomeRedCards = thisMatch.HomeRedCards + 1;
                else
                    thisMatch.AwayRedCards = thisMatch.AwayRedCards + 1;

                //databaseManager.UpdatePlayer(thisPlayer);
                //databaseManager.UpdateMatch(thisMatch);

                //await PopupNavigation.Instance.PopAsync();
            };

            RedCardLabel.GestureRecognizers.Add(redCardTapGesture);
            RedCardImage.GestureRecognizers.Add(redCardTapGesture);

            CloseButton = new Image
            {
                Source = "Close_White64.png",
                HeightRequest = 20,
                WidthRequest = 20
            };

            PlayerName = new Label
            {
                Text = thisPlayer.FirstName + " " + thisPlayer.LastName,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            };

            PlayerStack.Children.Add(PlayerName);

            GoalButtonGrid.Children.Add(GoalImage, 0, 0);
            GoalButtonGrid.Children.Add(GoalLabel, 1, 0);

            GoalFrame.Content = GoalButtonGrid;

            YellowButtonGrid.Children.Add(YellowCardImage, 0, 0);
            YellowButtonGrid.Children.Add(YellowCardLabel, 1, 0);

            YellowCardFrame.Content = YellowButtonGrid;

            RedButtonGrid.Children.Add(RedCardImage, 0, 0);
            RedButtonGrid.Children.Add(RedCardLabel, 1, 0);

            RedCardFrame.Content = RedButtonGrid;

            StatGrid.Children.Add(GoalFrame, 0, 0);
            StatGrid.Children.Add(YellowCardFrame, 0, 1);
            StatGrid.Children.Add(RedCardFrame, 0, 2);

            MainStack.Children.Add(PlayerStack);
            MainStack.Children.Add(StatGrid);

            MainFrame.Content = MainStack;

            MainContentView.Content = CloseButton;

            MainAbsoluteLayout.Children.Add(MainFrame);
            MainAbsoluteLayout.Children.Add(MainContentView, new Rectangle(1,0,-1,-1),AbsoluteLayoutFlags.PositionProportional);

            MainScrollView.Content = MainAbsoluteLayout;

            Content = MainScrollView;
        }
    }
}

