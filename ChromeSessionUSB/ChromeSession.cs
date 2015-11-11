using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            if (File.Exists("session-data"))
            {
                RestoreSessionButton.Enabled = true;
            }
            if (File.Exists(".saved-session"))
            {
                RestoreSessionButton.Text = "Restore Original Session";
            }
        }

        private void setChromePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.selectChromeDirectory(selectDirectoryDialog);
        }

        private void SaveSessionButton_Click(object sender, EventArgs e)
        {
            saveSessionWork();
        }

        private void saveSessionWork(String password = null)
        {
            List<String> cookieValue;
            CookieProcessor processor = new CookieProcessor();
            processor.buildWorkingCopy(ChromeHandler.chromeDir + "\\Cookies");
            byte[] data = processor.readCookies(".working", out cookieValue);
            File.Delete(".working");
            byte[] serialized = Utils.serialize(cookieValue);
            byte[] len = BitConverter.GetBytes(data.Length);
            byte[] built = new byte[len.Length + data.Length + serialized.Length];
            len.CopyTo(built, 0);
            data.CopyTo(built, len.Length);
            serialized.CopyTo(built, len.Length + data.Length);
            if (password == null)
            {
                DataInputBox box = new DataInputBox("Password entry", "Create password for securing session data", true);
                if (box.ShowDialog() == DialogResult.OK)
                {
                    Cryptography.updateSalt();
                    byte[] encrypted = Cryptography.Encrypt(built, box.getInputText().Trim());
                    if (encrypted == null)
                    {
                        MessageBox.Show("The inputted credentials were incorrect! Please try again.");
                        return;
                    }
                    File.WriteAllBytes("session-data", encrypted);
                    RestoreSessionButton.Enabled = true;
                    MessageBox.Show("Data saved and secured at \"" + Environment.CurrentDirectory + "\\session-data\"", "Complete!");
                }
            }
            else
            {
                Cryptography.updateSalt();
                byte[] encrypted = Cryptography.Encrypt(built, password);
                if (encrypted == null)
                {
                    MessageBox.Show("The inputted credentials were incorrect! Please try again.");
                    return;
                }
                File.WriteAllBytes("session-data", encrypted);
                RestoreSessionButton.Enabled = true;
                MessageBox.Show("Data saved and secured at \"" + Environment.CurrentDirectory + "\\session-data\"", "Complete!");
            }
        }

        private void RestoreSessionButton_Click(object sender, EventArgs e)
        {
            if ((File.Exists("session-data")) && (!File.Exists(".saved-session")))
            {
                DataInputBox box = new DataInputBox("Password entry", "Enter password used to secure this session data", true);
                if (box.ShowDialog() == DialogResult.OK)
                {
                    Cryptography.updateSalt();
                    byte[] decrypted = Cryptography.Decrypt(File.ReadAllBytes("session-data"), box.getInputText().Trim());
                    if (decrypted == null)
                    {
                        MessageBox.Show("The inputted credentials were incorrect! Please try again.");
                        return;
                    }
                    int len = BitConverter.ToInt32(decrypted, 0);
                    byte[] data = decrypted.subArray(4, len);
                    byte[] serialized = decrypted.subArray(4 + len, decrypted.Length - (len + 4));
                    List<String> cookieValue = (List<String>)Utils.deserialize(serialized);
                    CookieProcessor processor = new CookieProcessor();
                    byte[] modified = processor.updateData(data, cookieValue);
                    processor.buildWorkingCopy(ChromeHandler.chromeDir + "\\Cookies");
                    byte[] read = File.ReadAllBytes(".working");
                    File.WriteAllBytes(".saved-session", Cryptography.Encrypt(read, box.getInputText().Trim()));
                    MessageBox.Show("Data successfully adapted, please restart Chrome so that the session changes may take effect. You will be notified when the session has been successfully inputted.", "Session success!");
                    while (true)
                    {
                        try
                        {
                            File.WriteAllBytes(ChromeHandler.chromeDir + "\\Cookies", modified);
                            MessageBox.Show("Session successfully inputted into Chrome!", "Input success!");
                            break;
                        }
                        catch
                        {

                        }
                        Thread.Sleep(50);
                    }
                    RestoreSessionButton.Text = "Restore Original Session";
                }
            }
            else if (File.Exists(".saved-session"))
            {
                DataInputBox box = new DataInputBox("Password entry", "Enter password used to secure this session data", true);
                if (box.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("Would you like to save your current session back to the session data before reverting to the original session?", "Session query", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        saveSessionWork(box.getInputText().Trim());
                    }
                    Cryptography.updateSalt();
                    byte[] decrypted = Cryptography.Decrypt(File.ReadAllBytes(".saved-session"), box.getInputText().Trim());
                    if (decrypted == null)
                    {
                        MessageBox.Show("The inputted credentials were incorrect! Please try again.");
                        return;
                    }
                    File.Delete(".saved-session");
                    MessageBox.Show("Please restart Chrome so that the session changes may take effect. You will be notified when the session has been successfully inputted.", "Session success!");
                    while (true)
                    {
                        try
                        {
                            File.WriteAllBytes(ChromeHandler.chromeDir + "\\Cookies", decrypted);
                            MessageBox.Show("Session successfully inputted into Chrome!", "Input success!");
                            break;
                        }
                        catch
                        {

                        }
                        Thread.Sleep(50);
                    }
                    RestoreSessionButton.Text = "Restore Saved Session";
                    if (File.Exists("session-data"))
                    {
                        RestoreSessionButton.Enabled = true;
                    }
                    else
                    {
                        RestoreSessionButton.Enabled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Session data file not found! Please ensure you have saved a chrome session to this installation.", "Session data error");
            }
        }

        private void deleteSessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreSessionButton.Enabled = false;
            if (File.Exists("session-data"))
            {
                File.Delete("session-data");
            }
            if (File.Exists(".saved-session"))
            {
                File.Delete(".saved-session");
            }
        }
    }
}