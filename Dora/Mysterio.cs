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

        public static byte[] Encrypt(byte[] pubKeyByte, string plainText)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(ConvertBytesToKey(pubKeyByte));
            return rsa.Encrypt(Encoding.Unicode.GetBytes(plainText), false);
        }

        public static string Decrypt(string priKey, byte[] cypherBytes)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(ConvertStringToKey(priKey));
            byte[] plainTextBytes = rsa.Decrypt(cypherBytes, false);
            return Encoding.Unicode.GetString(plainTextBytes);
        }

        public static byte[] Encrypt(RSAParameters pubKey, string plainText)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(pubKey);
            byte[] cypherByte = rsa.Encrypt(Encoding.Unicode.GetBytes(plainText), false);
            string cypherBase64Str = Convert.ToBase64String(cypherByte);
            return Encoding.Unicode.GetBytes(cypherBase64Str); //cypherBase64Bytes
        }

        public static string Decrypt(RSAParameters priKey, byte[] cypherBase64Bytes)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(priKey);

            string base64CypherStr = Encoding.Unicode.GetString(cypherBase64Bytes);
            byte[] cypherBytes = Convert.FromBase64String(base64CypherStr);

            byte[] plainTextBytes = rsa.Decrypt(cypherBytes, false);
            return Encoding.Unicode.GetString(plainTextBytes);
        }

        public static RSAParameters ConvertBytesToKey(byte[] keyBytes)
        {
            return ConvertStringToKey(Encoding.Unicode.GetString(keyBytes));
        }

        public static string ConvertKeyToXmlString(RSAParameters key)
        {
            StringWriter strWriter = new StringWriter();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(strWriter, key);
            return strWriter.ToString();
        }

        public static byte[] ConvertKeyToBytes(RSAParameters key)
        {
            return Encoding.Unicode.GetBytes(ConvertKeyToXmlString(key));
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
