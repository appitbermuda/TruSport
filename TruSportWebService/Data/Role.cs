using System;
namespace OnTrackWebService.Data
{
    public static class Roles
    {
        public const string AllUsers = "Administrator,Team Manager (Football),Team Manager (Cricket),Score Administrator (Football),Score Administrator (Cricket),Match Commissioner (Football),TicketBuyer,Ticketing Administrator (Football),Ticketing Owner (Football)";
        public const string Tickets = "Administrator,TicketBuyer,Ticketing Administrator (Football),Ticketing Owner (Football)";
        public const string Customer = "Administrator,TicketBuyer";
        public const string TicketAdmin = "Administrator,Ticketing Administrator (Football),Ticketing Owner (Football),Ticketing Administrator (Cricket),Ticketing Owner (Cricket)";
        public const string TicketScanner = "Ticketing Administrator (Football),Ticketing Owner (Football),Ticketing Administrator (Cricket),Ticketing Owner (Cricket)";
        public const string TicketOwner = "Administrator,Ticketing Owner (Football),Ticketing Owner (Cricket)";
        public const string Admin = "Administrator";
        public const string TeamAdmin = "Administrator,Team Manager (Football),Team Manager (Cricket)";
        public const string ScoreAdmin = "Administrator,Score Administrator (Football),Score Administrator (Cricket)";
        public const string MatchCommissioner = "Administrator,Match Commissioner (Football)";
    }
}
