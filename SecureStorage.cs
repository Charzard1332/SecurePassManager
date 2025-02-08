using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurePassManager
{
    public static class SecureStorage
    {
        private static readonly string storageFile = "securedata.dat";
        private static readonly string encryptionKey = "MY_SECRET_KEY_32CHARS_LONG";  // Change this for security!

        public static void SaveEncryptedData(string data)
        {
            try
            {
                string encryptedData = Encrypt(data);
                File.WriteAllText(storageFile, encryptedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving encrypted data: {ex.Message}");
            }
        }

        public static string LoadEncryptedData()
        {
            if (!File.Exists(storageFile))
                return null;

            try
            {
                string encryptedData = File.ReadAllText(storageFile);
                return Decrypt(encryptedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading encrypted data: {ex.Message}");
                return null;
            }
        }

        private static string Encrypt(string plainText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
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

        private static string Decrypt(string encryptedText)
        {
            byte[] fullCipher = Convert.FromBase64String(encryptedText);
            byte[] iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
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
