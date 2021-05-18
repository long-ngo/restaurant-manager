using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string userName, string passWord)
        {
            string query = "dbo.USP_GetAccount @userName , @passWord";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] {userName, passWord});
            return data.Rows.Count > 0;
        }

        public Account getAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_Account_By_UserName @userName", new object[] { userName });

            foreach (DataRow item in data.Rows)
            {
                Account account = new Account(item);
                return account;
            }

            return null;
        }

        public DataTable getListAccountFilter()
        {
            return DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_Account_Filter");
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Insert_Account @userName , @displayName , @type", new object[] { userName, displayName, type });
            return result > 0;
        }

        public bool UpdateAccountLogin(string userName, string displayName, string passWord, string newPassWord)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Update_Account_Login @userName , @displayName , @passWord , @newPassWord", new object[] { userName, displayName, passWord, newPassWord });
            return result > 0;
        }

        public bool UpdateAccount(string useUserName, string nameAccount, string userName, string displayName, int type)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Update_Account @useUserName , @nameAccount , @userName , @displayName , @type", new object[] { useUserName, nameAccount, userName, displayName, type });
            return result > 0;
        }

        public bool DeleteAccount(string userNameAccount, string userName)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC dbo.USP_Delete_Account @userNameAccount , @userName", new object[] { userNameAccount, userName });
            return result > 0;
        }
    }
}
