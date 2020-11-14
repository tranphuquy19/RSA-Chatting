using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    #region Server class
    class Server
    {
        public Socket serverSocket { get; set; }
        public IPEndPoint ipEndPoint { get; set; }
        public Socket receiveSocket { get; set; }
        public int serverPort { get; set; }

        public Server() { }

        public Server(int serverPort)
        {
            this.serverPort = serverPort;
            this.ipEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
            try
            {
                this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(this.ipEndPoint);
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
            this.serverSocket.Listen(100);
            Console.WriteLine("Started server...");
            Console.WriteLine("Waitting for client...");
            while (true)
            {
                this.receiveSocket = this.serverSocket.Accept();
                Console.WriteLine("Accepted new client...");
                new ClientThread(this.receiveSocket);
            }
        }

        private void Stop()
        {
            if (this.receiveSocket != null)
                this.receiveSocket.Close();
            if (this.serverSocket != null)
                this.serverSocket.Close();
        }
    }
    #endregion

    #region ClientThread
    class ClientThread
    {
        private Socket clientSocket;

        public ClientThread(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            Thread send = new Thread(new ThreadStart(Send)); send.Start();
            Thread receive = new Thread(new ThreadStart(Receive)); receive.Start();
        }

        void Send()
        {
            NetworkStream networkStream = new NetworkStream(this.clientSocket);
            StreamWriter streamWriter = new StreamWriter(networkStream);
            while (true)
            {
                string str = Console.ReadLine();
                streamWriter.WriteLine(str);
                streamWriter.Flush();
                if (str.ToUpper().Equals("QUIT")) break;
            }
            streamWriter.Close();
            networkStream.Close();
        }

        void Receive()
        {
            NetworkStream networkStream = new NetworkStream(clientSocket);
            StreamReader streamReader = new StreamReader(networkStream);
            while (true)
            {
                string receiveStr = streamReader.ReadLine();
                Console.WriteLine("message from client: " + receiveStr);
                if (receiveStr.ToUpper().Equals("QUIT")) break;
            }
            streamReader.Close();
            networkStream.Close();
        }
    }
    #endregion ClientThread

    class Program
    {
        public static void Main(string[] args)
        {
            new Server(16057);
        }
    }
}