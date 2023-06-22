using DevExpress.XtraEditors;
using OldTraffordLibrary.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OldTraffordLibrary.VIEW.DialogForm
{
    public partial class frmAddReader : DevExpress.XtraEditors.XtraForm
    {
        OldTraffordLibraryEntities dbContext = new OldTraffordLibraryEntities();

        public frmAddReader()
        {
            InitializeComponent();
        }

        private void frmAddReader_Load(object sender, EventArgs e)
        {
            txtReaderID.Text = GenAutoReaderID();
        }

        string GenAutoReaderID()
        {
            string result = "";
            int maxReaderID = dbContext.tbl_Reader.Max(p => p.AutoID);
            if (maxReaderID + 1 < 10)
            {
                result = "R000" + (maxReaderID + 1).ToString();
            }
            else if (maxReaderID + 1 < 100)
            {
                result = "R00" + (maxReaderID + 1).ToString();
            }
            else if (maxReaderID + 1 < 1000)
            {
                result = "R0" + (maxReaderID + 1).ToString();
            }
            else if (maxReaderID + 1 < 10000)
            {
                result = "R" + (maxReaderID + 1).ToString();
            }
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validate())
                {
                    tbl_Reader insertReader = new tbl_Reader
                    {
                        ReaderID = txtReaderID.Text,
                        ReaderName = txtReaderName.Text != txtReaderName.Properties.NullText ? txtReaderName.Text : null,
                        DateOfBirth = DateTime.ParseExact(dtDateOfBirth.Text, "dd/MM/yyyy", null),
                        Sex = cbSex.Text != cbSex.Properties.NullText ? cbSex.Text : null,
                        PhoneNumber = txtPhoneNum.Text != txtPhoneNum.Properties.NullText ? txtPhoneNum.Text : null,
                        Address = txtAddress.Text != txtAddress.Properties.NullText ? txtAddress.Text : null,
                        RegistrationDate = DateTime.ParseExact(dtRegistrationDate.Text, "dd/MM/yyyy", null),
                        ExpirationDate = DateTime.ParseExact(dtExpirationDate.Text, "dd/MM/yyyy", null),
                    };
                    dbContext.tbl_Reader.Add(insertReader);
                    dbContext.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                this.Close();
            }
        }

        bool Validate()
        {
            if (txtReaderName.Text == txtReaderName.Properties.NullText)
            {
                MessageBox.Show("Tên độc giả không được để trống !");
                txtReaderName.Focus();
                return false;
            }
            if (dtDateOfBirth.Text == dtDateOfBirth.Properties.NullText)
            {
                MessageBox.Show("Bạn chưa nhập ngày sinh !");
                dtDateOfBirth.Focus();
                return false;
            }
            if (cbSex.Text == cbSex.Properties.NullText)
            {
                MessageBox.Show("Bạn chưa nhập giới tính !");
                cbSex.Focus();
                return false;
            }
            if (dtRegistrationDate.Text == dtRegistrationDate.Properties.NullText)
            {
                MessageBox.Show("Bạn chưa nhập ngày đăng ký !");
                dtRegistrationDate.Focus();
                return false;
            }
            if (dtExpirationDate.Text == dtExpirationDate.Properties.NullText)
            {
                MessageBox.Show("Bạn chưa nhập ngày hết hạn !");
                dtExpirationDate.Focus();
                return false;
            }
            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}