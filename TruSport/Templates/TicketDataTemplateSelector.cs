using System;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.Templates
{
    public class TicketDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TicketFixtureTemplate { get; set; }
        public DataTemplate TicketEventTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                return (!String.IsNullOrEmpty(((SportEvent)item).HomeTeam) && (!String.IsNullOrEmpty(((SportEvent)item).AwayTeam))) ? TicketFixtureTemplate : TicketEventTemplate;
            }
            catch (Exception ex)
            {
                return TicketEventTemplate;
            }

        }
    }
}
