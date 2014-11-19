using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QCM
{
    public partial class Lottery : Form
    {
        public Lottery()
        {
            InitializeComponent();
        }
        private void Lottery_Load(object sender, EventArgs e)
        {
            LoadPrograms();
            LoadResultlist();

        }

        protected void LoadResultlist()
        {
            //dataGridView1.Rows.Clear();
            gridControl1.DataSource = null;
            
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.Lottery_ResultDataTable Dt = s.SelectLoLst((int)((MyListItem)cmbPrograms.SelectedItem).value);

            comboBox2.Items.Clear();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                DateTime Dtm=  DateTime.Parse(Dt.Rows[i]["Datetime"].ToString().Trim());




                MyListItem Itm = new MyListItem(DateConversion.GD2JD(Dtm) + " - " + string.Format("{0:00}", Dtm.Hour) + ":" + string.Format("{0:00}", Dtm.Minute), int.Parse(Dt.Rows[i]["ID"].ToString().Trim()));
                
                comboBox2.Items.Add(Itm);
            }
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
        }

        protected void LoadPrograms()
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.PROGRAMSDataTable Dt = s.Programs_SelectAll();

            cmbPrograms.Items.Clear();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                MyListItem Itm = new MyListItem(Dt.Rows[i]["TITLE"].ToString().Trim(), int.Parse(Dt.Rows[i]["ID"].ToString().Trim()));
                cmbPrograms.Items.Add(Itm);
            }
            if (cmbPrograms.Items.Count > 0)
            {
                cmbPrograms.SelectedIndex = 0;
            }
        }
        private void cmbPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProgOptions((int)((MyListItem)cmbPrograms.SelectedItem).value);
        }
        protected void LoadProgOptions(int ProgID)
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.PROGRAMS_OPTIONSDataTable Dt = s.Program_Options_SelectByPid(ProgID);

            cmbScore.Items.Clear();


          //  MyListItem Itm0 = new MyListItem("همه امتیازات", 0);
           // cmbScore.Items.Add(Itm0);



            for (int i = 0; i < Dt.Count; i++)
            {
                MyListItem Itm = new MyListItem("فقط - "+Dt[i]["TITLE"].ToString().Trim(), int.Parse(Dt[i]["ID"].ToString().Trim()));
                cmbScore.Items.Add(Itm);
            }
            if (cmbScore.Items.Count > 0)
            {
                cmbScore.SelectedIndex = 0;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.LotteryResult[] Lst = s.DoLettery((int)((MyListItem)cmbPrograms.SelectedItem).value, int.Parse(textBox1.Text.Trim()), (int)((MyListItem)cmbScore.SelectedItem).value);
            //QtvWebService.LotteryResult[] Lst = s.DoLettery(1,100,1);

            //foreach (QtvWebService.LotteryResult item in Lst)
            //{
            //    dataGridView1.Rows.Add(item.UserName, item.UserNcode, item.UserCity, item.UserPhone, item.UserMobile);
            //}

            MessageBox.Show("قرعه کشی با موفقیت انجام شد","قرعه کشی",MessageBoxButtons.OK,MessageBoxIcon.Information);
            LoadResultlist();


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.LotteryResult[] Lst = s.SelectLottery((int)((MyListItem)comboBox2.SelectedItem).value);

            gridControl1.DefaultView.ClearDocument();
           // dataGridView1.Rows.Clear();
          //  foreach (QtvWebService.LotteryResult item in Lst)
          //  {
               // dataGridView1.Rows.Add(item.UserName, item.UserNcode, item.UserCity, item.UserPhone, item.UserMobile);
           // }
            gridControl1.DataSource = Lst;
           // gridControl1.ExportToCsv()
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
           DialogResult Dr=  MessageBox.Show("آیا نتیجه انتخاب شده حذف گردد؟","تایید حذف",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

           if (Dr == System.Windows.Forms.DialogResult.Yes)
           {
               QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
               s.DeleteLo((int)((MyListItem)comboBox2.SelectedItem).value);
               LoadResultlist();
           }

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridControl1.ExportToXlsx(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ((MyListItem)comboBox2.SelectedItem).Name.Replace("/","-").Replace(":","-")+ ".xlsx");
            gridControl1.ExportToXls(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ((MyListItem)comboBox2.SelectedItem).Name.Replace("/", "-").Replace(":", "-") + ".xls");
            alertControl1.Show(this, "فایل اکسل در محل اجرای برنامه ذخیره شد", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + ((MyListItem)comboBox2.SelectedItem).Name.Replace("/", "-").Replace(":", "-") + ".xlsx");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            LotteryPrint Pr = new LotteryPrint((int)((MyListItem)comboBox2.SelectedItem).value);
            //Pr.MdiParent = (fo)this.Parent;
            Pr.Show();
        }


    }
}
