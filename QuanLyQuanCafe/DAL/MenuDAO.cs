using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAL
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return instance; }
            private set { instance = value; }
        }

        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> menus = new List<Menu>();
            DataTable data = DBContext.Instance.ExcuteQuery("SELECT f.[Name], bi.[count], f.price, f.price*bi.[count]" +
                " AS TotalPrice FROM dbo.Bill b, dbo.BillInfo bi, dbo.Food f\r\nWHERE bi.IDBill = b.ID AND bi.IDFood = f.ID AND b.Status = 0 AND b.IDTable = " + id);
            
            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                menus.Add(menu);
            }
            return menus;
        }
    }
}
