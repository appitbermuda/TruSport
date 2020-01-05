using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using NodaTime;
using RestSharp;
using Syncfusion.DataSource.Extensions;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Converters
{
        public class TeamIDConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return "Team Name";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

    public class EntryValidationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Color.Red;

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class ShowScoreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var score = (int?)value;

            if (score == null)
                return "V";

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class IsUpcomingFixtureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((DateTime)value > DateTime.Now)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SeasonFixtureDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var season = parameter as Label;

            var seasonDate = (DateTime)value;

            if (season.Text == "2019")
                return seasonDate.ToString("dd MMM");

            return seasonDate.Year.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsPastFixtureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((DateTime)value > DateTime.Now)
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return App.ScreenWidth - 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MatchSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((int)value == 0)
                    return false;
                else
                    return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SummaryMinuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((int)value == -1)
                    return "";
                else
                    return value.ToString() + "'";
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FavouriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "";
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FavouriteFontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return App.Current.Resources["fontAwesomeSolidFamily"];
            else
                return App.Current.Resources["fontAwesomeLightFamily"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StarterTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Starter";
            else
                return "Sub";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StarterBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Color.FromHex("ff00b4");
            else
                return Color.FromHex("ff77d7");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OwnGoalTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Yes";
            else
                return "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OwnGoalBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Color.FromHex("ff00b4");
            else
                return Color.FromHex("ff77d7");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ButtonSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return (Color)App.Current.Resources["primaryPink"];
            else
                return (Color)App.Current.Resources["primaryDarkBlueTwo"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTeamProfileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedTeamID = parameter as Label;
            var teamID = (string)value;

            if(selectedTeamID.Text == teamID)
                return FontAttributes.Bold;

            return FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class SelectedTeamResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = (string)value;

            if (result == "")
                return Color.Transparent;
            else if (result == "W")
                return Color.LimeGreen;
            else if (result == "D")
                return Color.FromHex("ffbf00");
            else
                return Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class SelectedTeamPenaltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedTeamID = parameter as Label;
            var teamID = (string)value;

            if (selectedTeamID.Text == teamID)
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class SelectedAwayTeamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedTeamID = parameter as Label;
            var teamID = (string)value;

            if(selectedTeamID.Text == teamID)
                return FontAttributes.Bold;

            return FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class YellowCardConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if((int)value == 1)
                    return "YellowCard_75px.png";
                else if ((int)value == 2)
                    return "YellowCards_75px.png";

                return "";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

        public class RedCardConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if ((int)value == 1)
                    return "RedCard_75px.png";

                return "";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

        public class GoalConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if ((int)value > 0)
                    return "FootballWhite48.png";
                else
                    return "";

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

        public class GameTimeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int thisValue = System.Convert.ToInt32((string)value);

                if (thisValue > 45 && thisValue < 60)
                    return "HT";
                else if (thisValue > 60)
                    return (thisValue - 15).ToString() + "'";
                else
                    return (string)value + "'";

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

    public class MatchTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                var matchDateLabel = parameter as Label;

                string time = (string)value;
                TimeSpan timeValue = new TimeSpan();

                if (TimeSpan.TryParse(time, out timeValue))
                {
                    DateTime dateTime = new DateTime();

                    if (DateTime.TryParse(matchDateLabel.Text, out dateTime))
                    {
                        dateTime = (dateTime.Date.Add(timeValue)).ToLocalTime();

                        //dateTime = dateTime.AddMinutes(90);

                        if (dateTime.AddMinutes(105) < DateTime.Now)
                            return "FT";
                        //else //if(dateTime > DateTime.Now)

                        return dateTime.ToString("h:mm tt");
                    }
                    //else
                    //{
                    //    //var gameTime = (timeValue.Add(TimeSpan.FromMinutes(105)) - DateTime.Now.TimeOfDay).TotalMinutes;
                    //    var gameTime = Math.Round((DateTime.Now.TimeOfDay - timeValue).TotalMinutes);

                    //    //int gameTimeValue = System.Convert.ToInt32(gameTime);

                    //    if (gameTime > 45 && gameTime < 60)
                    //        return "HT";
                    //    else if (gameTime > 60)
                    //        return (gameTime - 15).ToString() + "'";
                    //    else
                    //        return gameTime + "'";
                    //}
                }

                return "FT";
            }
            catch (Exception ex)
            {

            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class TeamNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Label aliasLabel = parameter as Label;

            string thisValue = (string)value;

            return aliasLabel.Text == "" || aliasLabel.Text == null ? thisValue : aliasLabel.Text;

            ////if(thisValue == fixture.HomeTeam.Name)
            //{
            //    if (fixture.HomeTeam.Alias != null && !String.IsNullOrEmpty(fixture.HomeTeam.Alias))
            //        return fixture.HomeTeam.Alias;
            //    else
            //        return fixture.HomeTeam.Name;
            //}
            //else
            //{
            //    if (fixture.AwayTeam.Alias != null && !String.IsNullOrEmpty(fixture.AwayTeam.Alias))
            //        return fixture.AwayTeam.Alias;
            //    else
            //        return fixture.AwayTeam.Name;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }

    public class UTCTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(DateTime))
            {
                return DateTime.MinValue;
            }

            if (parameter == null || parameter.GetType() != typeof(string) || !DateTimeZoneProviders.Tzdb.Ids.Contains((string)parameter))
            {
                return value;
            }

            //TimeZoneInfo timeZoneInfo = new TimeZoneInfo();

            var timeZone = DateTimeZoneProviders.Tzdb[(string)parameter];
            var utcDateTime = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
            var zonedDateTime = Instant.FromDateTimeUtc(utcDateTime).InZone(timeZone).ToDateTimeUnspecified();

            return zonedDateTime;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }

    }

    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //DateTime? thisValue = (DateTime)value;
            Label postponed = parameter as Label;

            if (postponed != null)
            {
                var isPostponed = System.Convert.ToBoolean(postponed.Text);

                if(isPostponed)
                    return "Postp.";
            }

            var time = value as string;

            if (time != null)
            {
                TimeSpan timeValue = new TimeSpan();

                if (TimeSpan.TryParse(time, out timeValue))
                {
                    //DateTime dateTime = DateTime.Now.Date;
                    DateTime dateTime = new DateTime().AddDays(5);

                    dateTime = dateTime.Date + timeValue;

                    TimeZone zone = TimeZone.CurrentTimeZone;
                    TimeSpan offset = zone.GetUtcOffset(DateTime.Now);
                    TimeSpan utcoffset = zone.GetUtcOffset(dateTime);

                    return dateTime.ToLocalTime().ToString("h:mm tt");
                }
                else
                    return null;
            }
            else
            {
                TimeSpan timeValue = (TimeSpan)value;
                return timeValue.ToString("h:mm tt");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }


    

    public class NewsCreatorCoverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string creator = (string)value;

                if (creator.Contains("Bernews"))
                    return "Bernews";
                else
                    return (string)value;

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

        public class TeamSheetConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if ((bool)value)
                    return "STARTERS";
                else
                    return "SUBS";

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

        public class FavouriteTypeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if ((string)value == "Team")
                    return "TeamWhite36.png";
                else
                    return "FootballWhite24.png";

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

        public class FavouriteValueConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string[] valueArray;
                string thisValue = (string)value;
                if (thisValue.Contains(";"))
                {
                    valueArray = thisValue.Split(';');

                    return valueArray[1];
                }
                else
                    return (string)value;

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return int.Parse((string)value);
            }
        }

        public class GroupHeaderConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime dateTime = new DateTime();

                var isDate = DateTime.TryParse(value.ToString(), out dateTime);

                if (targetType.Name == "Color")
                {
                    if (isDate)
                        return (Color)App.Current.Resources["primaryDarkBlue"];
                    else
                        //return (Color)App.Current.Resources["primaryBlue"];
                        return Color.Transparent;
                }
                else
                {
                    if (isDate)
                        return new Thickness(5, 5, 5, 0);
                    else
                        return new Thickness(15, 5, 5, 0);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class GroupHeaderNameConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime dateTime = new DateTime();

            //GroupResult groupResult = value as GroupResult;

            string[] result = value.ToString().Split(',');


            var isDate = DateTime.TryParse(result[0], out dateTime);

                if (isDate)
                {
                    return dateTime.ToString("D");
                }
                else
                {
                    return (string)result[0];
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class GroupHeaderSizeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime dateTime = new DateTime();


                var isDate = DateTime.TryParse(value.ToString(), out dateTime);

                if (isDate)
                {
                    return 18;
                }
                else
                {
                    return 14;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class GroupHeaderFontConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime dateTime = new DateTime();


                var isDate = DateTime.TryParse(value.ToString(), out dateTime);

                if (isDate)
                {
                    return FontAttributes.Bold;
                }
                else
                {
                    return FontAttributes.Italic | FontAttributes.Bold;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class SelectedTeamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return FontAttributes.Bold;
            else
                return FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTeamBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return (Color)App.Current.Resources["primaryPink"];
            else
                return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTeamTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if ((bool)value)
            //    //return Color.White;
            //    return (Color)App.Current.Resources["primaryDarkBlue"];
            //else
                return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupHeaderReversedSizeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime dateTime = new DateTime();


                var isDate = DateTime.TryParse(value.ToString(), out dateTime);

                if (isDate)
                {
                    return 14;
                }
                else
                {
                    return 18;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class GroupHeaderReversedFontConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime dateTime = new DateTime();


                var isDate = DateTime.TryParse(value.ToString(), out dateTime);

                if (isDate)
                {
                    return FontAttributes.Italic | FontAttributes.Bold;
                }
                else
                {
                    return FontAttributes.Bold;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class GroupingStarterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            //DateTime dateTime = new DateTime();

            GroupResult groupResult = value as GroupResult;

            //SfListView listview = parameter as SfListView;

            //string[] result = groupResult.Key.ToString().Split(',');

            //var groupdataitems = (value as GroupResult).Items.ToList<FixtureListView>().ToList();
            var items = new List<Player>(groupResult.Items.ToList<Player>());
            var data = items[0];

            //if (parameter is Label)
            //    return data.GroupingData.Designation;
            //else
            //return data.GroupingData.EmployeeImage;

            
            if (data.IsStarter)
                return "Starting Line-Up";
            else
                return "Substitutes";
            
            //else
            //{
            //    return data.Date.ToString("MMM d");
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupingSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            //DateTime dateTime = new DateTime();

            GroupResult groupResult = value as GroupResult;

            //SfListView listview = parameter as SfListView;

            //string[] result = groupResult.Key.ToString().Split(',');

            //var groupdataitems = (value as GroupResult).Items.ToList<FixtureListView>().ToList();
            var items = new List<Fixture>(groupResult.Items.ToList<Fixture>());
            var data = items[0];

            //if (parameter is Label)
            //    return data.GroupingData.Designation;
            //else
            //return data.GroupingData.EmployeeImage;

            if (parameter is Label)
            {
                if (!data.MatchType.IsTable)
                    return data.League.Name + " " + data.MatchType.Name;
                else
                    return data.League.Name;
            }
            else
            {
                if (data.Season.IsCurrent)
                    return data.Date.ToString("MMM d");
                else
                    return data.Date.Year.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupingTransferSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            //DateTime dateTime = new DateTime();

            GroupResult groupResult = value as GroupResult;

            //SfListView listview = parameter as SfListView;

            //string[] result = groupResult.Key.ToString().Split(',');

            //var groupdataitems = (value as GroupResult).Items.ToList<FixtureListView>().ToList();
            var items = new List<Transfers>(groupResult.Items.ToList<Transfers>());
            var data = items[0];

            return data.PreviousTeam;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupingLeagueDateSelectionConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            DateTime dateTime = new DateTime();

            GroupResult groupResult = value as GroupResult;

            //SfListView listview = parameter as SfListView;

            //string[] result = groupResult.Key.ToString().Split(',');

            //var groupdataitems = (value as GroupResult).Items.ToList<FixtureListView>().ToList();

            if (groupResult != null)
            {
                var items = new List<Fixture>(groupResult.Items.ToList<Fixture>());
                var data = items[0];

                //if (parameter is Label)
                //    return data.GroupingData.Designation;
                //else
                //return data.GroupingData.EmployeeImage;

                if (parameter is Label)
                {
                    return data.MatchType.Name;
                }
                else
                {
                    return data.Date.ToString("dd MMM yyyy");
                }
            }

            var isDate = DateTime.TryParse(value.ToString(), out dateTime);

            if (isDate)
                return dateTime.ToString("dd MMM yyyy");
            else
                return (string)value;
                //return data.MatchType.Name + " - " + data.Date.ToString("MMM d, yyyy");
            }
        

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class GroupingTeamConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null)
                    return value;

                GroupResult groupResult = value as GroupResult;
                var items = new List<Team>(groupResult.Items.ToList<Team>());
                var data = items[0];

                return data.League.Name;

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class GroupingPlayerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            GroupResult groupResult = value as GroupResult;
            var items = new List<PlayerSeason>(groupResult.Items.ToList<PlayerSeason>());
            var data = items[0];

            return data.Team.Name;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IndexToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var listview = parameter as SfListView;
                var index = listview.DataSource.DisplayItems.IndexOf(value);

                if (index % 2 == 0)
                    return (Color)App.Current.Resources["primaryDarkBlue"];
                return (Color)App.Current.Resources["primaryLightBlue"];
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value != null && (string)value != "")
            {
                //var client = new RestClient("http://ontrackimagestore.blob.core.windows.net/images/");
                //var request = new RestRequest((string)value, Method.GET);
                ////request.AddHeader("authorization", "Bearer " + accessToken);

                //// We execute the request and capture the response
                //// in a variable called `response`
                //IRestResponse response = client..Execute(request);

                //if (response.IsSuccessful)
                //{
                //    string image = JsonConvert.DeserializeObject<string>(response.Content);

                //    return image;
                //}
                //return "ontrack.png";
                var image = "http://ontrackimagestore.blob.core.windows.net/images/" + (string)value;


                return image;
            }
            else
                return "ontrack.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TableIndexToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var listview = parameter as SfListView;
                var index = listview.DataSource.DisplayItems.IndexOf(value);

                if (index % 2 == 0)
                    //return Color.White;
                    return (Color)App.Current.Resources["primaryDarkBlueTwo"];

                //return "#ECEFF1";
                return (Color)App.Current.Resources["primaryDarkBlue"];

                //return (Color)App.Current.Resources["primaryDarkBlueOne"];
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class IDToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var id = (int)value;

                if (id % 2 == 0)
                    return (Color)App.Current.Resources["primaryDarkBlue"];
                return (Color)App.Current.Resources["primaryLightBlue"];
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class IsValidatedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isValidated = (bool)value;

            if (!isValidated)
                return (Color)App.Current.Resources["primaryAccentPink"];

            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBooleanConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var thisValue = (bool)value;

                return !thisValue;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class EventArgsConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                object eventArgs = null;
                if (value is Syncfusion.ListView.XForms.ItemTappedEventArgs)
                    eventArgs = value as Syncfusion.ListView.XForms.ItemTappedEventArgs;
                else if (value is ListViewLoadedEventArgs)
                    eventArgs = parameter;
                else if (value is ItemSelectionChangedEventArgs)
                    eventArgs = value as ItemSelectionChangedEventArgs;
                else if (value is SwipingEventArgs)
                    eventArgs = value as SwipingEventArgs;
                else if (value is Syncfusion.SfPicker.XForms.SelectionChangedEventArgs)
                    eventArgs = value as Syncfusion.SfPicker.XForms.SelectionChangedEventArgs;
                else if (value is Syncfusion.XForms.ComboBox.SelectionChangedEventArgs)
                    eventArgs = value as Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;
                else if (value is ToggledEventArgs)
                    eventArgs = value as ToggledEventArgs;
            else if (value is Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs)
                eventArgs = value as Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs;
            return eventArgs;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
}
