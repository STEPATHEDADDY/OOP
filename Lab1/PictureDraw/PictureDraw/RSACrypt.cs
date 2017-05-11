using System;
using System.Security.Cryptography;

namespace DigitalSignature
{
    class RSACrypt
    {
        public static bool VerifySignedHash(byte[] unsignedFile)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                var bytesPublicSize = 148;
                var signatureSize = 128;

                byte[] publicKey = new byte[bytesPublicSize];
                byte[] signature = new byte[signatureSize];
                byte[] file = new byte[unsignedFile.Length - (bytesPublicSize + signatureSize)];

                Buffer.BlockCopy(unsignedFile, 0, file, 0, unsignedFile.Length - (bytesPublicSize + signatureSize));
                Buffer.BlockCopy(unsignedFile, unsignedFile.Length - (bytesPublicSize + signatureSize), signature, 0, signatureSize);
                Buffer.BlockCopy(unsignedFile, unsignedFile.Length - bytesPublicSize, publicKey, 0, bytesPublicSize);

                RSAalg.ImportCspBlob(publicKey);

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

