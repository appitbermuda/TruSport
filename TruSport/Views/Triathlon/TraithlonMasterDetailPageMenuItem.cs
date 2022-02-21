using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruSport.Views.Triathlon
{
    public class TriathlonMasterDetailPageMenuItem
    {
        public TriathlonMasterDetailPageMenuItem()
        {
            TargetType = typeof(TriathlonMainPage);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }
        public string Group { get; set; }
    }
}
