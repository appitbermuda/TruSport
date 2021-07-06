using System;
namespace TruSport.Data
{
    public static class Bootstrap
    {
        public static void Begin(Func<IDeviceInstallationService> deviceInstallationService)
        {
            PushServiceContainer.Register(deviceInstallationService);

            PushServiceContainer.Register<IPushNotificationActionService>(()
                => new PushNotificationActionService());

            PushServiceContainer.Register<INotificationRegistrationService>(()
                => new NotificationRegistrationService());
        }
    }
}
