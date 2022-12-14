using System;
namespace DocumentService.Commons.Constants
{
    public static class FileConstants
    {
        public static readonly string[] AllowFileExtensions = new string[] { "doc", "docx", "pdf" };

        public static string GetFileKey(string ext) => $"{ext.ToLower()}/{Guid.NewGuid()}";
    }
}

