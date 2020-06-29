using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public class MatchRosterPopUpPage : ContentPage
    {
        ScrollView MainScrollView;
        AbsoluteLayout MainAbsoluteLayout;
        Frame MainFrame;
        StackLayout MainStack;
        Grid HeaderGrid;

        StackLayout StatTypeStack;
        Label StatLabel;

        StackLayout PlayerListStack;
        ListView HomeTeamPlayerList;
        ListView AwayTeamPlayerList;

        Image GoalImage;
        Image YellowCardImage;
        Image RedCardImage;

        Image CloseButton;

        Label PlayerTitle;
        Label StarterTitle;
        Label SubTitle;

        ContentView MainContentView;

        string thisMatchID;
        string thisTeamID;

        bool IsHomeTeam;

        
        

        RosterViewModel multiSelectViewModel;

        List<MatchRosterListView> matchRoster;

        public MatchRosterPopUpPage(string matchID, string teamID, bool isHomeTeam, List<SelectableData<MatchRosterListView>> rosterList)
        {
            thisMatchID = matchID;
            thisTeamID = teamID;
            IsHomeTeam = isHomeTeam;

            BindingContext = new RosterViewModel(rosterList);

            HeaderGrid = new Grid
            {
                Padding = new Thickness(0),
                RowSpacing = 1,
                ColumnSpacing = 1,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                new RowDefinition { Height = new GridLength (30, GridUnitType.Absolute) },
                new RowDefinition { Height = new GridLength (1, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Star) },
                //new RowDefinition { Height = new GridLength (40, GridUnitType.Absolute) },
                //new RowDefinition { Height = new GridLength (24, GridUnitType.Absolute) },
                },
                ColumnDefinitions = {
                //new ColumnDefinition { Width = new GridLength (40, GridUnitType.Absolute) },
                //new ColumnDefinition { Width = new GridLength (2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength (55, GridUnitType.Absolute) },
                new ColumnDefinition { Width = new GridLength (55, GridUnitType.Absolute) },
                }
            };

            

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
                Padding = new Thickness(5),
                WidthRequest = App.ScreenWidth - 50,
                HeightRequest = App.ScreenHeight - 200,
                Margin = new Thickness(15),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = (Color)App.Current.Resources["primaryGray"],
            };

            MainStack = new StackLayout
            {
                IsClippedToBounds = true,
                Padding = new Thickness(0),
                Spacing = 5
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

            HomeTeamPlayerList = new ListView
            {
                BackgroundColor = Color.Transparent,
                HasUnevenRows = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(CustomRosterCell)),
                Footer = this.BindingContext,
                FooterTemplate = new DataTemplate(typeof(CustomHomeTeamFooterCell))
            };
            HomeTeamPlayerList.SetBinding(ListView.ItemsSourceProperty, "RosterList");

            AwayTeamPlayerList = new ListView
            {
                BackgroundColor = Color.Transparent,
                HasUnevenRows = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(CustomRosterCell)),
                Footer = this.BindingContext,
                FooterTemplate = new DataTemplate(typeof(CustomAwayTeamFooterCell))
            };
            AwayTeamPlayerList.SetBinding(ListView.ItemsSourceProperty, "RosterList");

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
                Text = "SELECT 18",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            };

            PlayerTitle = new Label
            {
                Text = "PLAYER",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 9
            };

            StarterTitle = new Label
            {
                Text = "STARTER",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 9
            };

            SubTitle = new Label
            {
                Text = "SUB",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 9
            };

            BoxView boxView = new BoxView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 1,
                BackgroundColor = Color.White
            };

            StatTypeStack.Children.Add(StatLabel);

            if(isHomeTeam)
            {
                PlayerListStack.Children.Add(HomeTeamPlayerList);
            }
            else
            {
                PlayerListStack.Children.Add(AwayTeamPlayerList);
            }


            HeaderGrid.Children.Add(StatTypeStack, 0, 0);
            Grid.SetColumnSpan(StatTypeStack, 3);
            HeaderGrid.Children.Add(boxView, 0, 1);
            Grid.SetColumnSpan(boxView, 3);
            HeaderGrid.Children.Add(PlayerTitle, 0, 2);
            HeaderGrid.Children.Add(StarterTitle, 1, 2);
            HeaderGrid.Children.Add(SubTitle, 2, 2);

            //MainStack.Children.Add(StatTypeStack);
            MainStack.Children.Add(HeaderGrid);
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

            //matchRoster = databaseManager.SetMatchRosterByMatchByTeam(thisMatchID, thisTeamID);

            //PlayerList
                
                //.ItemsSource = databaseManager.SetMatchRosterByMatchByTeam(thisMatchID, thisTeamID);
            //PlayerList.ItemsSource = await playerManager.GetPlayersByTeamIDAsync(thisTeamID, false);
        }

        public class CustomHomeTeamFooterCell : ContentView
        {
            public CustomHomeTeamFooterCell()
            {
                this.StyleId = "none";

                StackLayout mainStackLayout = new StackLayout
                {
                    //BackgroundColor = (Color)App.Current.Resources["primaryListBlue"],
                    //BackgroundColor = Color.Red,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(10, 5)
                };

                Button SaveButtonn = new Button()
                {
                    Text = "SAVE",
                    BackgroundColor = Color.White,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = (Color)App.Current.Resources["primaryGray"],
                };
                SaveButtonn.SetBinding(Button.CommandProperty, "FinishHomeTeamCommand");

                //mainFrame.Content = grid;
                mainStackLayout.Children.Add(SaveButtonn);

                // add to parent view
                Content = mainStackLayout;
            }
        }

        public class CustomAwayTeamFooterCell : ContentView
        {
            public CustomAwayTeamFooterCell()
            {
                this.StyleId = "none";

                StackLayout mainStackLayout = new StackLayout
                {
                    //BackgroundColor = (Color)App.Current.Resources["primaryListBlue"],
                    //BackgroundColor = Color.Red,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(10, 5)
                };

                Button SaveButtonn = new Button()
                {
                    Text = "SAVE",
                    BackgroundColor = Color.White,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = (Color)App.Current.Resources["primaryGray"],
                };
                SaveButtonn.SetBinding(Button.CommandProperty, "FinishAwayTeamCommand");

                //mainFrame.Content = grid;
                mainStackLayout.Children.Add(SaveButtonn);

                // add to parent view
                Content = mainStackLayout;
            }
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
                    new ColumnDefinition { Width = new GridLength (55, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (55, GridUnitType.Absolute) },
                    }
                };

                //Image CashImage = new Image()
                //{
                //    Source = "Cash32.png"
                //};
                Switch subSwitch = new Switch()
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                subSwitch.SetBinding(Switch.IsToggledProperty, "SubSelected");

                Switch starterSwitch = new Switch()
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                starterSwitch.SetBinding(Switch.IsToggledProperty, "StarterSelected");

                Label PlayerNumberLabel = new Label()
                {
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White
                };
                PlayerNumberLabel.SetBinding(Label.TextProperty, "Data.JerseyNumber");

                Label PlayerNameLabel = new Label()
                {
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White
                };
                PlayerNameLabel.SetBinding(Label.TextProperty, "Data.PlayerName");

                //Label PlayerLastNameLabel = new Label()
                //{
                //    FontSize = 16,
                //    FontAttributes = FontAttributes.Bold,
                //    TextColor = Color.White
                //};
                //PlayerLastNameLabel.SetBinding(Label.TextProperty, "LastName");



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
                PlayerNameStack.Children.Add(PlayerNameLabel);
                //PlayerNameStack.Children.Add(PlayerLastNameLabel);

                //grid.Children.Add(JerseyNumberStack, 0, 0);
                grid.Children.Add(PlayerNameStack, 0, 0);
                grid.Children.Add(starterSwitch, 1, 0);
                grid.Children.Add(subSwitch, 2, 0);

                //mainFrame.Content = grid;
                mainStackLayout.Children.Add(grid);

                // add to parent view
                View = mainStackLayout;
            }
        }
    }
}

