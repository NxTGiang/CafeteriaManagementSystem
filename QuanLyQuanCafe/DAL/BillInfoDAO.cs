using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAL
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return instance; }
            private set { instance = value; }
        }

        private BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();
            DataTable data = DBContext.Instance.ExcuteQuery("SELECT * FROM dbo.BillInfo WHERE IDBill = " + id);

            foreach (DataRow item in data.Rows) 
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }
            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DBContext.Instance.ExcuteNonQuery("USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }

        public void DeleteBillInfoByFoodID(int id)
        {
            DBContext.Instance.ExcuteQuery("Delete dbo.BillInfo WHERE idFood = " + id);
        }
    }
}
