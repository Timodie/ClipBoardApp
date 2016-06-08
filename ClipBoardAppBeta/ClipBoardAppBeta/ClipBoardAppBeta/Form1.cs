using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ClipBoardAppBeta
{
    public partial class Form1 : Form
    {
        //IDataObject clip;
        Clipboard clip1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        public Form1()
        {
            InitializeComponent();
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
            if(iData.GetDataPresent(DataFormats.Text)){
                MessageBox.Show("Clipboard says :\n"+(String)iData.GetData(DataFormats.Text));
                richTextBox2.Text =  (string)iData.GetData(DataFormats.Text);
            }
            else{
                MessageBox.Show("Sorry no text data");
            }
        }
    }
/* GOALS:
 1.Create a data structure that can take in text,audio,pictures,exe,folder,daaam all kinds of files??
 *2.Focus on text for now
 *3.Create a stack like GUI that displays the most recently copied texts
 *4.Get notification from clipboard
 *5.Run as a service or minimized background
 *6.When clicked should fly in
 *7.Possibly evoke when pasting texts
 */
}
