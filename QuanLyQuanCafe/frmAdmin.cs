using Microsoft.SqlServer.Management.Sdk.Sfc;
using QuanLyQuanCafe.DAL;
using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class frmAdmin : Form
    {

        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();
        public frmAdmin()
        {
            InitializeComponent();

            LoadData();
        }

        void LoadData()
        {
            dgvFoodList.DataSource = foodList;
            dgvAccount.DataSource = accountList;
            LoadDateTimePickerBill();

            LoadListByDate(dtpFrom.Value, dtpTo.Value);

            LoadListFood();
            LoadAccount();

            LoadCategoryIntoCbo(cboFoodCategory);

            AddFoodBinding();
            AddAccountBinding();

        }

        void AddAccountBinding()
        {
            txtUsername.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "username", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "displayName", true, DataSourceUpdateMode.Never));
            nudType.DataBindings.Add(new Binding("Value", dgvAccount.DataSource, "type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dgvFoodList.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtIDFood.DataBindings.Add(new Binding("Text", dgvFoodList.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nudPrice.DataBindings.Add(new Binding("Value", dgvFoodList.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCbo(ComboBox cbo)
        {
            cbo.DataSource = CategoryDAO.Instance.GetListCategory();
            cbo.DisplayMember = "Name";
        }
        private void tpFood_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tpAccount_Click(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            LoadListByDate(dtpFrom.Value, dtpTo.Value);
        }

        void LoadListByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
            dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void txtIDFood_TextChanged(object sender, EventArgs e)
        {
            if (dgvFoodList.SelectedCells.Count > 0)
            {
                int id = (int)dgvFoodList.SelectedCells[0].OwningRow.Cells["IDCategory"].Value;
                
                Category category = CategoryDAO.Instance.GetCategoryByID(id);

                cboFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;
                foreach (Category item in cboFoodCategory.Items)
                {
                    if (item.Id == category.Id)
                    {
                        index  = i; break;
                    }
                    i++;
                }
            }

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int IDCategory = (cboFoodCategory.SelectedItem as Category).Id;
            float price = (float)nudPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, IDCategory, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int IDCategory = (cboFoodCategory.SelectedItem as Category).Id;
            float price = (float)nudPrice.Value;
            int id = Convert.ToInt32(txtIDFood.Text);
            if (FoodDAO.Instance.UpdateFood(id, name, IDCategory, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDFood.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món");
            }
        }

        private event EventHandler insertFood;

        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;

        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;

        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        void AddAccount(string username, string display, int type)
        {
            if (AccountDAO.Instance.InsertAccount(username, display, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
            LoadAccount();
        }

        void EditAccount(string username, string display, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(username, display, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }
            LoadAccount();
        }

        void DeleteAccount(string username)
        {
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string displayname = txtDisplayName.Text;
            int type = (int)nudType.Value;
            AddAccount(username, displayname, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string displayname = txtDisplayName.Text;
            int type = (int)nudType.Value;
            EditAccount(username, displayname, type);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string displayname = txtDisplayName.Text;
            int type = (int)nudType.Value;
            DeleteAccount(username);
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            ResetPassword(username);
        }

        void ResetPassword(string username)
        {
            if (AccountDAO.Instance.ResetPass(username))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
            LoadAccount();
        }
    }
}
