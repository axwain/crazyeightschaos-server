using System.Text;

using CrazyEights.NetLib.Cryptography;

namespace CrazyEights.Tests.NetLib.Cryptography
{
    [TestFixture]
    public class AesCipherTests
    {
        [Test, Description("Correctly encrypts and decrypts data")]
        public void AesCipher_Encryption_Decryption()
        {
            var cipher = new AesCipher();
            var randomSalt = new Random().Next(0, Int32.MaxValue);
            var plainText = $"My Mocked Salted Data {randomSalt}";
            var data = Encoding.UTF8.GetBytes(plainText);
            var encryptedData = cipher.Encrypt(data);
            var decryptedData = Encoding.UTF8.GetString(cipher.Decrypt(encryptedData));

            Assert.That(decryptedData, Is.EqualTo(plainText), "data has been encrypted and decrypted");
        }

        [Test, Description("Correctly decrypts data with imported keys")]
        public void AesCipher_Export_Import_Keys()
        {
            var encrypter = new AesCipher();
            var decrypter = new AesCipher();
            var randomSalt = new Random().Next(0, Int32.MaxValue);
            var plainText = $"My Mocked Salted Data {randomSalt}";
            var data = Encoding.UTF8.GetBytes(plainText);

            var encryptedData = encrypter.Encrypt(data);

            Assert.That(encrypter.Key, Is.Not.EqualTo(decrypter.Key), "ciphers have different keys");
            Assert.That(encrypter.IV, Is.Not.EqualTo(decrypter.IV), "ciphers have different ivs");

            decrypter.Key = encrypter.Key;
            decrypter.IV = encrypter.IV;

            var decryptedData = Encoding.UTF8.GetString(decrypter.Decrypt(encryptedData));

            Assert.That(decryptedData, Is.EqualTo(plainText), "data has been decrypted with imported keys");
        }
    }
}