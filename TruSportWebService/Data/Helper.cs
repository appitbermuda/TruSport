using System;
using System.Security.Cryptography;

namespace OnTrackWebService.Data
{
    public class Helper
    {
        public static string ComputeHash(string Key)
        {
            SHA1CryptoServiceProvider objSHA1 = new SHA1CryptoServiceProvider();
            objSHA1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Key.ToCharArray()));
            byte[] buffer = objSHA1.Hash;
            string HashValue = System.Convert.ToBase64String(buffer);
            return HashValue;
        }
    }
}
