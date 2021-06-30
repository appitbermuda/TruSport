using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class SportEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Season { get; set; }
        public string SportID { get; set; }
        public string FieldID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string EventLogo { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }

        [ForeignKey("FieldID")]
        public Field Field { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }

        [NotMapped]
        public int TicketsAvailable { get; set; }

        [NotMapped]
        public string PostponedOrCancelled => IsPostponed ? "Postponed" : IsCancelled ? "Cancelled" : String.Empty;

        [NotMapped]
        public DateTime EventTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? Date.AddDays(1).Add(TimeSpan.Parse(Time)) : Date.Add(TimeSpan.Parse(Time));

    }
}
