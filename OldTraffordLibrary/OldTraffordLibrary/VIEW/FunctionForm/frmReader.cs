using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using OldTraffordLibrary.Database;
using OldTraffordLibrary.Report;
using OldTraffordLibrary.VIEW.DialogForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OldTraffordLibrary.VIEW.FunctionForm
{
    public partial class frmReader : DevExpress.XtraEditors.XtraForm
    {
        OldTraffordLibraryEntities dbContext = new OldTraffordLibraryEntities();

        public frmReader()
        {
            InitializeComponent();
        }

        private void frmReader_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData(string keySearch = "")
        {
            try
            {
                List<tbl_Reader> listData = new List<tbl_Reader>();
                if (keySearch != "")
                {
                    listData = dbContext.tbl_Reader.Where(r => r.ReaderID.Contains(keySearch) ||
                                                            r.ReaderName.Contains(keySearch) ||
                                                            r.Sex.Contains(keySearch) ||
                                                            r.Address.Contains(keySearch)).ToList();
                }
                else
                {
                    listData = dbContext.tbl_Reader.ToList();
                }
                BindingSource bs = new BindingSource();
                bs.DataSource = listData;
                bvData.BindingSource = bs;
                dgvData.AutoGenerateColumns = false;
                dgvData.DataSource = bs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
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

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            frmAddReader frm = new frmAddReader();
            DialogResult dislogResult = frm.ShowDialog();
            if (dislogResult == DialogResult.OK)
            {
                MessageBox.Show("Thêm độc giả thành công !");
                LoadData();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa độc giả ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    var delReader = dbContext.tbl_Reader.Find(dgvData.CurrentRow.Cells["colReaderID"].Value.ToString());
                    dbContext.tbl_Reader.Remove(delReader);
                    dbContext.SaveChanges();
                    dgvData.Rows.Remove(dgvData.CurrentRow);
                    MessageBox.Show("Xóa độc giả thành công !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return;
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

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != txtSearch.Properties.NullText)
            {
                txtSearch.SelectAll();
            }
        }

        private void dgvData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var row = dgvData.Rows[e.RowIndex];
                var updateReader = dbContext.tbl_Reader.Find(row.Cells["colReaderID"].Value);
                updateReader.ReaderName = row.Cells["colReaderName"].Value?.ToString();
                updateReader.DateOfBirth = Convert.ToDateTime(row.Cells["colDateOfBirth"].Value?.ToString());
                updateReader.Sex = row.Cells["colSex"].Value?.ToString();
                updateReader.PhoneNumber = row.Cells["colPhoneNumber"].Value?.ToString();
                updateReader.Address = row.Cells["colAddress"].Value?.ToString();
                updateReader.RegistrationDate = Convert.ToDateTime(row.Cells["colRegistrationDate"].Value?.ToString());
                updateReader.ExpirationDate = Convert.ToDateTime(row.Cells["colExpirationDate"].Value?.ToString());
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return;
            }
        }

        private void btnPrintCard_Click(object sender, EventArgs e)
        {
            try
            {
                var row = dgvData.CurrentRow;
                var dataPrint = dbContext.tbl_Reader.Find(row.Cells["colReaderID"].Value.ToString());
                var report = new rptReaderCard();
                report.DataSource = dataPrint;
                report.ShowPreviewDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);

            }
        }
    }
}