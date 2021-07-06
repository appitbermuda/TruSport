using System;
using TruSport.Model;

namespace TruSport.Data
{
    public interface IPushNotificationActionService : INotificationActionService
    {
        event EventHandler<PushAction> ActionTriggered;
    }
}
