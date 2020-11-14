using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Dora
{
    [Serializable]
    public class Postman
    {
        public enum PostmanType { SEND_KEY, SEND_MESSAGE, DISCONNECT }

        public PostmanType Type { get; set; }
        public byte[] Payload { get; set; }

        public Postman()
        {
            this.Type = PostmanType.SEND_MESSAGE;
            this.Payload = Encoding.Unicode.GetBytes("");
        }

        public Postman(PostmanType type, byte[] payload)
        {
            this.Type = type;
            this.Payload = payload;
        }

        public static void SendPackage(NetworkStream netStream, Postman postMan)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(netStream, postMan);
        }
        public static Postman GetPackage(NetworkStream netStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            Postman postMan = (Postman)formatter.Deserialize(netStream);
            return postMan;
        }

        public override string ToString()
        {
            return "Type: " + this.Type.ToString() + ", Payload: " + Encoding.Unicode.GetString(this.Payload);
        }
    }
}
