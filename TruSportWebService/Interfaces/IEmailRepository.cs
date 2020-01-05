using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnTrackWebService.Interfaces
{
    public interface IEmailRepository<T>
    {
        Task SendSignUpEmail(string name, string email, string role, string team);
    }
}
