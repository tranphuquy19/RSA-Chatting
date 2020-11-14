using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Dora;

namespace Server
{
    #region Server class
    class Server
    {
        public Socket ServerSocket { get; set; }
        public IPEndPoint IpEndPoint { get; set; }
        public Socket ReceiveSocket { get; set; }
        public int ServerPort { get; set; }

        public Server() { }

        public Server(int serverPort)
        {
            this.ServerPort = serverPort;
            this.IpEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
            try
            {
                this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Bind(this.IpEndPoint);
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
            this.ServerSocket.Listen(100);
            Console.WriteLine("Started server...");
            Console.WriteLine("Waitting for client...");
            while (true)
            {
                this.ReceiveSocket = this.ServerSocket.Accept();
                Console.WriteLine("Accepted new client...");

                Thread send = new Thread(new ThreadStart(Send)); send.Start();
                Thread receive = new Thread(new ThreadStart(Receive)); receive.Start();
            }
        }

        private void Stop()
        {
            if (this.ReceiveSocket != null)
                this.ReceiveSocket.Close();
            if (this.ServerSocket != null)
                this.ServerSocket.Close();
        }

        void Send()
        {
            NetworkStream networkStream = new NetworkStream(this.ReceiveSocket);

            while (true)
            {
                string str = Console.ReadLine();

                PostMan postMan = new PostMan()
                {
                    Type = PostMan.PostManType.SEND_MESSAGE,
                    Payload = Encoding.Unicode.GetBytes(str)
                };

                PostMan.SendPackage(networkStream, postMan);
                if (str.ToUpper().Equals("QUIT")) break;
            }

            networkStream.Close();
        }

        void Receive()
        {
            NetworkStream networkStream = new NetworkStream(ReceiveSocket);
            while (true)
            {
                PostMan postMan = PostMan.GetPackage(networkStream);
                Console.WriteLine(postMan);
                if (postMan.Type == PostMan.PostManType.DISCONNECT) break;
            }
            networkStream.Close();
        }
    }
    #endregion

    class Program
    {
        public static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            new Server(16057);
        }
    }
}