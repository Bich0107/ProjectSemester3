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
    }
}
