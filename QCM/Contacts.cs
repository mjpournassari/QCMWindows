using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QCM
{
    public partial class Contacts : Form
    {
        private int _CurrentContactId = 0;

        public Contacts()
        {
            InitializeComponent();
        }

        private void Contacts_Load(object sender, EventArgs e)
        {
            LoadProvince();
            LoadStaticCmb();
            LoadPrograms();
        }

        protected void LoadStaticCmb()
        {
            string[] IrDate = DateConversion.GD2JD(DateTime.Now).Split('/');

            for (int i = 1; i < 32; i++)
            {
                MyListItem Lst = new MyListItem(string.Format("{0:00}", i), i);
               // cmbBirthDay.Items.Add(Lst);
                cmbScoreDay.Items.Add(Lst);
                if (string.Format("{0:00}", i) == IrDate[2])
                {
                 //   cmbBirthDay.SelectedIndex = i - 1;
                    cmbScoreDay.SelectedIndex = i - 1;
                }
            }

            for (int i = 1; i < 13; i++)
            {
                MyListItem Lst = new MyListItem(string.Format("{0:00}", i), i);
                //cmbBirthMonth.Items.Add(Lst);
                cmbScoreMonth.Items.Add(Lst);

                if (string.Format("{0:00}", i) == IrDate[1])
                {
                  //  cmbBirthMonth.SelectedIndex = i - 1;
                    cmbScoreMonth.SelectedIndex = i - 1;
                }
            }

            int Indx = 0;
            for (int i = 1320; i < 1400; i++)
            {
                MyListItem Lst = new MyListItem(i.ToString(), i);
               // cmbBirthYear.Items.Add(Lst);
                cmbScoreYear.Items.Add(Lst);

                if (string.Format("{0:00}", i) == IrDate[0])
                {
                    //cmbBirthYear.SelectedIndex = Indx;
                    cmbScoreYear.SelectedIndex = Indx;
                }
                Indx++;
            }

          //  cmbBirthDay.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmbBirthDay.AutoCompleteSource = AutoCompleteSource.ListItems;

            //cmbBirthMonth.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmbBirthMonth.AutoCompleteSource = AutoCompleteSource.ListItems;

            //cmbBirthYear.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmbBirthYear.AutoCompleteSource = AutoCompleteSource.ListItems;

            // cmbPrograms.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            // cmbPrograms.AutoCompleteSource = AutoCompleteSource.ListItems;

            //   cmbScore.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //   cmbScore.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbScoreDay.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbScoreDay.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbScoreMonth.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbScoreMonth.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbScoreYear.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbScoreYear.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        protected void LoadProvince()
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.PROVINCEDataTable Dt = s.Province_Select_All();

            cmbProvince.Items.Clear();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                MyListItem Itm = new MyListItem(Dt.Rows[i]["NAME"].ToString().Trim(), int.Parse(Dt.Rows[i]["ID"].ToString().Trim()));
                cmbProvince.Items.Add(Itm);
            }
            if (cmbProvince.Items.Count > 0)
            {
                cmbProvince.SelectedIndex = 0;
            }

            cmbProvince.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProvince.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        protected void LoadPrograms()
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.PROGRAMSDataTable Dt = s.Programs_SelectAll();

            cmbPrograms.Items.Clear();
            for (int i = 0; i < Dt.Count; i++)
            {
                MyListItem Itm = new MyListItem(Dt[i]["TITLE"].ToString().Trim(), int.Parse(Dt[i]["ID"].ToString().Trim()));
                cmbPrograms.Items.Add(Itm);
            }
            if (cmbPrograms.Items.Count > 0)
            {
                cmbPrograms.SelectedIndex = 0;
            }
        }

        protected void LoadProgOptions(int ProgID)
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.PROGRAMS_OPTIONSDataTable Dt = s.Program_Options_SelectByPid(ProgID);

            cmbScore.Items.Clear();
            for (int i = 0; i < Dt.Count; i++)
            {
                MyListItem Itm = new MyListItem(Dt[i]["TITLE"].ToString().Trim(), int.Parse(Dt[i]["ID"].ToString().Trim()));
                cmbScore.Items.Add(Itm);
            }
            if (cmbScore.Items.Count > 0)
            {
                cmbScore.SelectedIndex = 0;
            }
        }

        private void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.PROVINCEDataTable Dt = s.Cities_SelectByProvince((short)((MyListItem)cmbProvince.SelectedItem).value);

            cmbCity.Items.Clear();
            for (int i = 0; i < Dt.Count; i++)
            {
                MyListItem Itm = new MyListItem(Dt[i]["NAME"].ToString().Trim(), int.Parse(Dt[i]["ID"].ToString().Trim()));
                cmbCity.Items.Add(Itm);
            }
            if (cmbCity.Items.Count > 0)
            {
                cmbCity.SelectedIndex = 0;
            }
            cmbCity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCity.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void BtnSaveContact_Click(object sender, EventArgs e)
        {
            QtvWebService.MyDB.CONTACTSDataTable Dt = new QtvWebService.MyDB.CONTACTSDataTable();
            //Requierd Fileds:
            if (txtName.Text.Trim().Length > 0 && txtFamily.Text.Trim().Length > 0)
            {
                //Check NCode is Uniqe:
                QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();

                if (_CurrentContactId == 0)
                {
                    //Insert New Contact:

                    if (txtNCode.Text.Trim().Length == 10)
                    {
                        Dt = s.Contact_SelectByNCode(txtNCode.Text.Trim());
                    }
                    else
                    {
                        if (txtNCode.Text.Trim().Length > 0)
                        {
                            Dt = s.Contact_SelectbyId(long.Parse(txtNCode.Text.Trim()));
                        }
                    }

                    if (Dt.Count == 0)
                    {
                       // DateTime Dt_Birth = DateConversion.JD2GD(cmbBirthYear.SelectedItem + "/" +
                         //                  cmbBirthMonth.SelectedItem + "/" +
                           //                 cmbBirthDay.SelectedItem);
                        

                        bool Result = s.Contact_Insert(
                            txtNCode.Text.Trim(),
                            txtName.Text.Trim(),
                            txtFamily.Text.Trim(),
                            txtFatherName.Text.Trim(),
                            (short)((MyListItem)cmbProvince.SelectedItem).value,
                           (short)((MyListItem)cmbCity.SelectedItem).value,
                            txtPhone.Text.Trim(),
                            txtPhonePrefix.Text.Trim(),
                            txtCellPhone.Text.Trim(),
                            txtAddress.Text.Trim(),
                            txtBankAccount.Text.Trim(),
                            txtBankName.Text.Trim(),
                            txtBankBranchCode.Text.Trim(),
                            txtBankCardNumber.Text.Trim(),
                            txtDescriptions.Text.Trim(),
                            DateTime.Now,
                            (int)numAge.Value, txtShenasname.Text.Trim()
                            );

                        if (Result)
                        {
                            MessageBox.Show("اطلاعات مخاطب با موفقیت ثبت شد", "ثبت اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //  MessageBox.Show("اطلاعات مخاطب با موفقیت ثبت شد", "ثبت اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        TxtSearchNCode.Text = txtNCode.Text;
                        BtnSearchMain_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("کد ملی وارد شده تکراری میباشد", "ثبت اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //Update Current Contact:

                    //DateTime Dt_Birth = DateConversion.JD2GD(cmbBirthYear.SelectedItem + "/" +
                    //                     cmbBirthMonth.SelectedItem + "/" +
                    //                      cmbBirthDay.SelectedItem);

                    bool Result = s.Contact_Update(
                        txtNCode.Text.Trim(),
                        txtName.Text.Trim(),
                        txtFamily.Text.Trim(),
                        txtFatherName.Text.Trim(),
                        (short)((MyListItem)cmbProvince.SelectedItem).value,
                       (short)((MyListItem)cmbCity.SelectedItem).value,
                        txtPhone.Text.Trim(),
                        txtPhonePrefix.Text.Trim(),
                        txtCellPhone.Text.Trim(),
                        txtAddress.Text.Trim(),
                        txtBankAccount.Text.Trim(),
                        txtBankName.Text.Trim(),
                        txtBankBranchCode.Text.Trim(),
                        txtBankCardNumber.Text.Trim(),
                        txtDescriptions.Text.Trim(),
                         (int)numAge.Value,
                        txtShenasname.Text.Trim(),
                        _CurrentContactId
                        );

                    if (Result)
                    {
                        MessageBox.Show("اطلاعات مخاطب با موفقیت بروز رسانی شد", "بروز رسانی اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //  MessageBox.Show("اطلاعات مخاطب با موفقیت ثبت شد", "ثبت اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("ثبت کد ملی 10 رقمی و نام و نام خانوادگی ضروری میباشد", "ثبت اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearchMain_Click(object sender, EventArgs e)
        {
            //if (TxtSearchNCode.Text.Trim().Length == 10)
            //{
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.CONTACTSDataTable Dt = s.Contact_Select(TxtSearchNCode.Text.Trim(), TxtSearchName.Text.Trim(), TxtSearchFamily.Text.Trim());

            dgvContacts.Rows.Clear();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                dgvContacts.Rows.Add(Dt.Rows[i]["id"].ToString().Trim(),
                    Dt.Rows[i]["First_Name"].ToString().Trim(),
                    Dt.Rows[i]["Last_Name"].ToString().Trim(),
                    Dt.Rows[i]["NCOCE"].ToString().Trim());
            }

            if (Dt.Rows.Count == 1)
            {
                _CurrentContactId = int.Parse(Dt.Rows[0]["id"].ToString().Trim());
                LoadContact();
                tabControl1.SelectedIndex = 2;
            }
            else
            {
                _CurrentContactId = 0;
                if (Dt.Rows.Count > 0)
                {
                    tabControl1.SelectedIndex = 0;
                }
                if (Dt.Rows.Count == 0)
                {
                    //Check With Id:
                    if (TxtSearchNCode.Text.Trim().Length > 0)
                    {
                        QtvWebService.QTvSoapClient s2 = new QtvWebService.QTvSoapClient();
                        QtvWebService.MyDB.CONTACTSDataTable Dt2 = s2.Contact_SelectbyId(long.Parse(TxtSearchNCode.Text.Trim()));
                        if (Dt2.Rows.Count == 1)
                        {
                            _CurrentContactId = int.Parse(TxtSearchNCode.Text.Trim());
                            LoadContact();
                            tabControl1.SelectedIndex = 2;
                        }
                        else
                        {
                            DialogResult Drs = MessageBox.Show("مخاطبی با اطلاعات وارد شده وجود ندارد آیا ثبت گردد؟", "ثبت اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if (Drs == System.Windows.Forms.DialogResult.Yes)
                            {
                                ClearForm();
                                txtName.Text = TxtSearchName.Text.Trim();
                                txtNCode.Text = TxtSearchNCode.Text.Trim();
                                txtFamily.Text = TxtSearchFamily.Text.Trim();
                                tabControl1.SelectedIndex = 1;
                            }
                        }
                    }
                    else
                    {
                        DialogResult Drs = MessageBox.Show("مخاطبی با اطلاعات وارد شده وجود ندارد آیا ثبت گردد؟", "ثبت اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (Drs == System.Windows.Forms.DialogResult.Yes)
                        {
                            ClearForm();
                            txtName.Text = TxtSearchName.Text.Trim();
                            txtNCode.Text = TxtSearchNCode.Text.Trim();
                            txtFamily.Text = TxtSearchFamily.Text.Trim();
                            tabControl1.SelectedIndex = 1;
                        }
                    }
                }
            }

            //}
            //else
            //{
            //    MessageBox.Show(" کد ملی 10 رقمی ضروری میباشد", "جستجو اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        protected void ClearForm()
        {
            txtNCode.Text = "";
            txtName.Text = "";
            txtFamily.Text = "";
            txtFatherName.Text = "";
            txtAddress.Text = "";
            txtBankAccount.Text = "";
            txtBankCardNumber.Text = "";
            txtCellPhone.Text = "";
            txtPhone.Text = "";
            txtPhonePrefix.Text = "";
            txtShenasname.Text = "";
            TxtCurrentId.Text = "";
        }

        protected void LoadContact()
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.CONTACTSDataTable Dt = s.Contact_SelectbyId((long)_CurrentContactId);

            TxtSearchNCode.Enabled = false;
            TxtSearchName.Enabled = false;
            TxtSearchFamily.Enabled = false;
            TxtSearchName.Text = Dt.Rows[0]["First_Name"].ToString().Trim();
            TxtSearchFamily.Text = Dt.Rows[0]["Last_Name"].ToString().Trim();
            TxtSearchNCode.Text = Dt.Rows[0]["ncoce"].ToString().Trim();

            txtNCode.Text = Dt.Rows[0]["NCOcE"].ToString().Trim();
            txtName.Text = Dt.Rows[0]["first_name"].ToString().Trim();
            txtFamily.Text = Dt.Rows[0]["Last_name"].ToString().Trim();
            txtFatherName.Text = Dt.Rows[0]["FATHER_NAME"].ToString().Trim();
            txtPhone.Text = Dt.Rows[0]["PHONE"].ToString().Trim();
            txtPhonePrefix.Text = Dt.Rows[0]["PHONEP_REFIX"].ToString().Trim();

            txtCellPhone.Text = Dt.Rows[0]["CELLPHONE"].ToString().Trim();
            txtAddress.Text = Dt.Rows[0]["ADDRESS"].ToString().Trim();
            txtBankAccount.Text = Dt.Rows[0]["BANK_ACCOUNT"].ToString().Trim();
            txtBankName.Text = Dt.Rows[0]["BANK_NAME"].ToString().Trim();
            txtBankBranchCode.Text = Dt.Rows[0]["BANK_BRANCH_CODE"].ToString().Trim();
            txtBankCardNumber.Text = Dt.Rows[0]["BANK_CARD_NUMBER"].ToString().Trim();
            txtDescriptions.Text = Dt.Rows[0]["DESCRIPTION"].ToString().Trim();

            TxtCurrentId.Text = Dt.Rows[0]["ID"].ToString().Trim();

//            string[] BirthDate = DateConversion.GD2JD(DateTime.Parse(Dt.Rows[0]["DATETIME_BIRTH"].ToString().Trim())).Split('/');
            string sss = Dt.Rows[0]["DATETIME_BIRTH"].ToString().Trim();

            numAge.Value = decimal.Parse(Dt.Rows[0]["DATETIME_BIRTH1"].ToString().Trim());


            //for (int i = 0; i < cmbBirthDay.Items.Count; i++)
            //{
            //    if (BirthDate[2] == ((MyListItem)cmbBirthDay.Items[i]).value.ToString())
            //    {
            //        cmbBirthDay.SelectedIndex = i;
            //    }
            //}

            //for (int i = 0; i < cmbBirthMonth.Items.Count; i++)
            //{
            //    if (BirthDate[1] == ((MyListItem)cmbBirthMonth.Items[i]).value.ToString())
            //    {
            //        cmbBirthMonth.SelectedIndex = i;
            //    }
            //}

            //for (int i = 0; i < cmbBirthYear.Items.Count; i++)
            //{
            //    if (BirthDate[0] == ((MyListItem)cmbBirthYear.Items[i]).value.ToString())
            //    {
            //        cmbBirthYear.SelectedIndex = i;
            //    }
            //}



            lblDateRegister.Text = DateConversion.GD2JD(DateTime.Parse(Dt.Rows[0]["DATETIME_REGISTER"].ToString().Trim()));

            for (int i = 0; i < cmbProvince.Items.Count; i++)
            {
                if (Dt.Rows[0]["PROVINCEID"].ToString().Trim() == ((MyListItem)cmbProvince.Items[i]).value.ToString())
                {
                    cmbProvince.SelectedIndex = i;
                }
            }

            for (int i = 0; i < cmbCity.Items.Count; i++)
            {
                if (Dt.Rows[0]["CITYID"].ToString().Trim() == ((MyListItem)cmbCity.Items[i]).value.ToString())
                {
                    cmbCity.SelectedIndex = i;
                }
            }

            LoadContactOptions();
        }

        private void BtnSearchNew_Click(object sender, EventArgs e)
        {
            TxtSearchNCode.Enabled = true;
            TxtSearchName.Enabled = true;
            TxtSearchFamily.Enabled = true;
            TxtSearchName.Text = "";
            TxtSearchFamily.Text = "";
            TxtSearchNCode.Text = "";
            _CurrentContactId = 0;
            ClearForm();
            tabControl1.SelectedIndex = 1;
        }

        private void cmbPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProgOptions((int)((MyListItem)cmbPrograms.SelectedItem).value);
        }

        private void btnInsertScore_Click(object sender, EventArgs e)
        {
            if (_CurrentContactId == 0)
            {
                MessageBox.Show("ابتدا یک مخاطب را انتخاب نمایید", "ثبت امتیاز", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


           
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.CONTACTSDataTable Dt = s.Select_ContactByRowCode(txtRowCode.Text.Trim());
            if (Dt.Rows.Count > 0)
            {
                MessageBox.Show(" این کد قبلا ثبت شده است لیست آن را مشاهده کنید", "ثبت امتیاز", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControl1.SelectedIndex = 0;
                dgvContacts.Rows.Clear();
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    dgvContacts.Rows.Add(Dt.Rows[i]["id"].ToString().Trim(),
                        Dt.Rows[i]["First_Name"].ToString().Trim(),
                        Dt.Rows[i]["Last_Name"].ToString().Trim(),
                        Dt.Rows[i]["NCOCE"].ToString().Trim());
                }

            }
            else
            {
                DateTime Dt_Vote = DateConversion.JD2GD(cmbScoreYear.SelectedItem + "/" +
                                            cmbScoreMonth.SelectedItem + "/" +
                                             cmbScoreDay.SelectedItem);

                QtvWebService.QTvSoapClient s2 = new QtvWebService.QTvSoapClient();
                bool Result = s2.Contact_Option_Insert(
                  _CurrentContactId,
                  (int)((MyListItem)cmbPrograms.SelectedItem).value,
                  (int)((MyListItem)cmbScore.SelectedItem).value,
                  Dt_Vote, "",
                  txtRowCode.Text.Trim());

                LoadContactOptions();
            }
        }

        protected void LoadContactOptions()
        {
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.Contacts_OptionsDataTable Dt = s.Contact_Options_Select(_CurrentContactId);
            dgvScores.Rows.Clear();

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string Dtm_Insert = DateConversion.GD2JD(DateTime.Parse(Dt.Rows[i]["Datetime_Insert"].ToString().Trim())) + "  " + DateTime.Parse(Dt.Rows[i]["Datetime_Insert"].ToString().Trim()).Hour + ":" + DateTime.Parse(Dt.Rows[i]["Datetime_Insert"].ToString().Trim()).Minute + ":" + DateTime.Parse(Dt.Rows[i]["Datetime_Insert"].ToString().Trim()).Second;
                string Dtm_Vote = DateConversion.GD2JD(DateTime.Parse(Dt.Rows[i]["Datetime_vote"].ToString().Trim()));
                dgvScores.Rows.Add(Dt.Rows[i]["ID"].ToString().Trim(),
                    Dt.Rows[i]["RowCode"].ToString().Trim(),
                    Dt.Rows[i]["PTITLE"].ToString().Trim(),
                    Dt.Rows[i]["OTITLE"].ToString().Trim(),
                   Dtm_Vote,
                    Dtm_Insert);
            }
            lblScoreCount.Text = Dt.Rows.Count.ToString();
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count == 1)
            {
                _CurrentContactId = int.Parse(dgvContacts.SelectedRows[0].Cells[0].Value.ToString());
                LoadContact();
                tabControl1.SelectedIndex = 2;
            }
        }

        private void btnDeleteScore_Click(object sender, EventArgs e)
        {
            if (dgvScores.SelectedRows.Count == 1)
            {
                DialogResult Drs = MessageBox.Show(" آیا امتیاز مورد نظر حذف شود؟ ", "حذف امتیاز", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (Drs == System.Windows.Forms.DialogResult.Yes)
                {
                    QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
                    bool Result = s.Contact_Options_DeletebyId(long.Parse(dgvScores.SelectedRows[0].Cells[0].Value.ToString()));

                    if (Result)
                    {
                        MessageBox.Show("امتیاز مورد نظر حذف شد", "حذف امتیاز", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //  MessageBox.Show("اطلاعات مخاطب با موفقیت ثبت شد", "ثبت اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    LoadContactOptions();
                }
            }
        }

        private void TxtSearchNCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchMain_Click(null, null);
            }
        }

        private void TxtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchMain_Click(null, null);
            }
        }

        private void TxtSearchFamily_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchMain_Click(null, null);
            }
        }

        private void BtnDeleteContact_Click(object sender, EventArgs e)
        {
            if (_CurrentContactId != 0)
            {
                DialogResult Drs = MessageBox.Show(" آیا مخاطب مورد نظر حذف شود؟ ", "حذف مخاطب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Drs == System.Windows.Forms.DialogResult.Yes)
                {
                    QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
                    s.Contact_DeletebyId(_CurrentContactId);
                    TxtSearchNCode.Enabled = true;
                    TxtSearchName.Enabled = true;
                    TxtSearchFamily.Enabled = true;
                    TxtSearchName.Text = "";
                    TxtSearchFamily.Text = "";
                    TxtSearchNCode.Text = "";
                    _CurrentContactId = 0;
                    ClearForm();
                    tabControl1.SelectedIndex = 0;
                }
            }
        }

        private void BtnSearchCode_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            QtvWebService.QTvSoapClient s = new QtvWebService.QTvSoapClient();
            QtvWebService.MyDB.CONTACTSDataTable Dt = s.Select_ContactByRowCode(txtCodeSearch.Text.Trim());
            dgvContacts.Rows.Clear();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                dgvContacts.Rows.Add(Dt.Rows[i]["id"].ToString().Trim(),
                    Dt.Rows[i]["First_Name"].ToString().Trim(),
                    Dt.Rows[i]["Last_Name"].ToString().Trim(),
                    Dt.Rows[i]["NCOCE"].ToString().Trim());
            }
        }
    }
}