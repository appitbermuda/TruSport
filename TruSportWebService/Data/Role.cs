using System;
namespace OnTrackWebService.Data
{
    public static class Roles
    {
        public const string AllUsers = "Administrator,Team Manager (Football),Team Manager (Cricket),Score Administrator (Football),Score Administrator (Cricket),Match Commissioner (Football),TicketBuyer";
        public const string Customer = "TicketBuyer";
        public const string TicketAdmin = "Ticketing Administrator (Football)";
        public const string Admin = "Administrator";
        public const string TeamAdmin = "Administrator,Team Manager (Football),Team Manager (Cricket)";
        public const string ScoreAdmin = "Administrator,Score Administrator (Football),Score Administrator (Cricket)";
        public const string MatchCommissioner = "Administrator,Match Commissioner (Football)";
    }
}
