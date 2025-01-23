using System.Security.Cryptography;
using System.Text;

namespace Factoring.WebMvc.Helpers
{
    public class AesEncryption
    {

        // Método para encriptar
        public static string Encrypt(string plainText, string key, string fixedIV)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(fixedIV))
                throw new ArgumentNullException(nameof(fixedIV));

            // Convertir la clave y el IV a bytes
            var keyBytes = new byte[32];
            Encoding.UTF8.GetBytes(key.PadRight(keyBytes.Length)).CopyTo(keyBytes, 0);
            var ivBytes = Encoding.UTF8.GetBytes(fixedIV.PadRight(16).Substring(0, 16)); // IV debe ser de 16 bytes

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (var writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(plainText);
                }
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText, string key, string fixedIV)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(fixedIV))
                throw new ArgumentNullException(nameof(fixedIV));

            var keyBytes = new byte[32];
            Encoding.UTF8.GetBytes(key.PadRight(keyBytes.Length)).CopyTo(keyBytes, 0);
            var ivBytes = Encoding.UTF8.GetBytes(fixedIV.PadRight(16).Substring(0, 16));

            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(fullCipher);
            using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
