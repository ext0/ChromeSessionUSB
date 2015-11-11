using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
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
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM cookies ORDER BY creation_utc;", cnn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[] hash = (byte[])(reader["encrypted_value"]);
                            byte[] unprotected = ProtectedData.Unprotect(hash, null, DataProtectionScope.CurrentUser);
                            String build = String.Empty;
                            for (int i = 0; i < unprotected.Length; i++)
                            {
                                build += (char)unprotected[i];
                            }
                            decrypted.Add(build);
                        }
                    }
                }
                cnn.Close();
            }
            File.Delete(workingFile);
            return bytes;
        }
        public byte[] updateData(byte[] data, List<String> cookieValues, String workingFile = ".temp")
        {
            File.WriteAllBytes(workingFile, data);
            List<Tuple<long, String>> values = new List<Tuple<long, String>>();
            using (SQLiteConnection cnn = new SQLiteConnection("data source=\"" + Directory.GetCurrentDirectory() + "\\" + workingFile + "\""))
            {
                cnn.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM cookies ORDER BY creation_utc;", cnn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            values.Add(new Tuple<long, String>((long)reader["creation_utc"], cookieValues[i]));
                            i++;
                        }
                    }
                }
                using (SQLiteTransaction transaction = cnn.BeginTransaction())
                {
                    foreach (Tuple<long,String> tuple in values)
                    {
                        using (SQLiteCommand update = new SQLiteCommand("UPDATE cookies SET encrypted_value=@encrypted WHERE creation_utc=@creation_utc;", cnn))
                        {
                            byte[] built = new byte[tuple.Item2.Length];
                            for (int j = 0; j < built.Length; j++)
                            {
                                built[j] = (byte)tuple.Item2[j];
                            }
                            update.Parameters.AddWithValue("encrypted", ProtectedData.Protect(built, null, DataProtectionScopeDataProtectionScope.CurrentUser));
                            update.Parameters.AddWithValue("creation_utc", tuple.Item1);
                            update.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                cnn.Close();
            }
            data = File.ReadAllBytes(workingFile);
            File.Delete(workingFile);
            return data;
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
