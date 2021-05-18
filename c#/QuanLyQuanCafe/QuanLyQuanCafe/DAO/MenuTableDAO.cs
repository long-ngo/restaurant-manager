using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class MenuTableDAO
    {
        private static MenuTableDAO instance;

        public static MenuTableDAO Instanse 
        {
            get { if (instance == null) instance = new MenuTableDAO(); return MenuTableDAO.instance; }
            private set { MenuTableDAO.instance = value; }
        }
        
        private MenuTableDAO() { }

        public List<MenuTable> getListMenuByTable(int id)
        {
            List<MenuTable> menuList = new List<MenuTable>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_GetFood_Bill_InfoBill @idTable", new object[] { id });
            foreach (DataRow item in data.Rows)
            {
                MenuTable menuTable = new MenuTable(item);
                menuList.Add(menuTable);
            }
            return menuList;
        }
    }
}
