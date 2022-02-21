using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruSport.Views.Basketball
{
    public class BasketballMasterDetailPageMenuItem
    {
        public BasketballMasterDetailPageMenuItem()
        {
            TargetType = typeof(BasketballMainPage);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }
        public string Group { get; set; }
    }
}
