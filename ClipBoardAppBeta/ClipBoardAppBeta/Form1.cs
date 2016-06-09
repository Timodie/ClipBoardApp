using System;
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
        int index;
        Stack board;
        public Form1()
        {
            InitializeComponent();
            Clipboard.Clear();
            board =new Stack();
            index = 0;
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
            // this.richTextBox1 = new System.Windows.Forms.RichTextBox();
        }
        //private void Initialize

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("you clicked on A1");
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
                MessageBox.Show("Clipboard says :\n" + (String)iData.GetData(DataFormats.Text));
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
                   // MessageBox.Show("Clipboard says :\n" + (String)iData.GetData(DataFormats.Text));
                    board.Push((String)iData.GetData(DataFormats.Text));
                    ListViewItem lvi = new ListViewItem((String)iData.GetData(DataFormats.Text));
                    listView1.Items.Insert(index,lvi);
                   // index++;
                }
             
                else{
                    MessageBox.Show("Clipboard empty");
                    }
                }
            catch(Exception e){
                MessageBox.Show(e.ToString());
            }
            }

       

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

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
         */
    }
}