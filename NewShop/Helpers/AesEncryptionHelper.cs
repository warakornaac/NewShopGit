using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NewShop.Helpers
{
    public static class AesEncryptionHelper
    {
        private static readonly string SecretKey = ConfigurationManager.AppSettings["AES_SECRET_KEY"];

        private static readonly string IV = ConfigurationManager.AppSettings["AES_IV"];

        /// <summary>
        /// Encrypt Plain Text
        /// </summary>
        public static string Encrypt(string plainText) {
            byte[] keyBytes = Encoding.UTF8.GetBytes(SecretKey);
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

            using (Aes aes = Aes.Create()) {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256;

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (MemoryStream ms = new MemoryStream()) {
                    using (CryptoStream cs =
                        new CryptoStream(ms,
                        encryptor,
                        CryptoStreamMode.Write)) {
                        using (StreamWriter sw =
                            new StreamWriter(cs)) {
                            sw.Write(plainText);
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt Plain Text
        /// (ใช้เฉพาะตอน Test)
        /// </summary>
        public static string Decrypt(string cipherText) {
            byte[] keyBytes = Encoding.UTF8.GetBytes(SecretKey);
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

            byte[] cipherBytes =
                Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create()) {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256;

                ICryptoTransform decryptor =
                    aes.CreateDecryptor();

                using (MemoryStream ms =
                    new MemoryStream(cipherBytes)) {
                    using (CryptoStream cs =
                        new CryptoStream(ms,
                        decryptor,
                        CryptoStreamMode.Read)) {
                        using (StreamReader sr =
                            new StreamReader(cs)) {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}