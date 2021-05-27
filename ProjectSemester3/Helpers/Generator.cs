using OtpNet;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ProjectSemester.Helpers
{
    public class Generator
    {
        public string GenerateNumericString(int length)
        {
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            byte[] rgb = new byte[length];

            rngCrypt.GetBytes(rgb);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                builder.Append(rgb[i] % 10);
            }

            return builder.ToString();
        }
        public string GenerateOtp()
        {
            string base32Secret = "6L4OH6DDC4PLNQBA5422GM67KXRDIQQP";
            var secret = Base32Encoding.ToBytes(base32Secret);

            var totp = new Totp(secret);
            var code = totp.ComputeTotp();

            return code;
        }
        public string GenerateBankCode()
        {
            Random rand = new Random();
            String card = "BE";
            for (int i = 0; i < 14; i++)
            {
                int n = rand.Next(10) + 0;
                card += n.ToString();
            }
            return card;
        }
    }
}
