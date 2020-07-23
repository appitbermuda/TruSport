using System;
using System.Collections.Generic;

namespace OnTrackWebService.Models.CricHQ
{
    public class crichq
    {
        public List<item> items { get; set; }
    }

    public class item
    {
        public int id { get; set; }
        public string Name { get; set; }

        public team_one team_one {get;set;}
        public team_two team_two { get; set; }
        public club_one club_one { get; set; }
        public club_two club_two { get; set; }
        public List<dates> dates { get; set; }
        public List<match_result> match_result { get; set; }
    }

    public class team_one
    {
        public int id { get; set; }
        public string Name { get; set; }

        public club club { get; set; }
        public List<score> scores { get; set; }
    }

    public class team_two
    {
        public int id { get; set; }
        public string Name { get; set; }

        public club club { get; set; }
        public List<score> scores { get; set; }
    }

    public class club_one
    {
        public int id { get; set; }
        public string Name { get; set; }

        public team team { get; set; }
    }

    public class club_two
    {
        public int id { get; set; }
        public string Name { get; set; }

        public team team { get; set; }
    }

    public class team
    {
        public int id { get; set; }
        public string Name { get; set; }

        public List<player> players { get; set;}
        public List<inning> innings { get; set; }
    }

public class player
{
    public int id { get; set; }
    public string Name { get; set; }
}

    public class inning
    {
        public int over_balls { get; set; }
        public int overs { get; set; }
        public int runs { get; set; }
        public int wickets { get; set; }
    }

    public class club
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    public class dates
    {
        public int id { get; set; }
        public DateTime start_date { get; set; }
    }

    public class organisation
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    public class match_result
    {
        public int id { get; set; }
        public match match { get; set; }

        public team_one team_one { get; set; }
        public team_two team_two { get; set; }
    }

    public class match
    {
        public int id { get; set; }
        public bool manual { get; set; }
    }

    public class score
    {
        public int runs { get; set; }
        public int wickets { get; set; }
        public int overs { get; set; }
        public int over_balls { get; set; }
        public int overs_display { get; set; }
    }
}
