using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Dora
{
    [Serializable]
    public class PostMan
    {
        public enum PostManType { SEND_KEY, SEND_MESSAGE, DISCONNECT }

        public PostManType Type { get; set; }
        public byte[] Payload { get; set; }

        public PostMan()
        {
            this.Type = PostManType.SEND_MESSAGE;
            this.Payload = UnicodeEncoding.UTF8.GetBytes("");
        }

        public PostMan(PostManType type, byte[] payload)
        {
            this.Type = type;
            this.Payload = payload;
        }

        public static void SendPackage(NetworkStream netStream, PostMan postMan)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(netStream, postMan);
        }
        public static PostMan GetPackage(NetworkStream netStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //netStream.Seek(0, System.IO.SeekOrigin.Begin);

            PostMan postMan = (PostMan)formatter.Deserialize(netStream);
            return postMan;
        }

        public override string ToString()
        {
            return "Type: " + this.Type.ToString() + ", Payload: " + UnicodeEncoding.UTF8.GetString(this.Payload);
        }
    }
}
