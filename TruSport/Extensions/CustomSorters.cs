using System;
using System.Collections.Generic;
using TruSport.Model;

namespace TruSport.Extensions
{
    public class CustomSortComparer : IComparer<object>
    {
        public int Compare(object x, object y)
        {
            if (x.GetType() == typeof(Fixture))
            {
                var xitem = (x as Fixture).Date.Add(TimeSpan.Parse((x as Fixture).Time)).ToLocalTime();
                var yitem = (y as Fixture).Date.Add(TimeSpan.Parse((x as Fixture).Time)).ToLocalTime();

                if (xitem > yitem)
                {
                    return 1;
                }
                else if (xitem < yitem)
                {
                    return -1;
                }
                else
                {
                    //if (string.Compare(xitem, yitem) == -1)
                    //    return -1;
                    //else if (string.Compare(xitem, yitem) == 1)
                    //    return 1;
                }
            }

            return 0;
        }
    }
}
