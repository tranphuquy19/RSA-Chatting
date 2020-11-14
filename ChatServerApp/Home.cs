using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dora;

namespace ChatServerApp
{
    public partial class Home : Form
    {
        public Socket ServerSocket { get; set; }
        public IPEndPoint IpEndPoint { get; set; }
        public Socket ReceiveSocket { get; set; }
        public int ServerPort { get; set; }
        public Mysterio MyMysterio { get; set; }
        public Mysterio OtherMysterio { get; set; }
        public bool IsServer { get; set; }
        public Thread MainThread { get; set; }
        public Thread SendThread { get; set; }
        public Thread ReceiveThread { get; set; }

        private DebugForm debugForm = new DebugForm();

        public Home()
        {
            InitializeComponent();
            this.debugForm.Visible = false;
            this.debugForm.FormClosing += (o, e) =>
            {
                this.cbDebugMode.Checked = false;
            };

            this.ServerPort = 16057;
            this.IsServer = true;
            this.cbServer.Checked = this.IsServer;
            SetStatus(false);
            this.MyMysterio = new Mysterio(2048);
            this.OtherMysterio = new Mysterio();
        }

        private void AddEvent(string type, string payload)
        {
            ListViewItem newItem = new ListViewItem(DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"));
            newItem.SubItems.Add(type);
            newItem.SubItems.Add(payload);
            ThreadHelper.AddListViewItem(this.debugForm, this.debugForm.lvDebug, newItem);
        }

        private void SetStatus(bool isOnline)
        {
            ThreadHelper.SetStatus(this, lbStatus, isOnline);
            if (isOnline)
            {
                ThreadHelper.SetText(this, btnConnect, "Disconnect");
            }
            else
            {
                ThreadHelper.SetText(this, btnConnect, "Connect");
            }
            RenderForm();
        }

        private void RenderForm()
        {
            if (this.IsServer)
            {
                ThreadHelper.SetEnable(this, lbServerIp, false);
                ThreadHelper.SetEnable(this, txtServerIp, false);
            }
            else
            {
                ThreadHelper.SetEnable(this, lbServerIp, true);
                ThreadHelper.SetEnable(this, txtServerIp, true);
            }
            if (this.ReceiveSocket == null || this.ReceiveSocket.Connected == false)
            {
                ThreadHelper.SetEnable(this, cbServer, true);
                ThreadHelper.SetEnable(this, lbServer, true);
                ThreadHelper.SetEnable(this, txtMessage, false);
                ThreadHelper.SetEnable(this, txtMessages, false);
                ThreadHelper.SetEnable(this, btnSend, false);
            }
            else
            {
                ThreadHelper.SetEnable(this, cbServer, false);
                ThreadHelper.SetEnable(this, lbServer, false);
                ThreadHelper.SetEnable(this, txtMessage, true);
                ThreadHelper.SetEnable(this, txtMessages, true);
                ThreadHelper.SetEnable(this, btnSend, true);
            }
        }

        private void Send(string mess)
        {
            NetworkStream netStream = new NetworkStream(this.ReceiveSocket);
            Postman postman = new Postman()
            {
                Type = Postman.PostmanType.SEND_MESSAGE,
                Payload = Mysterio.Encrypt(this.OtherMysterio.PubKey, mess)
            };
            Postman.SendPackage(netStream, postman);
            AddEvent(postman.Type.ToString(), Encoding.Unicode.GetString(postman.Payload));
        }
        private void Send()
        {
            NetworkStream netStream = new NetworkStream(this.ReceiveSocket);
            Postman postman = new Postman()
            {
                Type = Postman.PostmanType.SEND_KEY,
                Payload = Mysterio.ConvertKeyToBytes(this.MyMysterio.PubKey)
            };
            Postman.SendPackage(netStream, postman);
            AddEvent(postman.Type.ToString(), Encoding.Unicode.GetString(postman.Payload));
        }

        private void Receive()
        {
            try
            {
                NetworkStream netStream = new NetworkStream(this.ReceiveSocket);
                while (true)
                {
                    Postman postman = Postman.GetPackage(netStream);
                    RenderForm();
                    switch (postman.Type)
                    {
                        case Postman.PostmanType.SEND_KEY:
                            this.OtherMysterio.PubKey = Mysterio.ConvertBytesToKey(postman.Payload);
                            break;
                        case Postman.PostmanType.SEND_MESSAGE:
                            string plainText = Mysterio.Decrypt(this.MyMysterio.PriKey, postman.Payload);
                            ThreadHelper.AppendMessages(this, txtMessages, DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt\t") + plainText, false);
                            break;
                    }
                    AddEvent(postman.Type.ToString(), Encoding.Unicode.GetString(postman.Payload));
                }
            }catch(Exception e)
            {
                //MessageBox.Show(e.Message);
                Stop();
            }
        }

        private void StartServer()
        {
            this.IpEndPoint = new IPEndPoint(IPAddress.Any, this.ServerPort);
            try
            {
                this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.ServerSocket.Bind(this.IpEndPoint);
                this.ServerSocket.Listen(100);
                SetStatus(true);
                while (true)
                {
                    this.ReceiveSocket = this.ServerSocket.Accept();
                    // TODO update message here!
                    this.SendThread = new Thread(new ThreadStart(Send)); this.SendThread.Start();
                    this.ReceiveThread = new Thread(new ThreadStart(Receive)); this.ReceiveThread.Start();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                Stop();
            }

        }

        private void Stop()
        {
            SetStatus(false);
            if (this.ReceiveSocket != null)
            {
                this.ReceiveSocket.Close();
                this.ReceiveSocket = null;
            }
                
            if (this.ServerSocket != null)
            {
                this.ServerSocket.Close();
                this.ServerSocket = null;
            }
                
            if (this.SendThread != null && this.SendThread.IsAlive) this.SendThread.Abort();
            if (this.ReceiveThread != null && this.ReceiveThread.IsAlive) this.ReceiveThread.Abort();
            if (this.MainThread != null && this.MainThread.IsAlive) this.MainThread.Abort();
        }

        private void StartClient()
        {
            this.IpEndPoint = new IPEndPoint(IPAddress.Parse(this.txtServerIp.Text.Trim()), this.ServerPort);
            try
            {
                this.ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.ReceiveSocket.Connect(this.IpEndPoint);
                this.SendThread = new Thread(new ThreadStart(Send)); this.SendThread.Start();
                this.ReceiveThread = new Thread(new ThreadStart(Receive)); this.ReceiveThread.Start();
                SetStatus(true);
            }
            catch (Exception e)
            {
                Stop();
                MessageBox.Show(e.Message);
            }
        }

        private void cbServer_CheckedChanged(object sender, EventArgs e)
        {
            if (cbServer.CheckState == CheckState.Checked) this.IsServer = true;
            else this.IsServer = false;
            RenderForm();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.btnConnect.Text == "Connect")
            {
                if (this.IsServer) this.MainThread = new Thread(new ThreadStart(StartServer));
                else this.MainThread = new Thread(new ThreadStart(StartClient));
                this.MainThread.Start();
            }
            else
            {
                Stop();
                if(this.MainThread.IsAlive) this.MainThread.Abort();
                this.MainThread = null;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtMessage.Text))
            {
                Send(this.txtMessage.Text.Trim());
                ThreadHelper.AppendMessages(this, txtMessages, DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt\t") + txtMessage.Text);
                ThreadHelper.SetText(this, txtMessage, "");
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        private void cbDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDebugMode.CheckState == CheckState.Checked)
            {
                this.debugForm.Visible = true;
            }
            else this.debugForm.Visible = false;
        }
    }
}
