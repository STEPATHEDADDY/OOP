using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DigitalSignature
{
    public class Keys
    {
        public string privateKey;
        public string publicKey;
        public Keys()
        {
            privateKey = File.ReadAllText(@"G:\private.xml");
            publicKey = File.ReadAllText(@"G:\public.xml");            
        }
    }
}
