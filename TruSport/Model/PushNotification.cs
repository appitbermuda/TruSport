using System;
using System.Collections.Generic;

namespace TruSport.Model
{
    public class PushNotification
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Tag { get; set; }
        public DateTime DeliveredTime { get; set; }
        public bool IsDelivered { get; set; }
        public string Error { get; set; }
    }
}
