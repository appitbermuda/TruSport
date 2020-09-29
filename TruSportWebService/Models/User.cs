using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string RoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TemporaryPassword { get; set; }
        public string TeamID { get; set; }

        [NotMapped]
        public string Token { get; set; }
        public bool IsValidated { get; set; }

        [ForeignKey("RoleID")]
        public Role Role { get; set; }

        //[ForeignKey("TeamID")]
        [NotMapped]
        public Team Team { get; set; }
    }

    public class AllUsers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserTypeID { get; set; }
        public string UserTypeName { get; set; }
        public string TeamID { get; set; }
        public string TeamName { get; set; }
        public bool IsValidated { get; set; }

        [ForeignKey("UserTypeID")]
        public UserType UserType { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }
    }

    public class UserResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string RoleID { get; set; }
        public string TeamID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [ForeignKey("RoleID")]
        public Role Role { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }
    }

    public class UserAuthentication
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
