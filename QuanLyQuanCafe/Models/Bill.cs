using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyQuanCafe.Models
{
    public class Bill
    {
        public int Id { get; set; }

        public DateTime? DateCheckIn { get; set; }

        public DateTime? DateCheckOut { get; set; }

        public int Status { get; set; }

        public int Discount { get; set; }
        public Bill() { }   

        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int status, int discount)
        {
            this.Id = id;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.Discount = discount;
        }

        public Bill(DataRow row) 
        {
            this.Id = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["dateCheckIn"];

            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)row["dateCheckOut"];
            this.Status = (int)row["status"];
            if (row["Discount"].ToString() != "")
                this.Discount = (int)row["Discount"];
        }
    }
}
