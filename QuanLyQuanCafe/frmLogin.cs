using QuanLyQuanCafe.DAL;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if (Login(username, password)) 
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUsername(username);
                frmTableManager f = new frmTableManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            } 
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }
        }

        bool Login(string username, string passowrd) 
        {
            return AccountDAO.Instance.Login(username, passowrd);
        }
    }
}