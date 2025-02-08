using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SecurePassManager
{
    public static class Utils
    {
        private static readonly string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_+=";

        public static string GenerateSecurePassword(int length = 12)
        {
            Random random = new Random();
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                password.Append(characters[random.Next(characters.Length)]);
            }
            return password.ToString();
        }

        public static void AutoClearClipboard(int delayInSeconds = 10)
        {
            Thread.Sleep(delayInSeconds * 1000);
            Clipboard.Clear();
        }

        public static void SetDarkMode(Form form)
        {
            form.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            form.ForeColor = System.Drawing.Color.White;

            foreach (Control control in form.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
                    button.ForeColor = System.Drawing.Color.White;
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
                    textBox.ForeColor = System.Drawing.Color.White;
                }
            }
        }
    }
}
