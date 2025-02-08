using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurePassManager
{
    public static class EncryptionHelper
    {
        private static readonly string encryptionKey = "6\\jEm&ScBq%d;h(^spq+XU6%`Km6ufH0"; // 32 characters

        public static string Encrypt(string plainText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            keyBytes = SHA256.Create().ComputeHash(keyBytes); // Ensure 32-byte key

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = keyBytes;
            aesAlg.GenerateIV();
            using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);
            swEncrypt.Write(plainText);
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] fullCipher = Convert.FromBase64String(encryptedText);
            byte[] iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            keyBytes = SHA256.Create().ComputeHash(keyBytes); // Ensure 32-byte key

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = keyBytes;
            aesAlg.IV = iv;
            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
