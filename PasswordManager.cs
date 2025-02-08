using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SecurePassManager
{
    public class PasswordManager
    {
        private readonly string storageFile = "passwords.dat";
        private Dictionary<string, Credentials> passwordData;

        public PasswordManager()
        {
            LoadPasswords();
        }

        public void SavePassword(string website, string username, string password)
        {
            string encryptedPassword = EncryptionHelper.Encrypt(password);
            passwordData[website] = new Credentials(username, encryptedPassword);
            SavePasswords();
        }

        public Credentials GetPassword(string website)
        {
            if (passwordData.ContainsKey(website))
            {
                string decryptedPassword = EncryptionHelper.Decrypt(passwordData[website].Password);
                return new Credentials(passwordData[website].Username, decryptedPassword);
            }
            return null;
        }

        private void LoadPasswords()
        {
            if (File.Exists(storageFile))
            {
                string json = File.ReadAllText(storageFile);
                passwordData = JsonSerializer.Deserialize<Dictionary<string, Credentials>>(json);
            }
            else
            {
                passwordData = new Dictionary<string, Credentials>();
            }
        }

        private void SavePasswords()
        {
            string json = JsonSerializer.Serialize(passwordData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(storageFile, json);
        }
    }

    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Credentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
