using System;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Templates
{
    public class FootballFixtureDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FootballFixtureTemplate { get; set; }
        public DataTemplate FootballScoreFixtureTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                return ((Fixture)item).Match.HomeTeamScore.HasValue && ((Fixture)item).Match.AwayTeamScore.HasValue ? FootballScoreFixtureTemplate : FootballFixtureTemplate;
            }
            catch (Exception ex)
            {
                return FootballFixtureTemplate;
            }
            
        }
    }
}
