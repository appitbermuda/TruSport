using System;
namespace TruSport.Data
{
    public interface INotificationActionService
    {
        void TriggerAction(string action);
    }
}
