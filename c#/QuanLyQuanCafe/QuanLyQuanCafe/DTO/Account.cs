using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        private string userName;
        private string displayName;
        private string passWord;
        private int type;

        public Account(string userName, string displayName, string passWord, int type)
        {
            this.userName = userName;
            this.displayName = displayName;
            this.passWord = passWord;
            this.type = type;
        }

        public Account(DataRow row)
        {
            this.userName = row["UserName"].ToString();
            this.displayName = row["DisplayName"].ToString();
            this.passWord = row["PassWord"].ToString();
            this.type = (int) row["Type"];
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
