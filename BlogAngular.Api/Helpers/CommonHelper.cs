using System.Security.Cryptography;

namespace BlogAngular.Api.Helpers
{
    public static class CommonHelper
    {
        public static string GenerateSecureRandomNumericCode()
        {
            var bytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            // Chuyển bytes thành số nguyên dương
            var randomValue = BitConverter.ToUInt32(bytes, 0);

            // Lấy 8 chữ số đầu tiên
            return (randomValue % 100000000).ToString("D8");
        }
    }
}
