using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAL
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set { instance = value; }
        }

        private BillDAO() { }

        public int GetUnCheckBillIDByTableID(int tableID)
        {
            DataTable data = DBContext.Instance.ExcuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + tableID + " AND Status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }
            return -1;
        }

        public void InsertBill(int id)
        {
            DBContext.Instance.ExcuteNonQuery("USP_InsertBill @idTable", new object[] {id});
        }

        public int GetMaxIDBill()
        {
            try 
            {
                return (int)DBContext.Instance.ExcuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
            
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            DBContext.Instance.ExcuteNonQuery("UPDATE dbo.Bill SET dateCheckOut = GETDATE(), status = 1, discount = "+ discount +" , totalPrice = "+ totalPrice +" WHERE id = " + id);

        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DBContext.Instance.ExcuteQuery("USP_GetListBillByDate @chechIn , @checkOut", new object[] {checkIn, checkOut});
        }
    }
}
