﻿using DevExpress.XtraEditors;
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
    public partial class frmAddLoanVoucher : DevExpress.XtraEditors.XtraForm
    {
        OldTraffordLibraryEntities dbContext = new OldTraffordLibraryEntities();
        //1: Insert, 2: Update
        public int action;
        public tbl_LoanVoucher voucherUpdate;

        public frmAddLoanVoucher()
        {
            InitializeComponent();
        }

        private void frmAddLoanVoucher_Load(object sender, EventArgs e)
        {
            if (action == 1)
            {
                txtVoucherID.Text = GenAutoLoanVoucherID();
            }
            else if (action == 2)
            {
                txtVoucherID.Text = voucherUpdate.VoucherID;
                txtReaderID.Text = voucherUpdate.ReaderID;
                dtBorrowDate.DateTime = (DateTime)voucherUpdate.BorrowDate;
                dtAppointmentDate.DateTime = (DateTime)voucherUpdate.AppointmentDate;

                LoadDetail();
            }
        }

        void LoadDetail()
        {
            var loanVoucherDetail = (from L in dbContext.tbl_LoanVoucherDetail
                                     join B in dbContext.tbl_Book on L.BookID equals B.BookID
                                     where L.VoucherID == txtVoucherID.Text
                                     select new
                                     {
                                         BookID = B.BookID,
                                         BookTitle = B.BookTitle,
                                         NumOfLoan = L.NumOfLoan
                                     }).ToList();
            BindingSource bs = new BindingSource();
            bs.DataSource = loanVoucherDetail;
            dgvBook.AutoGenerateColumns = false;
            dgvBook.DataSource = bs;
        }

        string GenAutoLoanVoucherID()
        {
            string result = "";
            int maxVoucherID = dbContext.tbl_LoanVoucher.Max(p => p.AutoID);
            if (maxVoucherID + 1 < 10)
            {
                result = "M000" + (maxVoucherID + 1).ToString();
            }
            else if (maxVoucherID + 1 < 100)
            {
                result = "M00" + (maxVoucherID + 1).ToString();
            }
            else if (maxVoucherID + 1 < 1000)
            {
                result = "M0" + (maxVoucherID + 1).ToString();
            }
            else if (maxVoucherID + 1 < 10000)
            {
                result = "M" + (maxVoucherID + 1).ToString();
            }
            return result;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchReader_Click(object sender, EventArgs e)
        {
            frmSearchReader frm = new frmSearchReader();
            frm.ShowDialog();
        }

        private void btnSearchUser_Click(object sender, EventArgs e)
        {
            frmSearchUser frm = new frmSearchUser();
            frm.ShowDialog();
        }

        private void btnSearchBook_Click(object sender, EventArgs e)
        {
            frmSearchBook frm = new frmSearchBook();
            frm.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (action == 1)
                {
                    dbContext.tbl_LoanVoucher.Add(new tbl_LoanVoucher
                    {
                        VoucherID = txtVoucherID.Text,
                        ReaderID = txtReaderID.Text,
                        BorrowDate = dtBorrowDate.DateTime,
                        AppointmentDate = dtAppointmentDate.DateTime,
                    });
                    dbContext.SaveChanges();

                    if (dgvBook.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgvBook.Rows.Count; i++)
                        {
                            dbContext.tbl_LoanVoucherDetail.Add(new tbl_LoanVoucherDetail
                            {
                                VoucherID = txtVoucherID.Text,
                                BookID = dgvBook.Rows[i].Cells["colBookID"].Value.ToString(),
                                NumOfLoan = int.Parse(dgvBook.Rows[i].Cells["colNumOfLoan"].Value.ToString()),
                            });
                            dbContext.SaveChanges();
                        }
                    }
                }
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (action == 1)
                {
                    var searchBook = dbContext.tbl_Book.Find(txtBookID.Text);
                    dgvBook.Rows.Add(new object[]
                    {
                        txtBookID.Text,
                        searchBook.BookTitle,
                        nudSL.Value
                    });
                }
                else if (action == 2)
                {
                    dbContext.tbl_LoanVoucherDetail.Add(new tbl_LoanVoucherDetail
                    {
                        VoucherID = txtVoucherID.Text,
                        BookID = txtBookID.Text,
                        NumOfLoan = (int?)nudSL.Value
                    });
                    dbContext.SaveChanges();
                    LoadDetail();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnDelDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBook.CurrentRow.Index > -1)
                {
                    if (action == 2)
                    {
                        string bookID = dgvBook.CurrentRow.Cells["colBookID"].Value.ToString();
                        var delDetail = dbContext.tbl_LoanVoucherDetail.FirstOrDefault(d => d.VoucherID == txtVoucherID.Text && d.BookID == bookID);
                        dbContext.tbl_LoanVoucherDetail.Remove(delDetail);
                        dbContext.SaveChanges();
                    }
                    dgvBook.Rows.RemoveAt(dgvBook.CurrentRow.Index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
    }
}