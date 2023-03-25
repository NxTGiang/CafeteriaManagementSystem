using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.Models
{
    public class BillInfo
    {
        public int Id { get; set; }
        public int BillId { get; set; }

        public int FoodId { get; set; }

        public int Count { get; set; }

        public BillInfo() { }

        public BillInfo(int id, int billId, int foodId, int count)
        {
            Id = id;
            BillId = billId;
            FoodId = foodId;
            Count = count;
        }

        public BillInfo(DataRow row)
        {
            Id = (int)row["id"];
            BillId = (int)row["IDBill"];
            FoodId = (int)row["IDFood"];
            Count = (int)row["count"];
        }
    }
}
