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
    public partial class frmAddBook : DevExpress.XtraEditors.XtraForm
    {
        OldTraffordLibraryEntities dbContext = new OldTraffordLibraryEntities();
        public frmAddBook()
        {
            InitializeComponent();
        }

        private void frmAddBook_Load(object sender, EventArgs e)
        {
            txtBookID.Text = GenAutoBookID();
        }

        string GenAutoBookID()
        {
            string result = "";
            int maxBookID = dbContext.tbl_Book.Max(p => p.AutoID);
            if (maxBookID + 1 < 10)
            {
                result = "S000" + (maxBookID + 1).ToString();
            }
            else if (maxBookID + 1 < 100)
            {
                result = "S00" + (maxBookID + 1).ToString();
            }
            else if (maxBookID + 1 < 1000)
            {
                result = "S0" + (maxBookID + 1).ToString();
            }
            else if (maxBookID + 1 < 10000)
            {
                result = "S" + (maxBookID + 1).ToString();
            }
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validate())
                {
                    tbl_Book insertBook = new tbl_Book
                    {
                        BookID = txtBookID.Text,
                        QuickCode = txtQuickCode.Text != txtQuickCode.Properties.NullText ? txtQuickCode.Text : null,
                        BookTitle = txtBookTitle.Text != txtBookTitle.Properties.NullText ? txtBookTitle.Text : null,
                        TypeOfBook = cbTypeOfBook.Text != cbTypeOfBook.Properties.NullText ? cbTypeOfBook.Text : null,
                        Author = cbAuthor.Text != cbAuthor.Properties.NullText ? cbAuthor.Text : null,
                        Publisher = cbPublisher.Text != cbPublisher.Properties.NullText ? cbPublisher.Text : null,
                        Amount = int.Parse(nudAmount.Value.ToString()),
                        Position = int.Parse(nudPosition.Value.ToString()),
                        Content = txtContent.Text
                    };
                    dbContext.tbl_Book.Add(insertBook);
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
            if (txtBookTitle.Text == txtBookTitle.Properties.NullText)
            {
                MessageBox.Show("Tên sách không được để trống !");
                txtBookTitle.Focus();
                return false;
            }
            if (cbTypeOfBook.Text == cbTypeOfBook.Properties.NullText)
            {
                MessageBox.Show("Bạn chưa chọn thể loại sách !");
                cbTypeOfBook.Focus();
                return false;
            }
            if (cbAuthor.Text == cbAuthor.Properties.NullText)
            {
                MessageBox.Show("Bạn chưa chọn tác giả !");
                cbAuthor.Focus();
                return false;
            }
            if (cbPublisher.Text == cbPublisher.Properties.NullText)
            {
                MessageBox.Show("Bạn chưa chọn nhà xuất bản !");
                cbPublisher.Focus();
                return false;
            }
            if (nudAmount.Value == 0)
            {
                MessageBox.Show("Bạn chưa nhập số lượng sách !");
                nudAmount.Focus();
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