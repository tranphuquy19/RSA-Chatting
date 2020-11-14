using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServerApp
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        public void AddEvent(string type, string payload)
        {
            ListViewItem newItem = new ListViewItem(DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"));
            newItem.SubItems.Add(type);
            newItem.SubItems.Add(payload);
            lvDebug.Items.Add(newItem);
        }

        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.Parent = null;
        }
    }
}
