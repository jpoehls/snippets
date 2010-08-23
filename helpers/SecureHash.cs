using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Samples.Security
{
    public class SecureHash : IHashAlgorithm
    {
        #region IHashAlgorithm Members

        public string ComputeHash(string plainText, string salt)
        {
            string plainTextAndSalt = salt + plainText;

            byte[] plainTextAndSaltBytes = Encoding.UTF8.GetBytes(plainTextAndSalt);

            HashAlgorithm hash = new SHA256Managed();

            byte[] hashBytes = hash.ComputeHash(plainTextAndSaltBytes);
            string hashValue = ToHexadecimalString(hashBytes);

            return hashValue;
        }

        #endregion

        private static string ToHexadecimalString(ICollection<byte> bytes)
        {
            var sb = new StringBuilder(bytes.Count * 2);
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}