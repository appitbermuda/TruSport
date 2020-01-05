using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnTrackWebService.Interfaces
{
    public interface IPushNotificationRepository<T>
    {
        Task<string> Send(string name, string title, string body);
    }
}
