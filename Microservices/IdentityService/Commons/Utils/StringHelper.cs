using System;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Commons.Utils
{
    public class StringHelper
    {
        public static string HashPassword(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}

