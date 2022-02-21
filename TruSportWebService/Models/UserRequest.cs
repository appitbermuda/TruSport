using System;
namespace OnTrackWebService.Models
{
    public class UserRequest
    {
        public string ID { get; set; }
        public string RoleID { get; set; }
        public string TeamID { get; set; }
        public string TicketCompanyID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TemporaryPassword { get; set; }
    }
}
