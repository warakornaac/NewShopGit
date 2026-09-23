// File: Security/PriceSignatureHelper.cs
using System;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace NewShop.Helpers
{
    public static class PriceSignatureHelper
    {
        private static readonly string SecretKey =
            ConfigurationManager.AppSettings["HMAC_SECRET_KEY"];

        private const int ExpiryMinutes = 60;

        // Unix Epoch = 1 Jan 1970 00:00:00 UTC
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string GenerateSignature(string productId, decimal price, long timestamp) {
            if (string.IsNullOrEmpty(SecretKey))
                throw new InvalidOperationException("HMAC_SECRET_KEY ยังไม่ได้ตั้งค่าใน Web.config");

            string raw = BuildRawString(productId, price, timestamp);

            var keyBytes = Encoding.UTF8.GetBytes(SecretKey);
            using (var hmac = new HMACSHA256(keyBytes)) {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// คำนวณ Unix timestamp (seconds) เอง แทน DateTimeOffset.ToUnixTimeSeconds()
        /// ซึ่งไม่รองรับใน .NET Framework 4.5
        /// </summary>
        public static long GetCurrentTimestamp() {
            TimeSpan diff = DateTime.UtcNow - UnixEpoch;
            return (long)diff.TotalSeconds;
        }

        public static bool VerifySignature(string productId, decimal price, long timestamp, string signature) {
            if (string.IsNullOrEmpty(signature)) return false;

            if (IsExpired(timestamp)) return false;

            long now = GetCurrentTimestamp();
            if (timestamp > now + 60) return false; // ป้องกัน clock drift / ปลอมเวลาอนาคต

            string expected = GenerateSignature(productId, price, timestamp);
            return SlowEquals(expected, signature);
        }

        private static bool IsExpired(long timestamp) {
            long now = GetCurrentTimestamp();
            long ageSeconds = now - timestamp;
            return ageSeconds > (ExpiryMinutes * 60);
        }

        private static string BuildRawString(string productId, decimal price, long timestamp) {
            return $"{productId}|{price.ToString("F2", CultureInfo.InvariantCulture)}|{timestamp}";
        }

        private static bool SlowEquals(string a, string b) {
            if (a.Length != b.Length) return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++) {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }
    }
}