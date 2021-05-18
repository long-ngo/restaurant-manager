using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instence;

        public static BillDAO Instence
        {
            get { if (instence == null) instence = new BillDAO(); return BillDAO.instence; }
            private set { BillDAO.instence = value; }
        }

        private BillDAO() { }

        public int GetUnCheckBillIdByTableId(int idTable)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_GetBill @idTable", new object[] { idTable });
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id; //lấy id của bill
            }
            return -1;
        }

        public void CheckOut(int id, float totalPrice)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Update_Bill @id , @totalPrice", new object[] { id, totalPrice });
        }

        public void InsertBill(int idTable)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Insert_Bill @idTable", new object[] { idTable });
        }

        public int getMaxIdBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
