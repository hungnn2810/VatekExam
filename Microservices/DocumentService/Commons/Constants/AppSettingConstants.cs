namespace DocumentService.Commons.Constants
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
            public const string DocumentDb = "Server=localhost;Database=VatekExam_Document;User Id=sa;Password=Hung2001@";
            public const string IdentityDb = "Server=localhost;Database=VatekExam_Identity;User Id=sa;Password=Hung2001@";
        }

        public static class S3Settings
        {
            public const string Endpoint = "ap-southeast-1";
            public const short BucketId = 1;
            public const string BucketName = "vatek-exam";
            public const string AccessKey = "AKIAZC52SKQNQHP6KJU7";
            public const string SecretKey = "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";
        }
    }
}

