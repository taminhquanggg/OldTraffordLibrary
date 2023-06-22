using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using log4net;
using OldTraffordLibrary.Database;
using OldTraffordLibrary.Service;
using OldTraffordLibrary.VIEW.DialogForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OldTraffordLibrary.VIEW.FunctionForm
{
    public partial class frmBook : DevExpress.XtraEditors.XtraForm
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(frmBook));
        OldTraffordLibraryEntities dbContext = new OldTraffordLibraryEntities();
        public frmBook()
        {
            InitializeComponent();
        }


        private void frmBook_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData(string keySearch = "")
        {
            try
            {
                List<tbl_Book> listData = new List<tbl_Book>();
                if (keySearch != "")
                {
                    listData = dbContext.tbl_Book.Where(b => b.BookID.Contains(keySearch) ||
                                                            b.QuickCode.Contains(keySearch) ||
                                                            b.BookTitle.Contains(keySearch) ||
                                                            b.TypeOfBook.Contains(keySearch) ||
                                                            b.Author.Contains(keySearch) ||
                                                            b.Publisher.Contains(keySearch)).ToList();
                }
                else
                {
                    listData = dbContext.tbl_Book.ToList();
                }
                BindingSource bs = new BindingSource();
                bs.DataSource = listData;
                bvData.BindingSource = bs;
                dgvData.AutoGenerateColumns = false;
                dgvData.DataSource = bs;
                dgvData.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvData.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                log.Debug(ex.ToString());
            }
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            frmAddBook frm = new frmAddBook();
            DialogResult dislogResult = frm.ShowDialog();
            if (dislogResult == DialogResult.OK)
            {
                MessageBox.Show("Thêm sách thành công !");
                LoadData();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sách ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    var delBook = dbContext.tbl_Book.Find(dgvData.CurrentRow.Cells["colBookID"].Value.ToString());
                    dbContext.tbl_Book.Remove(delBook);
                    dbContext.SaveChanges();
                    dgvData.Rows.Remove(dgvData.CurrentRow);
                    MessageBox.Show("Xóa sách thành công !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                log.Debug(ex.ToString());

            }
        }

        private void dgvData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int _Amount, _Position;
                var row = dgvData.Rows[e.RowIndex];
                var updateBook = dbContext.tbl_Book.Find(row.Cells["colBookID"].Value);
                updateBook.QuickCode = row.Cells["colQuickCode"].Value?.ToString();
                updateBook.BookTitle = row.Cells["colBookTitle"].Value?.ToString();
                updateBook.TypeOfBook = row.Cells["colTypeOfBook"].Value?.ToString();
                updateBook.Author = row.Cells["colAuthor"].Value?.ToString();
                updateBook.Publisher = row.Cells["colPublisher"].Value?.ToString();
                if (int.TryParse(row.Cells["colAmount"].Value?.ToString(), out _Amount))
                {
                    updateBook.Amount = int.Parse(row.Cells["colAmount"].Value?.ToString());
                }
                if (int.TryParse(row.Cells["colPosition"].Value?.ToString(), out _Position))
                {
                    updateBook.Position = int.Parse(row.Cells["colPosition"].Value?.ToString());
                }
                updateBook.Content = row.Cells["colContent"].Value?.ToString();
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                log.Debug(ex.ToString());

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != txtSearch.Properties.NullText)
            {
                LoadData(txtSearch.Text);
            }
            else
            {
                MessageBox.Show("Nhập từ khóa tìm kiếm để thực hiện tìm kiếm");
                txtSearch.Focus();
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != txtSearch.Properties.NullText)
            {
                txtSearch.SelectAll();
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }

    }
}