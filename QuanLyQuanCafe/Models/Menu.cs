using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.Models
{
    public class Menu
    {
        public string FoodName { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }

        public float TotalPrice { get; set; }

        public Menu(string foodName, int count, float price, float totalPrice)
        {
            FoodName = foodName;
            Count = count;
            Price = price;
            TotalPrice = totalPrice;
        }

        public Menu(DataRow row)
        {
            FoodName = row["name"].ToString();
            Count = (int)row["count"];
            Price = (float)Convert.ToDouble(row["price"].ToString());
            TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }
    }
}
