using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RsaConsoleImplement
{
    class Program
    {
        static void Main(string[] args)
        {
            var csp = new RSACryptoServiceProvider(2048);

            var privKey = csp.ExportParameters(true);

            var pubKey = csp.ExportParameters(false);

            string pubKeyString;
            {
                var sw = new System.IO.StringWriter();
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, pubKey);
                pubKeyString = sw.ToString();
            }
            Console.WriteLine(pubKeyString);
            {
                var sr = new System.IO.StringReader(pubKeyString);
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }

            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);

            var plainTextData = "foobar";

            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            var cypherText = Convert.ToBase64String(bytesCypherText);


            // tranmission

            bytesCypherText = Convert.FromBase64String(cypherText);

            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);

            bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

            Console.WriteLine(plainTextData);

            Console.ReadKey();
        }
    }
}
