using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SocialClub.Configuration
{
    public class EncryptDecrypt
    {
        public string Encrypt(string toEncrypt, bool useHashing = true) // To Encrypt
        {
            byte[] KeyValue;
            byte[] EncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            AppSettingsReader settingreader = new AppSettingsReader();
            string Key = "InovediaPakistan";//(string)settingreader.GetValue("Inovedia311", typeof(string));

            if (useHashing)
            {
                MD5CryptoServiceProvider hashing = new MD5CryptoServiceProvider();
                KeyValue = hashing.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));
                hashing.Clear();
            }
            else
            {
                KeyValue = UTF8Encoding.UTF8.GetBytes(Key);
            }

            TripleDESCryptoServiceProvider triple = new TripleDESCryptoServiceProvider();
            triple.Key = KeyValue;
            triple.Mode = CipherMode.ECB;
            triple.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = triple.CreateEncryptor();

            byte[] ResultArray = transform.TransformFinalBlock(EncryptArray, 0, EncryptArray.Length);
            triple.Clear();

            return Convert.ToBase64String(ResultArray, 0, ResultArray.Length);
        }

    }
}