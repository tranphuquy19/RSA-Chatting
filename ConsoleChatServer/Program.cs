﻿using System;
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

namespace Server
{
    #region Server class
    class Server
    {
        public Socket ServerSocket { get; set; }
        public IPEndPoint IpEndPoint { get; set; }
        public Socket ReceiveSocket { get; set; }
        public int ServerPort { get; set; }
        public Mysterio ServerMysterio { get; set; }
        public Mysterio ClientMysterio { get; set; }

        public Server() { }

        public Server(int serverPort)
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
                PostMan postMan = new PostMan();

                switch (str.ToUpper())
                {
                    case "SEND_KEY":
                        postMan.Type = PostMan.PostManType.SEND_KEY;
                        postMan.Payload = Mysterio.ConvertKeyToBytes(this.ServerMysterio.PubKey);
                        break;
                    default:
                        postMan.Type = PostMan.PostManType.SEND_MESSAGE;
                        str = this.ServerMysterio.CreateCypherBase64String(str);
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
            NetworkStream networkStream = new NetworkStream(ReceiveSocket);
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            RSAParameters clientKey = new RSAParameters();
            while (true)
            {
                PostMan postMan = PostMan.GetPackage(networkStream);
                Console.WriteLine(postMan);
                //Console.WriteLine("Inner PRI: " + Mysterio.ConvertKeyToString(this.ServerMysterio.PriKey));
                //Console.WriteLine("Inner PUB: " + Mysterio.ConvertKeyToString(this.ServerMysterio.PubKey));
                switch (postMan.Type)
                {
                    case PostMan.PostManType.SEND_KEY:
                        //this.ClientMysterio.ImportParameters(postMan.Payload);
                        StringReader sr = new StringReader(Encoding.Unicode.GetString(postMan.Payload));
                        XmlSerializer xml = new XmlSerializer(typeof(RSAParameters));
                        clientKey = (RSAParameters)xml.Deserialize(sr);
                        csp.ImportParameters(clientKey);

                        break;
                    case PostMan.PostManType.SEND_MESSAGE:
                        //string plainText = this.ClientMysterio.DecryptCypherToString(postMan.Payload);
                        //Console.WriteLine("Client: " + plainText);
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