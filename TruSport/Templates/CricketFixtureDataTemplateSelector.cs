using System;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Templates
{
    public class CricketFixtureDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CricketFixtureTemplate { get; set; }
        public DataTemplate CricketScoreFixtureTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                var template = ((CricketFixture)item).MatchInnings != null && ((CricketFixture)item).MatchInnings.Count > 0 ? CricketScoreFixtureTemplate : CricketFixtureTemplate;

                return template;
            }
            catch (Exception ex)
            {
                return CricketFixtureTemplate;
            }
        }
    }
}
