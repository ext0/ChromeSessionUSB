using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeSessionUSB
{
    public partial class ChromeSession : Form
    {
        public ChromeSession()
        {
            InitializeComponent();
            String path = Utils.findChromeDirectory(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)));
            if (path != null)
            {
                ChromeHandler.chromeDir = path;
            }
            else
            {
                if ((MessageBox.Show("A valid Chrome installation could not be found. Would you like to manually select your Chrome directory?", "Discovery error", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    Utils.selectChromeDirectory(selectDirectoryDialog);
                }
            }
        }

        private void setChromePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.selectChromeDirectory(selectDirectoryDialog);
        }

        private void SaveSessionButton_Click(object sender, EventArgs e)
        {
            List<String> cookieValue;
            CookieProcessor processor = new CookieProcessor();
            processor.buildWorkingCopy(ChromeHandler.chromeDir + "\\Cookies");
            byte[] data = processor.readCookies(".working", out cookieValue);
            byte[] serialized = Utils.serialize(cookieValue);
            byte[] len = BitConverter.GetBytes(data.Length);
            byte[] built = new byte[len.Length + data.Length + serialized.Length];
            len.CopyTo(built, 0);
            data.CopyTo(built, len.Length);
            serialized.CopyTo(built, len.Length + data.Length);
            DataInputBox box = new DataInputBox("Password entry", "Create password for securing session data", true);
            if (box.ShowDialog() == DialogResult.OK)
            {
                Cryptography.updateSalt();
                byte[] encrypted = Cryptography.Encrypt(built, box.getInputText().Trim());
                File.WriteAllBytes("session-data", encrypted);
                MessageBox.Show("Data saved and secured at \"" + Environment.CurrentDirectory + "\\session-data\"", "Complete!");
            }
        }

        private void RestoreSessionButton_Click(object sender, EventArgs e)
        {
            if (File.Exists("session-data"))
            {
                DataInputBox box = new DataInputBox("Password entry", "Enter password used to secure this session", true);
                if (box.ShowDialog() == DialogResult.OK)
                {
                    Cryptography.updateSalt();
                    byte[] decrypted = Cryptography.Decrypt(File.ReadAllBytes("session-data"), box.getInputText().Trim());
                    int len = BitConverter.ToInt32(decrypted, 0);
                    byte[] data = decrypted.subArray(4, len);
                    byte[] serialized = decrypted.subArray(4 + len, decrypted.Length - (len + 4));
                    List<String> cookieValue = (List<String>)Utils.deserialize(serialized);
                    foreach (String s in cookieValue)
                    {
                        Debug.WriteLine(s);
                    }
                }
            }
            else
            {
                MessageBox.Show("Session data file not found! Please ensure you have saved a chrome session to this installation.", "Session data error");
            }
        }
    }
}
