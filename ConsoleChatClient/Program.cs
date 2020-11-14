using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

    class Client
    {
        public Socket clientSocket { get; set; }
        public IPEndPoint ipEndPoint { get; set; }
        public string serverIP { get; set; }
        public int serverPort { get; set; }

        public Client() { }
        public Client(string serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            try
            {
                this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Start()
        {
            try
            {
                this.clientSocket.Connect(this.ipEndPoint);
                new ServerThread(this.clientSocket);
            } catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Stop()
        {
            if (this.clientSocket != null)
                this.clientSocket.Close();
        }

    }

    class ServerThread
    {
        Socket serverSocket;

        public ServerThread(Socket serverSocket)
        {
            this.serverSocket = serverSocket;
            Thread send = new Thread(new ThreadStart(Send)); send.Start();
            Thread receive = new Thread(new ThreadStart(Receive)); receive.Start();
        }

        void Send()
        {
            NetworkStream networkStream = new NetworkStream(this.serverSocket);
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
            NetworkStream networkStream = new NetworkStream(this.serverSocket);
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

    class Program
    {
        public static void Main(string[] args)
        {
            new Client("127.0.0.1", 16057);
        }
    }
}