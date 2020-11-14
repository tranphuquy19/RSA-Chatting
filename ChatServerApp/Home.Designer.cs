namespace ChatServerApp
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbServer = new System.Windows.Forms.CheckBox();
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.lbServer = new System.Windows.Forms.Label();
            this.lbServerIp = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbServer
            // 
            this.cbServer.AutoSize = true;
            this.cbServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbServer.Location = new System.Drawing.Point(101, 17);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(113, 20);
            this.cbServer.TabIndex = 0;
            this.cbServer.Text = "Start as server";
            this.cbServer.UseVisualStyleBackColor = true;
            this.cbServer.CheckedChanged += new System.EventHandler(this.cbServer_CheckedChanged);
            // 
            // txtServerIp
            // 
            this.txtServerIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerIp.Location = new System.Drawing.Point(101, 52);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(176, 23);
            this.txtServerIp.TabIndex = 1;
            this.txtServerIp.Text = "127.0.0.1";
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbServer.Location = new System.Drawing.Point(15, 19);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(48, 16);
            this.lbServer.TabIndex = 2;
            this.lbServer.Text = "Server";
            // 
            // lbServerIp
            // 
            this.lbServerIp.AutoSize = true;
            this.lbServerIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbServerIp.Location = new System.Drawing.Point(15, 55);
            this.lbServerIp.Name = "lbServerIp";
            this.lbServerIp.Size = new System.Drawing.Size(66, 17);
            this.lbServerIp.TabIndex = 3;
            this.lbServerIp.Text = "Server IP";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(214, 18);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(59, 16);
            this.lbStatus.TabIndex = 4;
            this.lbStatus.Text = "(status)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbDebugMode);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.lbServerIp);
            this.groupBox1.Controls.Add(this.lbStatus);
            this.groupBox1.Controls.Add(this.cbServer);
            this.groupBox1.Controls.Add(this.txtServerIp);
            this.groupBox1.Controls.Add(this.lbServer);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(724, 98);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chat App Config";
            // 
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDebugMode.Location = new System.Drawing.Point(322, 17);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(106, 20);
            this.cbDebugMode.TabIndex = 7;
            this.cbDebugMode.Text = "Debug mode";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            this.cbDebugMode.CheckedChanged += new System.EventHandler(this.cbDebugMode_CheckedChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(322, 52);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(106, 23);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(13, 396);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(628, 68);
            this.txtMessage.TabIndex = 7;
            this.txtMessage.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(647, 396);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(89, 68);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtMessages.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessages.Location = new System.Drawing.Point(12, 116);
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtMessages.Size = new System.Drawing.Size(724, 265);
            this.txtMessages.TabIndex = 9;
            this.txtMessages.Text = "";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 476);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.groupBox1);
            this.Name = "Home";
            this.Text = "RSA Chat Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Home_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cbServer;
        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.Label lbServerIp;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbDebugMode;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtMessages;
    }
}

