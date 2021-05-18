using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillSave
    {
        private int id;
        private DateTime? dateCheckIn;
        private DateTime? dateCheckOut;
        private string idTable;
        private float totalPrice;

        public BillSave(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, string idTable, float totalPrice)
        {
            this.id = id;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.idTable = idTable;
            this.totalPrice = totalPrice;
        }

        public BillSave(DataRow row)
        {
            this.id = (int)row["id"];
            this.dateCheckIn = (DateTime?)row["DateCheckIn"];
            this.dateCheckOut = (DateTime?)row["DateCheckOut"];
            this.idTable = row["name"].ToString();
            this.totalPrice = (float)(Convert.ToDouble(row["totalPrice"].ToString()));
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }

        public string IdTable
        {
            get { return idTable; }
            set { idTable = value; }
        }

        public float TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }
    }
}
