using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<Food> getFoodListByCategoryID(int categoryID)
        {
            List<Food> listFood = new List<Food>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_FoodByCategoryId @categoryId", new object[] { categoryID });
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }

        public DataTable getListFoodFilter()
        {
            return DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_Food_Filter");
        }

        public Food getFoodByName(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_Food_By_FoodName @foodName", new object[] { name });
            Food food = new Food(data.Rows[0]);
            return food;
        }

        public bool InsertFood(string name, int idCategory, float price)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Insert_Food @name , @idCategory , @price", new object[] { name, idCategory, price });
            return result > 0;
        }

        public bool UpdateFood(int id, string name, int idCategory, float price)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Update_Food @id , @name , @idCategory , @price", new object[] { id, name, idCategory, price });
            return result > 0;
        }

        public bool DeleteFood(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Delete_Food_By_Id @id", new object[] { id });
            return result > 0;
        }

    }
}
