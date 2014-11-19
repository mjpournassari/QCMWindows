using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QCM
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
             if (textBox1.Text.ToLower().Trim() == "qtv" && textBox2.Text.ToLower().Trim() == "qtv!")
            {
                Main Frm1 = new Main();
                Frm1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(" نام کاربری و گذرواژه را چک فرمایید","LOGIN",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                pictureBox1_Click(null, null);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox1_Click(null, null);
            }
        }
    }
}
