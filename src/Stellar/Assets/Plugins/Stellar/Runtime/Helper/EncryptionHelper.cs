using System;
using System.Security.Cryptography;

namespace Stellar.Runtime.Helper
{
    public static class EncryptionHelper
    {
        public static string ComputeSHA256Hmac(string message, string secretKey)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);

            using var hmacSha256 = new HMACSHA256(keyBytes);
            byte[] hashBytes = hmacSha256.ComputeHash(messageBytes);
            string computedHmac = Convert.ToBase64String(hashBytes);

            return computedHmac;
        }

    }
}