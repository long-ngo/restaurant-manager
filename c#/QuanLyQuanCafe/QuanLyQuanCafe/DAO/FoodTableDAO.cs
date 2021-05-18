using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodTableDAO
    {
        private static FoodTableDAO instance;

        public static FoodTableDAO Instance
        {
            get { if (instance == null) instance = new FoodTableDAO(); return FoodTableDAO.instance; }
            private set { FoodTableDAO.instance = value; }
        }

        private FoodTableDAO() { }

        public static int TABLE_WIDTH = 90;
        public static int TABLE_HIGHT = 90;

        public List<FoodTable> LoadTableList()
        {
            List<FoodTable> tableList = new List<FoodTable>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_GetFoodTable");
            foreach (DataRow item in data.Rows)
            {
                FoodTable table = new FoodTable(item);
                tableList.Add(table);
            }
            return tableList;
        }

        public DataTable getListFoodTableFilter()
        {
            return DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_FoodTable_Filter");
        }

        public FoodTable getFoodTableByName(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_FoodTable_By_Name @name", new object[] { name });
            FoodTable foodTable = new FoodTable(data.Rows[0]);
            return foodTable;
        }

        public void RefreshTable(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Refresh_Table @idTable", new object[] { id });
        }

        public bool InsertFoodTable(string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Insert_FoodTable @name", new object[] { name });
            return result > 0;
        }

        public bool UpdateFoodTable(int id, string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Update_FoodTable @id , @name", new object[] { id, name });
            return result > 0;
        }

        public bool DeleteFoodTable(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Delete_FoodTable_By_Id @id", new object[] { id });
            return result > 0;
        }
    }
}
