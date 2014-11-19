using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QCM
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void ثبتاطلاعاتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QCM.Contacts Shr = new QCM.Contacts();
            Shr.MdiParent = this;
            Shr.TopMost = true;
            Shr.Show();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult Rs = MessageBox.Show("آیا از برنامه خارج می شوید", "خروج از برنامه",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Rs == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void قرعهکشیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QCM.Lottery Shr = new QCM.Lottery();
            Shr.MdiParent = this;
            Shr.TopMost = true;
            Shr.Show();
        }
    }
}
