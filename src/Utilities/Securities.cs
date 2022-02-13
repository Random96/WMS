using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ru.EmlSoft.Utilities
{
    public static class Securities
    {
        public static string ToMd5(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(str);

            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

                return data.ToHexString();
            }
        }

        public static string ToMd52(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(str);

            using (var md5Hasher = new MD5CryptoServiceProvider())
            {
                var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
                return data.ToHexString();
            }
        }

        public static string ToSha512(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(str);

            using (var shaM = SHA512.Create())
            {
                //SHA512 shaM = new SHA512Managed();
                var result = shaM.ComputeHash(str.ToBytes());
                return result.ToHexString();
            }
        }

        public static string ToSha1(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(str);

            using (var shaM = SHA1.Create())
            {
                //SHA1 shaM = new SHA1Managed();
                var result = shaM.ComputeHash(str.ToBytes());
                return result.ToHexString();
            }
        }

        public static string ToHexString(this byte[] bytes)
        {
            var sBuilder = new StringBuilder();
            foreach (var t in bytes)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}