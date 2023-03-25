using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAL
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static int width = 85;
        public static int height = 85;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> list = new List<Table>();

            DataTable data = DBContext.Instance.ExcuteQuery("USP_GetTableList");

            foreach(DataRow item in data.Rows)
            {
                Table table = new Table(item);
                list.Add(table);
            }

            return list;
        }

        public void SwitchTable(int id1, int id2)
        {
            DBContext.Instance.ExcuteQuery("USP_SwitchTable @idFirstTable , @idSecondTable", new object[] {id1, id2});
        }
    }
}
