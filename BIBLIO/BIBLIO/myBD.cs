using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BIBLIO
{
    public partial class myBD : Form
    {
        public myBD()
        {
            InitializeComponent();
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About f1= new About();
            f1.ShowDialog();    
        }

        private void калькуляторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculator f2 = new Calculator();
            f2.ShowDialog();
        }

        private void myBD_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void читачіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabl1 f1 = new tabl1();
            f1.ShowDialog();
        }

        private void каталогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabl2 f2 = new tabl2(); 
            f2.ShowDialog();
        }

        private void формулярToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabl3 f3 = new tabl3(); 
            f3.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
