namespace Hrms.Core.Utilities
{
    public static class AppSettings
    {
        public static string AppBaseUrl { get; set; }

        public static class Jwt
        {
            public static string Secret { get; set; }
            public static string Audience { get; set; }
            public static string Issuer { get; set; }
        }

        public static class SendGrid
        {
            public static string ApiKey { get; set; }
            public static string SenderName { get; set; }
            public static string SenderEmail { get; set; }
            public static string DefaultReceiver { get; set; }
        } 
    }
}
