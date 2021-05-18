using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillSaveDAO
    {
        private static BillSaveDAO instance;

        public static BillSaveDAO Instance
        {
            get { if (instance == null) instance = new BillSaveDAO(); return BillSaveDAO.instance; }
            private set { BillSaveDAO.instance = value; }
        }
        private BillSaveDAO() { }

        public List<BillSave> getListBillSave()
        {
            List<BillSave> listBillSave = new List<BillSave>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_BillSave");

            foreach (DataRow item in data.Rows)
            {
                BillSave billSave = new BillSave(item);
                listBillSave.Add(billSave);
            }

            return listBillSave;
        }

        public DataTable getListBillSaveByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_List_Bill_By_Date @dateCheckIn , @dateCheckOut", new object[] { dateCheckIn, dateCheckOut });
        }
    }
}
