using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;

namespace Dora
{
    public class Mysterio
    {
        public int KeySize { get; set; }
        public RSACryptoServiceProvider CSP { get; set; }
        public RSAParameters PubKey { get; set; }
        public RSAParameters PriKey { get; set; }

        public Mysterio()
        {
            this.CSP = new RSACryptoServiceProvider();
        }

        public Mysterio(int keySize)
        {
            this.KeySize = keySize;
            this.CSP = new RSACryptoServiceProvider(this.KeySize);
            this.PriKey = this.CSP.ExportParameters(true);
            this.PubKey = this.CSP.ExportParameters(false);
        }

        public static RSAParameters ConvertBytesToKey(byte[] keyBytes)
        {
            return ConvertStringToKey(Encoding.Unicode.GetString(keyBytes));
        }

        public static string ConvertKeyToString(RSAParameters key)
        {
            StringWriter strWriter = new StringWriter();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(strWriter, key);
            return strWriter.ToString();
        }

        public static byte[] ConvertKeyToBytes(RSAParameters key)
        {
            return Encoding.Unicode.GetBytes(ConvertKeyToString(key));
        }

        public static RSAParameters ConvertStringToKey(string keyStr)
        {
            StringReader strReader = new StringReader(keyStr);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)xmlSerializer.Deserialize(strReader);
        }

        public byte[] CreateCypherBytes(string plainText)
        {
            if (this.CSP == null) throw new Exception("Invalid CryptoServiceProvider! Please ImportParameters(key);");
            byte[] bytePlainText = Encoding.Unicode.GetBytes(plainText);
            byte[] byteCypherText = this.CSP.Encrypt(bytePlainText, false);
            return byteCypherText;
        }

        public string CreateCypherBase64String(string plainText)
        {
            return Convert.ToBase64String(CreateCypherBytes(plainText));
        }

        public byte[] DecryptCypherToBytes(byte[] cypherBytes)
        {
            if (this.CSP == null) throw new Exception("Invalid CryptoServiceProvider! Please ImportParameters(key);");
            byte[] plainTextBytes = this.CSP.Decrypt(cypherBytes, false);
            return plainTextBytes;
        }

        public string DecryptCypherToString(string base64CypherStr)
        {
            byte[] cypherBytes = Convert.FromBase64String(base64CypherStr);
            return Encoding.Unicode.GetString(DecryptCypherToBytes(cypherBytes));
        }

        public string DecryptCypherToString(byte[] base64CypherBytes)
        {
            return DecryptCypherToString(Encoding.Unicode.GetString(base64CypherBytes));
        }

        public void ImportParameters(RSAParameters key)
        {
            this.CSP.ImportParameters(key);
        }

        public void ImportParameters(string keyString)
        {
            RSAParameters key = ConvertStringToKey(keyString);
            ImportParameters(key);
        }

        public void ImportParameters(byte[] keyBytes)
        {
            ImportParameters(Encoding.Unicode.GetString(keyBytes));
        }
    }
}
