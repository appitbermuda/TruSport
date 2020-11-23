using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TruSport.Data
{
    public sealed class PasswordHasher
    {
        public byte Version => 1;
        public int Pbkdf2IterCount { get; } = 50000;
        public int Pbkdf2SubkeyLength { get; } = 256 / 8; // 256 bits
        private const int SaltSize = 256;
        private const int BlockSize = 256;
        private const int KeySize = 256;
        public HashAlgorithmName HashAlgorithmName { get; } = HashAlgorithmName.SHA256;

        public string EncryptString(string Text)
        {
            if (Text == null)
                throw new ArgumentNullException(nameof(Text));

            byte[] bytes;

            var salt = GetRandomBytes(SaltSize);
            var iv = GetRandomBytes(BlockSize);
            var data = Encoding.UTF8.GetBytes(Text);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Constants.CryptoPW, salt, Pbkdf2IterCount))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(Pbkdf2SubkeyLength);
                using (var encrypt = new RijndaelManaged())
                {
                    encrypt.BlockSize = BlockSize;
                    encrypt.Mode = CipherMode.CBC;
                    encrypt.Padding = PaddingMode.PKCS7;
                    using (var encryptor = encrypt.CreateEncryptor(bytes, iv))
                    {
                        using (var stream = new MemoryStream())
                        {
                            stream.Write(salt, 0, salt.Length);
                            stream.Write(iv, 0, iv.Length);
                            using (var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(data, 0, data.Length);
                                cryptoStream.FlushFinalBlock();
                            }
                            return Convert.ToBase64String(stream.ToArray());
                        }
                    }
                }
            }

            //var inArray = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            //inArray[0] = Version;
            //Buffer.BlockCopy(salt, 0, inArray, 1, SaltSize);
            //Buffer.BlockCopy(bytes, 0, inArray, 1 + SaltSize, Pbkdf2SubkeyLength);

            //return Convert.ToBase64String(inArray);
        }

        public string DecryptString(string Text)
        {
            if (Text == null)
                throw new ArgumentNullException(nameof(Text));

            // Get data plus salt and IV

            byte[] bytes;
            byte[] allData = Convert.FromBase64String(Text);
            byte[] salt = new byte[SaltSize / 8];
            byte[] iv = new byte[BlockSize / 8];
            byte[] data = new byte[allData.Length - (salt.Length + iv.Length)];

            Buffer.BlockCopy(allData, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(allData, salt.Length, iv, 0, iv.Length);
            Buffer.BlockCopy(allData, salt.Length + iv.Length, data, 0, data.Length);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Constants.CryptoPW, salt, Pbkdf2IterCount))
            {
                bytes = rfc2898DeriveBytes.GetBytes(Pbkdf2SubkeyLength);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = BlockSize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(bytes, iv))
                    {
                        using (var memoryStream = new MemoryStream(data))
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var plainTextBytes = new byte[data.Length];
                            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }


        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (hashedPassword == null)
                return PasswordVerificationResult.Failed;

            byte[] numArray = Convert.FromBase64String(hashedPassword);
            if (numArray.Length < 1)
                return PasswordVerificationResult.Failed;

            byte version = numArray[0];
            if (version > Version)
                return PasswordVerificationResult.Failed;

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(numArray, 1, salt, 0, SaltSize);
            byte[] a = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(numArray, 1 + SaltSize, a, 0, Pbkdf2SubkeyLength);
            byte[] bytes;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, Pbkdf2IterCount))
            {
                bytes = rfc2898DeriveBytes.GetBytes(Pbkdf2SubkeyLength);



                //if (CryptographicOperations.FixedTimeEquals(a, bytes))
                //    return PasswordVerificationResult.Success;

                return PasswordVerificationResult.Failed;
            }

            //// In .NET Core 2.1, you can use CryptographicOperations.FixedTimeEquals
            //// https://github.com/dotnet/corefx/blob/a10890f4ffe0fadf090c922578ba0e606ebdd16c/src/System.Security.Cryptography.Primitives/src/System/Security/Cryptography/CryptographicOperations.cs#L32
            //[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            //public static bool FixedTimeEquals(byte[] left, byte[] right)
            //{
            //    // NoOptimization because we want this method to be exactly as non-short-circuiting as written.
            //    // NoInlining because the NoOptimization would get lost if the method got inlined.
            //    if (left.Length != right.Length)
            //    {
            //        return false;
            //    }

            //    int length = left.Length;
            //    int accum = 0;

            //    for (int i = 0; i < length; i++)
            //    {
            //        accum |= left[i] - right[i];
            //    }

            //    return accum == 0;
            //}
        }

        public enum PasswordVerificationResult
        {
            Failed,
            Success,
            SuccessRehashNeeded,
        }

        private byte[] GetRandomBytes(int bits)
        {
            byte[] bytes = new byte[bits / 8];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(bytes);
            }
            return bytes;
        }
    }
}