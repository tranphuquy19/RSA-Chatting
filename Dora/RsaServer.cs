using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Dora
{
    public class RsaServer
    {
        public Socket ServerSocket { get; set; }
        public IPEndPoint IpEndPoint { get; set; }
        public Socket ReceiveSocket { get; set; }
        public int ServerPort { get; set; }
        public Mysterio ServerMysterio { get; set; }
        public Mysterio ClientMysterio { get; set; }

        public RsaServer() { }

        public RsaServer(int serverPort)
        {
            this.ServerMysterio = new Mysterio(2048);
            this.ClientMysterio = new Mysterio();
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
                Postman postMan = new Postman();

                switch (str.ToUpper())
                {
                    case "SEND_KEY":
                        postMan.Type = Postman.PostmanType.SEND_KEY;
                        postMan.Payload = Mysterio.ConvertKeyToBytes(this.ServerMysterio.PubKey);
                        break;
                    default:
                        postMan.Type = Postman.PostmanType.SEND_MESSAGE;
                        postMan.Payload = Mysterio.Encrypt(this.ClientMysterio.PubKey, str);
                        break;
                }

                Postman.SendPackage(networkStream, postMan);
                if (str.ToUpper().Equals("QUIT")) break;
            }

            networkStream.Close();
        }

        void Receive()
        {
            NetworkStream networkStream = new NetworkStream(ReceiveSocket);
            while (true)
            {
                Postman postMan = Postman.GetPackage(networkStream);
                Console.WriteLine(postMan);
                switch (postMan.Type)
                {
                    case Postman.PostmanType.SEND_KEY:
                        this.ClientMysterio.PubKey = Mysterio.ConvertBytesToKey(postMan.Payload);
                        break;
                    case Postman.PostmanType.SEND_MESSAGE:
                        string plainText = Mysterio.Decrypt(this.ServerMysterio.PriKey, postMan.Payload);
                        Console.WriteLine(plainText);
                        break;
                }
                if (postMan.Type == Postman.PostmanType.DISCONNECT) break;
            }
            networkStream.Close();
        }
    }
}
