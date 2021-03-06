using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class FoodCategory
    {
        private int id;
        private string name;

        public FoodCategory(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public FoodCategory(DataRow row)
        {
            this.id = (int) row["id"];
            this.name = row["name"].ToString();
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
    }
}
