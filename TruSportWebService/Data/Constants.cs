using System;
namespace OnTrackWebService.Data
{
    public class Constants
    {
        public static string SETTING_CRICKET_TBD_ID = "932AEB43-1017-4615-BB4B-F88A913C2117";
        public static string SETTING_FOOTBALL_TBD_ID = "BA7124FE-A966-428A-A46A-64AF78B15D62";
        public static string SETTING_PROCESSING_FEE_ID = "E1D4E89C-B118-4155-9D8C-D82791879757";
        public static string SMTPServer = "SMTP Server";
        public static string SMTPPort = "SMTP Port";
        public static string SMTPUsername = "SMTP Username";
        public static string SMTPPassword = "SMTP Password";

        public static string BCCAddress = "ontrackbda@gmail.com";
        public static string NoReplyFromAddress = "tickets@ontrackbda.com";
        public const string Url = "https://api.appcenter.ms/v0.1/apps/";
        public const string ApiKeyName = "X-API-Token";
        public const string ApiKey = "906a76bb025114bb6f621d1950b42cfb54601d45";
        public static string OnTrackWebEndpoint = "https://www.ontrackbda.com";

        public const string CricHQApiKeyName = "api_token";
        public const string CricHQApiKey = "d0c6af03ac033fa8e8dafd411105c42e";
        public const string CricHQEndpoint = "https://www.crichq.com/api/v2/public/";
        public const string CricHQFixturesEndpoint = "match_center/upcoming?query=bermuda";
        public const string CricHQLiveEndpoint = "in_progress?query=bermuda&competition_match_level_id=0";

        public static string TicketingAdmin = "Ticketing Administrator (Football)";

        public static string ProcessingPW = "v2fAD5x3";
        public static string AcquirerId = "464748";
        public static string Currency = "840";
        public static string MerchantId = "33302800";
        public static string OrderNumberPrefix = "OT_TIX_";

        public const string Organization = "techReef";
        public const string Android = "OnTrack-1";
        public const string IOS = "OnTrack";
        public const string DeviceTarget = "devices_target";
        public class Apis { public const string Notification = "push/notifications"; }

        public const string ImageEndpoint = "https://ontrackimagestore.blob.core.windows.net/images/";

#if DEBUG
        //public static string OnTrackEndpoint = "http://localhost:40139/api/";
        //public static string OnTrackEndpoint = "https://ontrackservicetest.azurewebsites.net/api/";
        public static string OnTrackEndpoint = "https://ontrackservice.azurewebsites.net/api/";
        public static string[] SubscriptionTags { get; set; } = { "default", "cricket", "football" };
        //public static string NotificationHubName { get; set; } = "OnTrackDevPushHub";
        //public static string FullAccessConnectionString { get; set; } = "Endpoint=sb://ontrackdevpush.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=uusrAVZy6JrvlTLtYZHUEoVR7EKbtqRGTJ3t0A6q0yE=";
        public static string NotificationHubName { get; set; } = "ontrackpushhub";
        public static string FullAccessConnectionString { get; set; } = "Endpoint=sb://ontrackpush.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=UA17J5DQ+x/8zKbnOjhrQWdjEaXWPuZg1ijAmcj3gHs=";

#else
        //public static string OnTrackEndpoint = "https://ontrackservice.azurewebsites.net/api/";
        public static string OnTrackEndpoint = "https://ontrackservicetest.azurewebsites.net/api/";
        public static string[] SubscriptionTags { get; set; } = { "default", "cricket", "football" };
        public static string NotificationHubName { get; set; } = "ontrackpushhub";
        public static string FullAccessConnectionString { get; set; } = "Endpoint=sb://ontrackpush.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=UA17J5DQ+x/8zKbnOjhrQWdjEaXWPuZg1ijAmcj3gHs=";
        //public static string NotificationHubName { get; set; } = "ontrackdevprodpush";
        //public static string FullAccessConnectionString { get; set; } = "Endpoint=sb://ontrackdevpush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=nsIXDAEutaeMsrisPfd0mnYAZNYGggsKpLE9JAjMRKs=";
#endif
    }
}
