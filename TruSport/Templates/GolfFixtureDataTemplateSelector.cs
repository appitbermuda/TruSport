using System;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Templates
{
    public class GolfFixtureDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GolfFixtureTemplate { get; set; }
        public DataTemplate GolfScoreFixtureTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                //return ((GolfFixture)item).Scores.HomeTeamScore.HasValue && ((GolfFixture)item).Match.AwayTeamScore.HasValue ? GolfScoreFixtureTemplate : GolfFixtureTemplate;
                return GolfFixtureTemplate;
            }
            catch (Exception ex)
            {
                return GolfFixtureTemplate;
            }

        }
    }
}
