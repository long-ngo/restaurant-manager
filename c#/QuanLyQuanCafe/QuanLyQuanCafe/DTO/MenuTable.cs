using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class MenuTable
    {
        private string foodName; //tên thức ăn
        private int count; //số lượng
        private float price; //giá
        private float intoMoney; //thành tiền

        public MenuTable(string foodName, int count, float unitPrice, float intoMoney)
        {
            this.foodName = foodName;
            this.count = count;
            this.price = unitPrice;
            this.intoMoney = intoMoney;
        }

        public MenuTable(DataRow row)
        {
            this.foodName = row["name"].ToString();
            this.count = (int) row["count"];
            this.price = (float) Convert.ToDouble(row["price"].ToString());
            this.intoMoney = (float) Convert.ToDouble(row["intoMoney"].ToString());
        }

        public string FoodName 
        {
            get { return foodName; }
            set { foodName = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public float IntoMoney
        {
            get { return intoMoney; }
            set { intoMoney = value; }
        }
    }
}
