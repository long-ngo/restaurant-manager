using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class FoodTable
    {
        private int id;
        private string name;
        private string status;

        public FoodTable(int id, string name, string status)
        {
            this.id = id;
            this.name = name;
            this.status = status;
        }

        public FoodTable(DataRow row)
        {
            this.id = (int) row["id"];
            this.name = row["name"].ToString();
            this.status = row["status"].ToString();
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
