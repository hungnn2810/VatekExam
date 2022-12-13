namespace IdentityService.Commons.Constants
{
    public static class AppSettingConstants
    {
        public static class IdentityServer
        {
            public const string DefaultScheme = "Bearer";
            public const string Authority = "https://localhost:4001";
            public const string TokenEndpoint = "https://localhost:4001/connect/token";
            public const string ClientId = "client_default";
            public const string ClientSecret = "client_secret";
            public const string ApiName = "microservices";
            public const string ApiSecret = "api_secret";
        }

        public static class ConnectionStrings
        {
            public const string IdentityDb = "Server=localhost;Database=VatekExam_Identity;User Id=sa;Password=Hung2001@";
        }
    }
}

