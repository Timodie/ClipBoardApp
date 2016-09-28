using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;


namespace ClipBoardAppBeta
{
    public partial class Form1 : Form
    {


        private System.Windows.Forms.RichTextBox richTextBox1;

        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hRemove, IntPtr hNewNext);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg,
                                      IntPtr wParam,
                                      IntPtr lParam);
        IntPtr nextClipboardViewer;
        List<string> clipItems;
        Stack board;
        Queue board1;
        string selected;
        public Form1()
        {
            InitializeComponent();
            Clipboard.Clear();
            board =new Stack();
            board1 = new Queue();
            clipItems = new List<string>();
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
            selected = "";
            // this.richTextBox1 = new System.Windows.Forms.RichTextBox();
        }
        //private void Initialize

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (board1.Count != 0)
            {
                MessageBox.Show("Dequeued :\n" + (string)board1.Dequeue());
            }
            else
            {
                MessageBox.Show("Queue is now empty");
                listView1.Clear();
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("By Tim Addai", "ClipApp");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void A2_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                MessageBox.Show("Clipboard says :\n" + (string)iData.GetData(DataFormats.Text));
                //richTextBox2.Text = (string)iData.GetData(DataFormats.Text);
            }
            else
            {
                MessageBox.Show("Sorry no text data");
            }
        }
        private void A3_Click(object sender, EventArgs e)
        {
            if (board.Count != 0)
            {
                MessageBox.Show("Stack popped:" + board.Pop().ToString());
            }
            else
            {
                MessageBox.Show("Stack empty");
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			// defined in winuser.h
			const int WM_DRAWCLIPBOARD = 0x308;
			const int WM_CHANGECBCHAIN = 0x030D;

			switch(m.Msg)
			{
				case WM_DRAWCLIPBOARD:
					DisplayClipboardData();
					SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
					break;

				case WM_CHANGECBCHAIN:
					if (m.WParam == nextClipboardViewer)
						nextClipboardViewer = m.LParam;
					else
						SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
					break;

				default:
					base.WndProc(ref m);
					break;
			}	
		}

        private void DisplayClipboardData()
        {
            try
            {
                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    string clip = (string)iData.GetData(DataFormats.Text);
                    if (!clipItems.Contains(clip))
                    {
                            clipItems.Add(clip);
                             board1.Enqueue(clip);
                             board.Push(clip);
                            ListViewItem lvi = new ListViewItem(clip);
                           // MessageBox.Show(board.Count.ToString());
                            if (clipItems.Count==1)
                            {
                                listView1.Items.Insert(0, lvi);
                            }
                             else if(listView1.Items.Count >0)
                            {
                                if (!selected.Equals(clip))
                                {
                                    listView1.Items.Insert(0, lvi);
                                }
                            }
                       
                        }
                }
                else
                {
                    MessageBox.Show("Clipboard empty");
                }
                }
            catch(Exception e){
                MessageBox.Show(e.ToString());
            }
            }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
                MessageBox.Show("minimized!");
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(500);
                this.Hide();
        }

     

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Exit ClipApp", "ClipApp Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                
                default:
                    break;
            }        
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                contextMenuStrip2.Show();
               

            }

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            contextMenuStrip2.Show();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelectedValuesToClipboard();
          
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in listView1.SelectedItems)
                builder.AppendLine(item.SubItems[0].Text);
            selected = builder.ToString();
            Clipboard.SetText(selected); 
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Tim\mytxt.txt";
            var builder = new StringBuilder();
           
            foreach (ListViewItem item in listView1.SelectedItems)
                builder.AppendLine(item.SubItems[0].Text);
            selected = builder.ToString();
            if(selected.Length > 4)
            {
                string sub = selected.Substring(0, 4);
                path = @"C:\Users\Tim\" + sub + ".txt";
            }
           
           
            File.WriteAllText(path, selected);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var builder = new StringBuilder();
                for (int i = listView1.Items.Count - 1; i >= 0; i--)
                {
                    if (listView1.Items[i].Selected)
                    {
                        builder.AppendLine(listView1.Items[i].Text);
                        selected = builder.ToString();
                        bool removed = clipItems.Remove(listView1.Items[i].Text);
                        listView1.Items[i].Remove();
                        selected = "";
                    }
                }
            }
            catch (Exception ) { /*MessageBox.Show(f.ToString());*/ }
        }






        /* GOALS:
         1.Create a data structure that can take in text,audio,pictures,exe,folder,daaam all kinds of files??
         *2.Focus on text for now
         *3.Create a stack like GUI that displays the most recently copied texts
         *4.Get notification from clipboard--DONE
         *5.Run as a service or minimized background
         *6.When clicked should fly in
         *7.Possibly evoke when pasting texts(Paste from clipApp)
         *8.If the copied item is an image, I could possibly save it somewhere and
         * load a preview to be pasted later
         *9.I could use all the Contains methods in future for audio,etc
         *10.Bug:Can't copy chinese, non utf-8
         *11.WPF splash for beginining loading,probably with progress bar
         */
    }
}