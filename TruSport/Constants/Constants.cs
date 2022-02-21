using System;
namespace TruSport
{
    public static class Constants
    {
        public static string Cycling = "Cycling";
        public static string Golf = "Golf";
        public static string Hockey = "Hockey";
        public static string Rugby = "Rugby";
        public static string Swimming = "Swimming";
        public static string TrackAndField = "TrackAndField";
        public static string Bowling = "Bowling";
        public static string Cricket = "Cricket";
        public static string Tennis = "Tennis";
        public static string Football = "Football";
        public static string Basketball = "Basketball";
        public static string Triathlon = "Triathlon";
        public static string CryptoPW = "Ontr@ckTick3tingPlatf0rm2020!";
        public static string OnTrackEmail = "sports@ontrackbda.com";
        public static string OnTrackTicketingEmail = "tickets@ontrackbda.com";
        //public static string OnTrackEmail = "ontrackbda@gmail.com";
        public const string ApplicationTicketURL = "https://www.ontrackbda.com/Ticket/";
        public const string ApplicationURL = "www.ontrackbda.com";
        public static string AdMobDeveloperBannerID = "ca-app-pub-3940256099942544/2934735716";
        public static string AdMobiOSProductionBannerID = "ca-app-pub-1338169805120312/4328088630";
        public static string AdMobAndroidProductionBannerID = "ca-app-pub-1338169805120312/3198424508";
        public const string Url = "https://api.appcenter.ms/v0.1/apps/";
        public const string ApiKeyName = "X-API-Token";
        public const string ApiKey = "906a76bb025114bb6f621d1950b42cfb54601d45";
        public const string Organization = "techReef";
        public const string Android = "OnTrack";
        public const string IOS = "OnTrack";
        public const string DeviceTarget = "devices_target";
        public class Apis { public const string Notification = "push/notifications"; }
        public static string Administrator = "Administrator,Team Administrator,Match Commissioner";
        public static string MatchCommissioner = "Match Commissioner";
        public static string TeamAdministrator = "Team Administrator";
        public static string NotificationChannelName { get; set; } = "OnTrackNotifyChannel";
        
        //public static string ListenConnectionString { get; set; } = "Endpoint=sb://uridepush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=ESGAUJ/HbYDTmU9g8l4ECSoffmBOndBB3TEML0/QRCA=";
        public static string DebugTag { get; set; } = "OnTrackNotify";
        public static string[] SubscriptionTags { get; set; } = { "default", "football", "cricket" };
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

#if DEBUG
        public const string APIEndpoint = "http://localhost:40139/api/";
        //public const string APIEndpoint = "https://ontrackservicetest.azurewebsites.net/api/";
        //public const string APIEndpoint = "https://ontrackservice.azurewebsites.net/api/";
        //public const string ChatEndpoint = "http://localhost:26585/chatHub";
        public static string NotificationHubName { get; set; } = "OnTrackDevPushHub";
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://ontrackdevpush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=TPTAMlyAdTjqLH7eemrlPunhzKEy5yt3BMkxosyrkSY=";
#else
        public const string APIEndpoint = "https://ontrackservice.azurewebsites.net/api/";
        //public const string APIEndpoint = "https://ontrackservicetest.azurewebsites.net/api/";
        public static string NotificationHubName { get; set; } = "OnTrackPushHub";
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://ontrackpush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=P0I0U/tizyUD2mhlZnAH+VvNEuOUSZr722a3X+J2414=";
        //public static string NotificationHubName { get; set; } = "ontrackdevprodpush";
        //public static string ListenConnectionString { get; set; } = "Endpoint=sb://ontrackdevpush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=nsIXDAEutaeMsrisPfd0mnYAZNYGggsKpLE9JAjMRKs=";
#endif
    }

}
