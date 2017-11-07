﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//
//namespace ChatHistoryBot.Helpers
//{
//    public class CryptoHelpers
//    {
//        public static string EncryptString(string text, string password, bool compressText)
//        {
//            byte[] baPwd = Encoding.UTF8.GetBytes(password);
//
//            // Hash the password with SHA256
//            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);
//
//            byte[] baText = Encoding.UTF8.GetBytes(text);
//
//            if (compressText)
//                baText = Compressor.Compress(baText);
//
//            byte[] baSalt = GetRandomBytes();
//            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];
//
//            // Combine Salt + Text
//            for (int i = 0; i < baSalt.Length; i++)
//                baEncrypted[i] = baSalt[i];
//            for (int i = 0; i < baText.Length; i++)
//                baEncrypted[i + baSalt.Length] = baText[i];
//
//            baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);
//
//            string result = Convert.ToBase64String(baEncrypted);
//            return result;
//        }
//
//        public static string DecryptString(string text, string password, bool decompressText)
//        {
//            byte[] baPwd = Encoding.UTF8.GetBytes(password);
//
//            // Hash the password with SHA256
//            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);
//
//            byte[] baText = Convert.FromBase64String(text);
//
//            byte[] baDecrypted = AES_Decrypt(baText, baPwdHash);
//
//            // Remove salt
//            int saltLength = GetSaltLength();
//            byte[] baResult = new byte[baDecrypted.Length - saltLength];
//            for (int i = 0; i < baResult.Length; i++)
//                baResult[i] = baDecrypted[i + saltLength];
//
//            if (decompressText)
//                baResult = Compressor.Decompress(baResult);
//
//            string result = Encoding.UTF8.GetString(baResult);
//            return result;
//        }
//        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
//        {
//            byte[] encryptedBytes = null;
//
//            // Set your salt here, change it to meet your flavor:
//            // The salt bytes must be at least 8 bytes.
//            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
//
//            using (MemoryStream ms = new MemoryStream())
//            {
//                using (RijndaelManaged AES = new RijndaelManaged())
//                {
//                    AES.KeySize = 256;
//                    AES.BlockSize = 128;
//
//                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
//                    AES.Key = key.GetBytes(AES.KeySize / 8);
//                    AES.IV = key.GetBytes(AES.BlockSize / 8);
//
//                    AES.Mode = CipherMode.CBC;
//
//                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
//                    {
//                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
//                        cs.Close();
//                    }
//                    encryptedBytes = ms.ToArray();
//                }
//            }
//
//            return encryptedBytes;
//        }
//
//        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
//        {
//            byte[] decryptedBytes = null;
//
//            // Set your salt here, change it to meet your flavor:
//            // The salt bytes must be at least 8 bytes.
//            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
//
//            using (MemoryStream ms = new MemoryStream())
//            {
//                using (RijndaelManaged AES = new RijndaelManaged())
//                {
//                    AES.KeySize = 256;
//                    AES.BlockSize = 128;
//
//                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
//                    AES.Key = key.GetBytes(AES.KeySize / 8);
//                    AES.IV = key.GetBytes(AES.BlockSize / 8);
//
//                    AES.Mode = CipherMode.CBC;
//
//                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
//                    {
//                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
//                        cs.Close();
//                    }
//                    decryptedBytes = ms.ToArray();
//                }
//            }
//
//            return decryptedBytes;
//        }
//
//        public static byte[] GetRandomBytes()
//        {
//            int saltLength = GetSaltLength();
//            byte[] ba = new byte[saltLength];
//            RNGCryptoServiceProvider.Create().GetBytes(ba);
//            return ba;
//        }
//
//        public static int GetSaltLength()
//        {
//            return 8;
//        }
//    }
//    public class Compressor
//    {
//        public static byte[] Compress(byte[] data)
//        {
//            MemoryStream output = new MemoryStream();
//            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
//            {
//                dstream.Write(data, 0, data.Length);
//            }
//            return output.ToArray();
//        }
//
//        public static byte[] Decompress(byte[] data)
//        {
//            MemoryStream input = new MemoryStream(data);
//            MemoryStream output = new MemoryStream();
//            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
//            {
//                dstream.CopyTo(output);
//            }
//            return output.ToArray();
//        }
//    }
//}
