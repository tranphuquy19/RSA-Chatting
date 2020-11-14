using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Dora;

namespace Client
{

    class Client
    {
        public Socket ClientSocket { get; set; }
        public IPEndPoint IpEndPoint { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public Mysterio ServerMysterio { get; set; }
        public Mysterio ClientMysterio { get; set; }

        public Client() { }
        public Client(string serverIP, int serverPort)
        {
            this.ClientMysterio = new Mysterio(2048);
            this.ServerMysterio = new Mysterio();
            this.ServerIp = serverIP;
            this.ServerPort = serverPort;
            this.IpEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            try
            {
                this.ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Start();
            }
            catch (Exception e)
            {
                Stop();
                Console.WriteLine(e.Message);
            }
        }

        private void Start()
        {
            try
            {
                this.ClientSocket.Connect(this.IpEndPoint);
                Thread send = new Thread(new ThreadStart(Send)); send.Start();
                Thread receive = new Thread(new ThreadStart(Receive)); receive.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Stop()
        {
            if (this.ClientSocket != null)
                this.ClientSocket.Close();
        }


        void Send()
        {
            NetworkStream networkStream = new NetworkStream(this.ClientSocket);
            while (true)
            {
                string str = Console.ReadLine();
                PostMan postMan = new PostMan();

                switch (str.ToUpper())
                {
                    case "SEND_KEY":
                        postMan.Type = PostMan.PostManType.SEND_KEY;
                        postMan.Payload = Mysterio.ConvertKeyToBytes(this.ClientMysterio.PubKey);
                        break;
                    default:
                        postMan.Type = PostMan.PostManType.SEND_MESSAGE;
                        str = this.ClientMysterio.CreateCypherBase64String(str);
                        postMan.Payload = Encoding.Unicode.GetBytes(str);
                        break;
                }

                PostMan.SendPackage(networkStream, postMan);
                if (str.ToUpper().Equals("QUIT")) break;
            }

            networkStream.Close();
        }

        void Receive()
        {
            NetworkStream networkStream = new NetworkStream(this.ClientSocket);
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            RSAParameters serverKey = new RSAParameters();
            while (true)
            {
                PostMan postMan = PostMan.GetPackage(networkStream);
                Console.WriteLine(postMan);
                //Console.WriteLine("Inner PRI: " + Mysterio.ConvertKeyToString(this.ClientMysterio.PriKey));
                //Console.WriteLine("Inner PUB: " + Mysterio.ConvertKeyToString(this.ClientMysterio.PubKey));
                switch (postMan.Type)
                {
                    case PostMan.PostManType.SEND_KEY:
                        //this.ServerMysterio.ImportParameters(postMan.Payload);

                        StringReader sr = new StringReader(Encoding.Unicode.GetString(postMan.Payload));
                        XmlSerializer xml = new XmlSerializer(typeof(RSAParameters));
                        serverKey = (RSAParameters)xml.Deserialize(sr);
                        csp.ImportParameters(serverKey);

                        break;
                    case PostMan.PostManType.SEND_MESSAGE:
                        //string plainText = this.ServerMysterio.DecryptCypherToString(postMan.Payload);
                        //Console.WriteLine("Server: " + plainText);
                        byte[] mess = Convert.FromBase64String(Encoding.Unicode.GetString(postMan.Payload));
                        byte[] temp = csp.Decrypt(mess, false);
                        Console.WriteLine(Encoding.Unicode.GetString(temp));
                        break;
                }
                if (postMan.Type == PostMan.PostManType.DISCONNECT) break;
            }
            networkStream.Close();
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            new Client("127.0.0.1", 16057);
        }
    }
}