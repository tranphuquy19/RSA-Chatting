using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServerApp
{
    public static class ThreadHelper
    {
        delegate void SetLbStatusDelegate(Form f, Control ctrl, bool isOnline);
        delegate void SetTextDelegate(Form f, Control ctrl, string text);
        delegate void AppendMessagesDelegate(Form f, Control ctrl, string text, bool isOwn);
        delegate void SetEnableDelegate(Form f, Control ctrl, bool isEnable);
        delegate void AddListviewItemDelegate(Form f, Control ctrl, ListViewItem item);

        public static void SetStatus(Form f, Control ctrl, bool isOnline)
        {
            if (ctrl.InvokeRequired)
            {
                SetLbStatusDelegate d = new SetLbStatusDelegate(SetStatus);
                f.Invoke(d, new object[] { f, ctrl, isOnline });
            }
            else
            {
                if (isOnline)
                {
                    ctrl.Text = "(online)";
                    ctrl.ForeColor = Color.Green;
                }
                else
                {
                    ctrl.Text = "(offline)";
                    ctrl.ForeColor = Color.Red;
                }
            }
        }

        public static void SetText(Form f, Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                SetTextDelegate d = new SetTextDelegate(SetText);
                f.Invoke(d, new object[] { f, ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        public static void AppendMessages(Form f, Control ctrl, string text, bool isOwn = true)
        {
            if (ctrl.InvokeRequired)
            {
                AppendMessagesDelegate d = new AppendMessagesDelegate(AppendMessages);
                f.Invoke(d, new object[] { f, ctrl, text, isOwn });
            }
            else
            {
                RichTextBox _txtMessages = (RichTextBox)ctrl;
                if (isOwn)
                {
                    _txtMessages.SelectionColor = Color.Green;
                }
                else
                {
                    _txtMessages.SelectionColor = Color.Blue;
                }
                _txtMessages.SelectedText = String.IsNullOrEmpty(_txtMessages.Text) ? text : Environment.NewLine + text;
            }
        }

        public static void SetEnable(Form f, Control ctrl, bool isEnable)
        {
            if (ctrl.InvokeRequired)
            {
                SetEnableDelegate d = new SetEnableDelegate(SetEnable);
                f.Invoke(d, new object[] { f, ctrl, isEnable });
            }
            else
            {
                ctrl.Enabled = isEnable;
            }
        }

        public static void AddListViewItem(Form f, Control ctrl, ListViewItem item)
        {
            if (ctrl.InvokeRequired)
            {
                AddListviewItemDelegate d = new AddListviewItemDelegate(AddListViewItem);
                f.Invoke(d, new object[] { f, ctrl, item });
            }
            else
            {
                DebugForm debugForm = (DebugForm)f;
                debugForm.lvDebug.Items.Add(item);
            }
        }
    }
}
