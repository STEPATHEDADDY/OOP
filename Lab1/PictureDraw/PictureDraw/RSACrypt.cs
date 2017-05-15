using System;
using System.Security.Cryptography;

namespace DigitalSignature
{
    class RSACrypt
    {
        public static bool VerifySignedHash(byte[] unsignedFile, string publicKey)
        {
            try
            {
                var signatureSize = 128;

                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                
                byte[] signature = new byte[signatureSize];
                byte[] file = new byte[unsignedFile.Length - signatureSize];

                Buffer.BlockCopy(unsignedFile, 0, file, 0, unsignedFile.Length - signatureSize);
                Buffer.BlockCopy(unsignedFile, unsignedFile.Length - signatureSize, signature, 0, signatureSize);

                RSAalg.FromXmlString(publicKey);

                return RSAalg.VerifyData(file, new SHA1CryptoServiceProvider(), signature);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}

