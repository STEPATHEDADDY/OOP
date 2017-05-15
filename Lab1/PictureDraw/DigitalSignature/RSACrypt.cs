using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DigitalSignature
{
    public class RSACrypt
    {
        public static string filePath { get; set; }
        public static string newPath { get; set; }
        public static byte[] file { get; set; }
        private static byte[] signedCopy { get; set; }
        private static int signatureSize = 128;

        public static void getParams(string mainPath)
        {
            filePath = mainPath;
            file = File.ReadAllBytes(filePath);
            newPath = filePath.Replace(".dll", string.Empty);
            newPath += "_signed.dll";            
        }
        
        public static bool HashAndSignBytes(string exportKey)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.FromXmlString(exportKey);                
                signedCopy = RSAalg.SignData(file, new SHA1CryptoServiceProvider());
                byte[] resultFile = new byte[file.Length + signatureSize];
                Buffer.BlockCopy(file, 0, resultFile, 0, file.Length);
                Buffer.BlockCopy(signedCopy, 0, resultFile, file.Length, signatureSize);
                File.WriteAllBytes(newPath, resultFile);
                return true;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool VerifySignedHash(byte[] unsignedFile, string publicKey)
        {
            try
            {
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

