using System;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Templates
{
    public class BasketballFixtureDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BasketballFixtureTemplate { get; set; }
        public DataTemplate BasketballScoreFixtureTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                return ((BasketballFixture)item).Match.HomeTeamScore.HasValue && ((BasketballFixture)item).Match.AwayTeamScore.HasValue ? BasketballScoreFixtureTemplate : BasketballFixtureTemplate;
            }
            catch (Exception ex)
            {
                return BasketballFixtureTemplate;
            }

        }
    }
}
