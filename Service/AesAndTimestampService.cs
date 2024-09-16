using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BPMPlus.Service
{
    public class AesAndTimestampService
    {
        ////時間戳記
        // 將 DateTime 轉換為 Unix 時間戳
        public static long ToUnixTimestamp(DateTime dateTime)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - unixEpoch).TotalSeconds;
        }


        //取得key(存在appsettings內)
        private readonly IConfiguration _configuration;
        public AesAndTimestampService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetKey()
        {
            return _configuration["Secrets:Key"];
        }

        /*AES加解密*/
        //AES Getkey
        //public string GenerateKey()
        //{
        //    string keyBase64 = "";
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.KeySize = 256;
        //        aes.GenerateKey();

        //        keyBase64 = Convert.ToBase64String(aes.Key);
        //    }
        //    return keyBase64;
        //}

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

        //public byte[] ConvertToSafeBase64(string input)
        //{
        //    string base64 = input.Replace('-', '+').Replace('_', '/');
        //    switch (base64.Length % 4)
        //    {
        //        case 2: base64 += "=="; break;
        //        case 3: base64 += "="; break;
        //    }
        //    return Convert.FromBase64String(base64);
        //}

    }
}
