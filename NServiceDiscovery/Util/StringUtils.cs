using System.Security.Cryptography;
using System.Text;

namespace NServiceDiscovery.Util
{
    public static class StringUtils
    {
        public static string GetMd5Hash(this string str)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            var md5Hash = sb.ToString();

            return md5Hash;
        }
    }
}
