using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Table(int id, string name, string status)
        {
            Id = id;
            Name = name;
            Status = status;
        }

        public Table(DataRow row) 
        { 
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Status = row["status"].ToString();
        }
    }
}
