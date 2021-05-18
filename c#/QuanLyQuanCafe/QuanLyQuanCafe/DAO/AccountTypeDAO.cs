using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountTypeDAO
    {
        private static AccountTypeDAO instance;

        public static AccountTypeDAO Instance
        {
            get { if (instance == null) instance = new AccountTypeDAO(); return AccountTypeDAO.instance; }
            private set { AccountTypeDAO.instance = value; }
        }

        private AccountTypeDAO() { }

        public List<AccountType> getListAccountType()
        {
            List<AccountType> listAccountType = new List<AccountType>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_Get_AccountType");

            foreach (DataRow item in data.Rows)
            {
                AccountType accountType = new AccountType(item);
                listAccountType.Add(accountType);
            }

            return listAccountType;
        }
    }
}
