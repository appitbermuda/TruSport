using System;
namespace TruSport.Views.Golf
{
    public class GolfMasterDetailPageMenuItem
    {
        public GolfMasterDetailPageMenuItem()
        {
            TargetType = typeof(GolfMainPage);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }
        public string Group { get; set; }
    }
}
