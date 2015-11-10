using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ChromeSessionUSB
{
    public static class Cryptography
    {
        private static Random random = new Random();
        private static int _iterations = 2;
        private static int _keySize = 256;
        private static int _ivSize = 16;
        private static string _hash = "SHA1";
        private static string _salt = "aselrias38490a32"; // Random

        public static byte[] Encrypt(byte[] bytes, string password)
        {
            return Encrypt<AesManaged>(bytes, password);
        }

        public static byte[] Encrypt<T>(byte[] valueBytes, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = new byte[_ivSize];
            random.NextBytes(vectorBytes);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] encrypted;
            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);
                cipher.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream to = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }

                cipher.Clear();
            }
            byte[] ret = new byte[encrypted.Length + vectorBytes.Length];
            vectorBytes.CopyTo(ret, 0);
            encrypted.CopyTo(ret, 16);
            return ret;
        }

        public static byte[] Decrypt(byte[] bytes, string password)
        {
            return Decrypt<AesManaged>(bytes, password);
        }

        public static byte[] Decrypt<T>(byte[] valueBytes, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = new byte[_ivSize];
            Buffer.BlockCopy(valueBytes, 0, vectorBytes, 0, _ivSize);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] decrypted;
            int decryptedByteCount = 0;
            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);
                cipher.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream from = new MemoryStream(valueBytes))
                    {
                        using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                        {
                            decrypted = new byte[valueBytes.Length];
                            decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                        }
                    }
                }

                cipher.Clear();
            }

            return decrypted;
        }
    }
}