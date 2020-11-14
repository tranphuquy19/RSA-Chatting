using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
            while (true)
            {

                PostMan postMan = PostMan.GetPackage(networkStream);
                Console.WriteLine(postMan);
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