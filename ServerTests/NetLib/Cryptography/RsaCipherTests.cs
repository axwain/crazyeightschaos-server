using System.Text;

using CrazyEights.NetLib.Cryptography;

namespace CrazyEights.Tests.NetLib.Cryptography
{
    [TestFixture]
    public class RsaCipherTests
    {
        [Test, Description("Correctly encrypts and decrypts data")]
        public void RsaCipher_Encryption_Decryption()
        {
            var cipher = new RsaCipher();
            var randomSalt = new Random().Next(0, Int32.MaxValue);
            var plainText = $"My Mocked Salted Data {randomSalt}";
            var data = Encoding.UTF8.GetBytes(plainText);
            var encryptedData = cipher.Encrypt(data);
            var decryptedData = Encoding.UTF8.GetString(cipher.Decrypt(encryptedData));

            Assert.That(decryptedData, Is.EqualTo(plainText), "data has been encrypted and decrypted");
        }

        [Test, Description("Correctly decrypts data with imported keys")]
        public void RsaCipher_Export_Import_Keys()
        {
            var decrypter = new RsaCipher();
            var encrypter = new RsaCipher();
            var randomSalt = new Random().Next(0, Int32.MaxValue);
            var plainText = $"My Mocked Salted Data {randomSalt}";
            var data = Encoding.UTF8.GetBytes(plainText);

            Assert.That(
                decrypter.PublicKeyBlob,
                Is.Not.EqualTo(encrypter.PublicKeyBlob),
                "should have different public keys"
            );

            encrypter.ImportKey(decrypter.PublicKeyBlob);
            var encryptedData = encrypter.Encrypt(data);

            var decryptedData = Encoding.UTF8.GetString(decrypter.Decrypt(encryptedData));

            Assert.That(decryptedData, Is.EqualTo(plainText), "data has been decrypted with imported keys");
        }
    }
}