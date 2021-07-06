using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruSport.Views.Bowling
{
    public class BowlingMasterDetailPageMenuItem
    {
        public BowlingMasterDetailPageMenuItem()
        {
            TargetType = typeof(BowlingMasterDetailPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }
    }
}
