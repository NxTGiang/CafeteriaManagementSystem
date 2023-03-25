using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IDCategory { get; set; }

        public float Price { get; set; }

        public Food(int id, string name, int idCategory, float price) 
        { 
            this.Id = id;
            this.Name = name;   
            this.IDCategory = idCategory;
            this.Price = price;
        }

        public Food(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
            this.IDCategory = (int)row["idCategory"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }
    }
}
