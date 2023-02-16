using System;
using System.Security.Cryptography;

namespace CrazyEights.NetLib.Cryptography
{
    public class RsaCipher
    {
        public byte[] PublicKeyBlob { get => RSA.ExportCspBlob(false); }

        private RSACryptoServiceProvider RSA { get; set; }
        public RsaCipher()
        {
            RSA = new RSACryptoServiceProvider();
        }

        public void ImportKey(byte[] keyBlob)
        {
            if (keyBlob == null || keyBlob.Length <= 0)
                throw new ArgumentNullException("key");

            RSA.ImportCspBlob(keyBlob);
        }

        public byte[] Encrypt(byte[] DataToEncrypt)
        {
            try
            {
                //OAEP padding is only available on Microsoft Windows XP or later.  
                return RSA.Encrypt(DataToEncrypt, false);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public byte[] Decrypt(byte[] DataToDecrypt)
        {
            try
            {
                //OAEP padding is only available on Microsoft Windows XP or later.  
                return RSA.Decrypt(DataToDecrypt, false);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
    }
}