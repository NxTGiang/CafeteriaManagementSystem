using QuanLyQuanCafe.DAL;
using QuanLyQuanCafe.Models;
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
    public partial class frmAccountProfile : Form
    {
        private Account loginAccount;
        private Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public frmAccountProfile(Account acc)
        {
            InitializeComponent();

            LoginAccount  = acc;
        }

        void ChangeAccount(Account acc)
        {
            txtUsername.Text = LoginAccount.Username;
            txtDisplayName.Text = LoginAccount.DisplayName;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }

        void UpdateAccount()
        {
            string displayName = txtDisplayName.Text;
            string password = txtPassword.Text;
            string newPass = txtNewPassword.Text;
            string reEnterPass = txtReEnter.Text;
            string username = txtUsername.Text;
            if (!newPass.Equals(reEnterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(username, displayName, password, newPass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                    {
                        updateAccount(this, new AccountEvent (AccountDAO.Instance.GetAccountByUsername(username)));
                    }
                } 
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;

        public event EventHandler<AccountEvent> UpdateAccountEvent
        { 
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
    }

    public class AccountEvent : EventArgs
    {
        public Account Acc { get; set; }

        public AccountEvent(Account acc)
        {
            Acc = acc;
        }
    }
}
