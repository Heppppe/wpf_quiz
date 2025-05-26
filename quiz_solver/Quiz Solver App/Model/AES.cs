using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quiz_Solver_App.Model
{
    public static class AES
    {
        private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
        private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("ABCDEF0123456789");
        public static Quiz DecryptQuiz(string filePath)
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            string json = DecryptStringFromBytes_Aes(encryptedData, aesKey, aesIV);
            var quiz = JsonSerializer.Deserialize<Quiz>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return quiz;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(cipherText);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
