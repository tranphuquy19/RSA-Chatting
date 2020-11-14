using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace PostMan
{
    [Serializable]
    public class PostMan
    {
        public int type { get; set; }
        public byte[] payload { get; set; }

        public static void SendPackage(NetworkStream netStream, PostMan postMan)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(netStream, postMan);
        }
        public static PostMan GetPackage(NetworkStream netStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            netStream.Seek(0, System.IO.SeekOrigin.Begin);

            PostMan postMan = (PostMan)formatter.Deserialize(netStream);
            return postMan;
        }
    }
}
