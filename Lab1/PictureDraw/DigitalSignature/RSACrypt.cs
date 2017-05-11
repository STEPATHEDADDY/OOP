using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature
{
    class RSACrypt
    {
        public string filePath { get; set; }
        public string newPath { get; set; }
        public byte[] file { get; set; }
        private byte[] signedCopy { get; set; }
        private static int  bytesPublicSize = 148;
        private static int signatureSize = 128;

        public RSACrypt(string filePath, byte[] file)
        {
            this.filePath = filePath;
            this.file = file;
            newPath = getNewPath(filePath);
        }

        private string getNewPath(string filePath)
        {
            var result = filePath.Replace(".dll", string.Empty);
            result += "_signed.dll";
            return result;
        }

        public bool HashAndSignBytes(RSAParameters exportKey, RSAParameters publicKey, byte[] bytesPublic)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportParameters(exportKey);                        
                signedCopy = RSAalg.SignData(file, new SHA1CryptoServiceProvider());
                byte[] resultFile = new byte[file.Length + signatureSize + bytesPublicSize];
                Buffer.BlockCopy(file, 0, resultFile, 0, file.Length);
                Buffer.BlockCopy(signedCopy, 0, resultFile, file.Length, signatureSize);
                Buffer.BlockCopy(bytesPublic, 0, resultFile, file.Length + signatureSize, bytesPublicSize);                
                File.WriteAllBytes(newPath, resultFile);
                return true;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool VerifySignedHash(byte[] unsignedFile)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                bytesPublicSize = 148;
                signatureSize = 128;

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

