namespace ChatServerApp
{
    partial class DebugForm
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
            this.lvDebug = new System.Windows.Forms.ListView();
            this.timestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.payload = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvDebug
            // 
            this.lvDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDebug.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timestamp,
            this.type,
            this.payload});
            this.lvDebug.FullRowSelect = true;
            this.lvDebug.GridLines = true;
            this.lvDebug.HideSelection = false;
            this.lvDebug.Location = new System.Drawing.Point(13, 12);
            this.lvDebug.Name = "lvDebug";
            this.lvDebug.Size = new System.Drawing.Size(715, 499);
            this.lvDebug.TabIndex = 0;
            this.lvDebug.UseCompatibleStateImageBehavior = false;
            this.lvDebug.View = System.Windows.Forms.View.Details;
            // 
            // timestamp
            // 
            this.timestamp.Text = "Timestamp";
            this.timestamp.Width = 128;
            // 
            // type
            // 
            this.type.Text = "Type";
            this.type.Width = 128;
            // 
            // payload
            // 
            this.payload.Text = "Payload";
            this.payload.Width = 452;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 548);
            this.Controls.Add(this.lvDebug);
            this.Name = "DebugForm";
            this.Text = "Debug  Form - RSA Chat Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.ColumnHeader timestamp;
        public System.Windows.Forms.ColumnHeader type;
        public System.Windows.Forms.ColumnHeader payload;
        public System.Windows.Forms.ListView lvDebug;
    }
}