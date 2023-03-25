using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAL
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return instance; }
            private set { instance = value; }
        }

        private FoodDAO() { }

        public List<Food> GetFoodListByCategory(int id)
        {
            List<Food> foods = new List<Food>();

            DataTable data = DBContext.Instance.ExcuteQuery("SELECT * FROM Food WHERE IDCategory = " + id);

            foreach (DataRow item in data.Rows) 
            {
                Food food = new Food(item);
                foods.Add(food);
            }

            return foods;
        }

        public List<Food> GetListFood()
        {
            List<Food> foods = new List<Food>();

            DataTable data = DBContext.Instance.ExcuteQuery("SELECT * FROM Food ");

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                foods.Add(food);
            }

            return foods;
        }

        public bool InsertFood(string name, int id, float price)
        {
            int result = DBContext.Instance.ExcuteNonQuery("INSERT dbo.Food ([Name], IDCategory, Price) VALUES (N'"+ name +"', "+id+", "+price+")");

            return result > 0;
        }

        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            int result = DBContext.Instance.ExcuteNonQuery("UPDATE dbo.Food SET [Name] = N'"+ name+"', IDCategory = "+id+", Price = " + price + "WHERE id = " + idFood);

            return result > 0;
        }

        public bool DeleteFood(int id)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(id);
            int result = DBContext.Instance.ExcuteNonQuery("DELETE Food WHERE id = "+ id);

            return result > 0;
        }
    }
}
