using Stellar.Runtime.Helper;

namespace Stellar.Runtime.Extension
{
    public static class EncryptionExtensions
    {
        public static string ComputeSHA256Hmac(this string message, string secretKey)
        {
            return EncryptionHelper.ComputeSHA256Hmac(message, secretKey);
        }
    }
}