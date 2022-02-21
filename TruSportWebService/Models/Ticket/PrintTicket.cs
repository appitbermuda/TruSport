using System;
using System.IO;

namespace OnTrackWebService.Models.Ticket
{
    public class PrintTicket
    {
        public Stream File { get; set; }
        public string FileName { get; set; }
    }
}
