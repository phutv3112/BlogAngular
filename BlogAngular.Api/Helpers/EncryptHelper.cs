using System.Security.Cryptography;
using System.Text;

public static class EncryptHelper
{
    public static string EncryptData(string data, IConfiguration configuration)
    {
        var key = configuration["EncryptionKey"];
        var encryptionKey = Convert.FromBase64String(key);
        using (var aes = Aes.Create())
        {
            aes.Key = encryptionKey;
            aes.GenerateIV();
            var iv = aes.IV;
            using (var encryptor = aes.CreateEncryptor())
            {
                var dataBytes = Encoding.UTF8.GetBytes(data);
                var encryptedData = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

                // Trả về IV + dữ liệu đã mã hóa
                return Convert.ToBase64String(iv) + ":" + Convert.ToBase64String(encryptedData);
            }
        }
    }

    public static string DecryptData(string encryptedData, IConfiguration configuration)
    {
        var key = configuration["EncryptionKey"];
        var encryptionKey = Convert.FromBase64String(key);
        var parts = encryptedData.Split(':');
        if (parts.Length != 2) throw new Exception("Data không hợp lệ.");

        var iv = Convert.FromBase64String(parts[0]);
        var cipherText = Convert.FromBase64String(parts[1]);

        using (var aes = Aes.Create())
        {
            aes.Key = encryptionKey;
            aes.IV = iv;
            using (var decryptor = aes.CreateDecryptor())
            {
                var decryptedData = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
    }

    public static (string PublicKey, string PrivateKey) GenerateRsaKeyPair(int keySize = 2048)
    {
        using (var rsa = new RSACryptoServiceProvider(keySize))
        {
            var publicKey = rsa.ToXmlString(false);
            var privateKey = rsa.ToXmlString(true);
            return (publicKey, privateKey);
        }
    }
    
}
