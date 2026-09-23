using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace NewShop.Helpers
{
    public class HmacHelper
    {
        private static readonly string SecretKey = ConfigurationManager.AppSettings["HMAC_SECRET_KEY"];

        /// <summary>
        /// Generate HMAC SHA256
        /// </summary>
        public static string GenerateSignature(
            int productId,
            decimal price) {
            string message =
                productId.ToString() + "|" +
                price.ToString("0.00");

            byte[] key =
                Encoding.UTF8.GetBytes(SecretKey);

            byte[] data =
                Encoding.UTF8.GetBytes(message);

            using (HMACSHA256 hmac =
                new HMACSHA256(key)) {
                byte[] hash =
                    hmac.ComputeHash(data);

                StringBuilder sb =
                    new StringBuilder();

                foreach (byte b in hash) {
                    sb.Append(
                        b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Verify Signature
        /// </summary>
        public static bool VerifySignature(
            int productId,
            decimal price,
            string signature) {
            string newSignature =
                GenerateSignature(
                    productId,
                    price);

            return SlowEquals(
                newSignature,
                signature);
        }

        /// <summary>
        /// ป้องกัน Timing Attack
        /// </summary>
        private static bool SlowEquals(
            string a,
            string b) {
            if (a == null || b == null)
                return false;

            if (a.Length != b.Length)
                return false;

            int diff = 0;

            for (int i = 0; i < a.Length; i++) {
                diff |= a[i] ^ b[i];
            }

            return diff == 0;
        }
    }
}