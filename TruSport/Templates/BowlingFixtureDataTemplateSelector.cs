using System;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Templates
{
    public class BowlingFixtureDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BowlingFixtureTemplate { get; set; }
        public DataTemplate BowlingScoreFixtureTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                var template = ((BowlingFixture)item).BowlingGames != null && ((BowlingFixture)item).BowlingGames.Count > 0 ? BowlingScoreFixtureTemplate : BowlingFixtureTemplate;

                return template;
            }
            catch (Exception ex)
            {
                return BowlingFixtureTemplate;
            }
        }
    }
}
