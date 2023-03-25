using QuanLyQuanCafe.DAL;
using QuanLyQuanCafe.Models;
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

namespace QuanLyQuanCafe
{
    public partial class frmTableManager : Form
    {
        private Account loginAccount;
        private Account LoginAccount 
        { 
            get { return loginAccount; } 
            set { loginAccount = value; ChangeAccount(loginAccount.Type); } 
        }
        public frmTableManager(Account loginAccount)
        {
            InitializeComponent();
            this.LoginAccount = loginAccount;

            LoadTable();

            LoadCategory();

            LoadComboBoxTable(cboSwitchTable);
            LoginAccount = loginAccount;
        }

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cboCategory.DataSource = listCategory;
            cboCategory.DisplayMember = "Name";
        }

        void LoadFoodByCategory(int id)
        {
            cboFood.DataSource = null;
            List<Food> listFood = FoodDAO.Instance.GetFoodListByCategory(id);
            cboFood.DataSource = listFood;
            cboFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table table in tableList)
            {
                Button btn = new Button() { Width = TableDAO.width, Height = TableDAO.height};
                btn.Text = table.Name + Environment.NewLine + table.Status;
                btn.Click += btn_Click;
                btn.Tag = table;
                switch (table.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightBlue;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice+= item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            txtTotalPrice.Text = totalPrice.ToString("c", new CultureInfo("vi-VN"));

            
        }

        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).Id;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        void LoadComboBoxTable(ComboBox cbo)
        {
            cbo.DataSource = TableDAO.Instance.LoadTableList();
            cbo.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).Id;

            int id2 = (cboSwitchTable.SelectedItem as Table).Id;
            Table table1 = (lsvBill.Tag as Table);
            Table table2 = (cboSwitchTable.SelectedItem as Table);
            if (MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển từ bàn {0} qua bàn {1}?", 
                (lsvBill.Tag as Table).Name, (cboSwitchTable.SelectedItem as Table).Name), 
                "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (!table1.Status.ToLower().Equals("trống") && table1.Status.ToLower().Equals(table2.Status.ToLower()))
                {
                    MessageBox.Show(string.Format("Bàn {0} và bàn {1} đều đang có người", table1.Name, table2.Name), "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    TableDAO.Instance.SwitchTable(id1, id2);
                    LoadTable();
                }
                
            }

            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccountProfile f = new frmAccountProfile(LoginAccount);
            f.UpdateAccountEvent += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin f = new frmAdmin();
            f.InsertFood += F_InsertFood;
            f.UpdateFood += F_UpdateFood;
            f.DeleteFood += F_DeleteFood;
            f.ShowDialog();
        }

        private void F_DeleteFood(object? sender, EventArgs e)
        {
            LoadFoodByCategory((cboCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
            LoadTable();
        }

        private void F_UpdateFood(object? sender, EventArgs e)
        {
            LoadFoodByCategory((cboCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
        }

        private void F_InsertFood(object? sender, EventArgs e)
        {
            LoadFoodByCategory((cboCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.Id;
            LoadFoodByCategory(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.Id);
            int foodID = (cboFood.SelectedItem as Food).Id;
            int count =  (int)nudQuantity.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.Id);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.Id);
            int discount = (int)nudDiscount.Value;

            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(' ')[0])*1000;
            double finalTotalPrice = totalPrice*(100 - discount)/100;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho {0}\n Tổng tiền sau khi giảm giá là {1}",table.Name,finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.Id);

                    LoadTable();
                }
            }
        }
    }
}
