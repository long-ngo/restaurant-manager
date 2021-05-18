using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodCategoryDAO
    {
        private static FoodCategoryDAO instence;

        public static FoodCategoryDAO Instence
        {
            get { if (instence == null) instence = new FoodCategoryDAO(); return FoodCategoryDAO.instence; }
            private set { FoodCategoryDAO.instence = value; }
        }

        private FoodCategoryDAO() { }

        public List<FoodCategory> getListFoodCategory()
        {
            List<FoodCategory> listFoodCategory = new List<FoodCategory>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_GetFoodCategory");
            foreach (DataRow item in data.Rows)
            {
                FoodCategory category = new FoodCategory(item);
                listFoodCategory.Add(category);
            }
            return listFoodCategory;
        }

        public FoodCategory getFoodCategoryByName(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_FoodCategory_By_Name @name", new object[] { name });
            FoodCategory foodCategory = new FoodCategory(data.Rows[0]);
            return foodCategory;
        }

        public DataTable getListFoodCategoryFilter()
        {
            return DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_Food_Category_Filter");
        }

        public bool InsertFoodCategory(string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Insert_FoodCategory @name", new object[] { name });
            return result > 0;
        }

        public bool UpdateFoodCategory(int id, String name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Update_FoodCategory @id , @name", new object[] { id, name });
            return result > 0;
        }

        public bool DeleteFoodCategory(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Delete_FoodCategory_By_Id @id", new object[] { id });
            return result > 0;
        }
    }
}
