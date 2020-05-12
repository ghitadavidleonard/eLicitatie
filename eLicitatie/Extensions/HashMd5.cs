using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eLicitatie.Api.Extensions
{
    public static class HashMd5
    {
        public static string GetMd5Hash(this string source)
        {
            var md5Hash = MD5.Create();

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var c in data)
            {
                stringBuilder.Append(c.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static bool VerifyMd5Hash(this string source, string hash)
        {
            string hashOfSource = GetMd5Hash(source);

            StringComparer comparer = StringComparer.Ordinal;

            if (comparer.Compare(hashOfSource, hash) == 0)
                return true;
            else
                return false;
        }
    }
}
