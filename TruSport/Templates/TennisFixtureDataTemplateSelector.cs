using System;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Templates
{
    public class TennisFixtureDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TennisFixtureTemplate { get; set; }
        public DataTemplate TennisScoreFixtureTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                var template = ((TennisFixture)item).TennisMatch[0].TennisSets != null && ((TennisFixture)item).TennisMatch[0].TennisSets.Count > 0 ? TennisScoreFixtureTemplate : TennisFixtureTemplate;

                return template;
            }
            catch (Exception ex)
            {
                return TennisFixtureTemplate;
            }
        }
    }
}
