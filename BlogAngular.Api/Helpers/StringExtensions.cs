using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace BlogAngular.Api.Helpers
{
    public static class StringExtensions
    {
        public static string GenerateUrlHandle(string input, string prefix)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Chuyển thành dạng chữ thường
            input = input.ToLowerInvariant();

            // Loại bỏ dấu tiếng Việt
            input = RemoveDiacritics(input);

            // Thay khoảng trắng và các ký tự đặc biệt bằng dấu "-"
            input = Regex.Replace(input, @"\s+", "-"); // Thay khoảng trắng bằng "-"
            input = Regex.Replace(input, @"[^a-z0-9-]", ""); // Loại bỏ ký tự không hợp lệ
            var random = Guid.NewGuid().ToString().Substring(0, 5);
            // Trả về kết quả kèm tiền tố
            return $"{prefix}-{random}-{input}";
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

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

        public static string GetOtpEmailTemplate(string otp)
        {
            var emailBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                        }}
                        .container {{
                            width: 80%;
                            margin: 0 auto;
                            padding: 20px;
                            border: 1px solid #dddddd;
                            border-radius: 10px;
                            background-color: #f9f9f9;
                        }}
                        .header {{
                            text-align: center;
                            font-size: 24px;
                            font-weight: bold;
                            color: #4CAF50;
                        }}
                        .content {{
                            margin-top: 20px;
                            font-size: 16px;
                        }}
                        .otp {{
                            font-size: 20px;
                            font-weight: bold;
                            color: #ff5722;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 14px;
                            color: #777777;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>Your One-Time Password (OTP)</div>
                        <div class='content'>
                            Use the following OTP to complete your action. This OTP is valid for 5 minutes:
                            <div class='otp'>{otp}</div>
                            <br/>
                            If you did not request this, please ignore this email.
                        </div>
                        <div class='footer'>
                            Thank you,<br/>
                            BlogAngular Team
                        </div>
                    </div>
                </body>
                </html>
                ";
            return emailBody;
        }
    }
}
