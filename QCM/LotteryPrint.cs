using Mustache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace QCM
{
    public partial class LotteryPrint : Form
    {
        int lotteryId = 0;
        public LotteryPrint(int  _lotteryId)
        {
            InitializeComponent();
            lotteryId = _lotteryId;
        }

        private void LotteryPrint_Load(object sender, EventArgs e)
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.LotteryResult[] Lst = s.SelectLottery(lotteryId);

            string Result = "";
            WebRequest request = WebRequest.Create("http://77.36.163.186/print.html");
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            Result = reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            response.Close();
            //List<LotteryResultClass> RList = new List<LotteryResultClass>();
            //for (int i = 0; i < Lst.Length; i++)
            //{
            //    LotteryResultClass Rt = new LotteryResultClass();
            //    Rt.bankAccount=Lst[i].UserNcode
            //}

//            List<Person> Plist = new List<Person>();

//            Plist.Add(P);

//            string dd = @"{{#each this}}
//                        Hello, {{PersonID}}!!  {{#if Name}} djhfgdjg {{Name}}  {{/if}}
//                        {{/each}}";

            FormatCompiler compiler = new FormatCompiler();
            Generator generator = compiler.Compile(Result);
            string Html = generator.Render(Lst);

            webBrowser1.DocumentText = Html;

//            Response.Write(result);


        }

        private void toolStripBtnPrint_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }
    }
}
