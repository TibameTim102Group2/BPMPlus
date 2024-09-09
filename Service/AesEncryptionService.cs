using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BPMPlus.Service
{
    public class AesEncryptionService
    {
        public string GenerateKey()
        {
            string keyBase64 = "";
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();

                keyBase64 = Convert.ToBase64String(aes.Key);
            }
            return keyBase64;
        }

        //加密
        public string Encrypt(string PlainText, string Key, out string IVKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                aes.Key = Convert.FromBase64String(Key);
                aes.GenerateIV();
                IVKey = Convert.ToBase64String(aes.IV);
                ICryptoTransform encryptor = aes.CreateEncryptor();

                byte[] encryptedData;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs)) 
                            {
                                sw.Write(PlainText);
                            }
                            encryptedData = ms.ToArray();
                    }
                }
                    return Convert.ToBase64String(encryptedData);
            }
        }

        //解密
        public string Decrypt(string CiperText, string Key, string IVKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                aes.Key = Convert.FromBase64String(Key);
                aes.IV = Convert.FromBase64String(IVKey);

                ICryptoTransform decrypt = aes.CreateDecryptor();

                string PlainText = "";
                byte[] ciper = Convert.FromBase64String(CiperText);

                using (MemoryStream ms = new MemoryStream(ciper))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs)) 
                            {
                                PlainText = sr.ReadToEnd();
                            }
                    }
                }
                return PlainText;
            }
        }

    }
}
