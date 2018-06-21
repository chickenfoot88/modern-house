using System;
using System.Security.Cryptography;
using System.Text;

namespace FileManager.FileSystem.Utils
{
    public static class Hasher
    {
        public static string GetHash(string data)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(data);

                var hashBytes = md5.ComputeHash(bytes);

                return new Guid(hashBytes).ToString("N");
            }    
        }
    }
}
