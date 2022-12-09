namespace Application.Common.Constants
{
    public static class AppSettingConstants
    {
        public static class IdentityServer
        {
            public const string DefaultScheme = "Bearer";
            public const string Authority = "https://localhost:4001";
            public const string TokenEndpoint = "https://localhost:4001/connect/token";
            public const string ClientId = "client_default";
            public const string ClientSecret = "gwvpWXnCoARIz7pZ5PbRzkiqJRcC51UbFBuQh3UTGdo=";
            public const string ApiSecret = "AQtyl6gQNd0cNR+ax1LRL5ZZqtkE9OGL/N0lyrZrxe4=";
        }

        public static class ConnectionStrings
        {
            public const string IdentityDbConnection = "Server=localhost;Database=VatekExam_Identity;User Id=sa;Password=Hung2001@";
        }
    }
}

