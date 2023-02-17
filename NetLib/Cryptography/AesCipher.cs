using System;
using System.IO;
using System.Security.Cryptography;

namespace CrazyEights.NetLib.Cryptography
{
    public class AesCipher
    {
        public byte[] Key
        {
            get => Cipher.Key;
            set
            {
                if (value == null || value.Length <= 0)
                    throw new ArgumentNullException("Key");
                Cipher.Key = value;
            }
        }
        public byte[] IV
        {
            get => Cipher.IV;
            set
            {
                if (value == null || value.Length <= 0)
                    throw new ArgumentNullException("IV");
                Cipher.IV = value;
            }
        }

        private Aes Cipher { get; set; }

        public AesCipher()
        {
            Cipher = Aes.Create();
            Cipher.KeySize = 128;
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");

            byte[] result;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, Cipher.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }

                result = ms.ToArray();
            }

            return result;
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");

            byte[] decryptedData;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, Cipher.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }

                decryptedData = ms.ToArray();
            }

            return decryptedData;
        }
    }
}