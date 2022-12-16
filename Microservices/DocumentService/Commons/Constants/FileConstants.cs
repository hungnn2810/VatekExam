using System;
namespace DocumentService.Commons.Constants
{
    public static class FileConstants
    {
        public static readonly string[] AllowFileExtensions = new string[] { "doc", "docx", "pdf", "xlsx" };

        public static string GetFileKey(string ext) => $"{ext.ToLower()}/{Guid.NewGuid()}";

        public static long MaxSize => 10485760;
    }
}

