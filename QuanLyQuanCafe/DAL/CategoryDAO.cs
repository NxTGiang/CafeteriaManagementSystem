using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAL
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO();  return instance; }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> categories = new List<Category>();

            DataTable data = DBContext.Instance.ExcuteQuery("SELECT * FROM FoodCategory");

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                categories.Add(category);
            }

            return categories;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;

            DataTable data = DBContext.Instance.ExcuteQuery("SELECT * FROM FoodCategory WHERE id = " + id);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
            }

            return category;
        }
    }
}
