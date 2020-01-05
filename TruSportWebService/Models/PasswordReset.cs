using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class PasswordReset
    {
        public string Email { get; set; }
        public string TemporaryPassword { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPassword
    {
        public string Email { get; set; }
    }
}
