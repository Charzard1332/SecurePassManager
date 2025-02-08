using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecurePassManager
{
    public partial class MainForm : Form
    {
        private PasswordManager passwordManager;

        public MainForm()
        {
            InitializeComponent();
            passwordManager = new PasswordManager();
        }

        private void savePassBtn_Click(object sender, EventArgs e)
        {
            string website = websiteTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(website) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            passwordManager.SavePassword(website, username, password);
            MessageBox.Show("Password Saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void getPassBtn_Click(object sender, EventArgs e)
        {
            string website = websiteTextBox.Text;
            var credentials = passwordManager.GetPassword(website);

            if (credentials != null)
            {
                usernameTextBox.Text = credentials.Username;
                passwordTextBox.Text = credentials.Password;
                Clipboard.SetText(credentials.Password);
                MessageBox.Show("Password copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No saved password for this website!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void miniBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
