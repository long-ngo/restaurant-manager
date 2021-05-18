using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public fAccountProfile(Account account)
        {
            InitializeComponent();
            LoginAccount = account;
        }

        public void ChangeAccount(Account account)
        {
            txbUserName.Text = account.UserName;
            txbDisplayName.Text = account.DisplayName;
        }

        #region Events

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txbPassWord.Text == "")
            {
                MessageBox.Show("Nhập mật khẩu trước khi thực hiện cập nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            string passWord = txbPassWord.Text;
            string newPassWord = txbNewPass.Text;
            string reEnterPass = txbReEnterPass.Text;
            bool result = false;

            if (newPassWord.Equals(reEnterPass))
            {
                result = AccountDAO.Instance.UpdateAccountLogin(userName, displayName, passWord, newPassWord);
            }
            else
            {
                MessageBox.Show("Mật khẩu mới không khớp với mật khẩu nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (result)
            {
                MessageBox.Show("Cập nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cập nhập không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
