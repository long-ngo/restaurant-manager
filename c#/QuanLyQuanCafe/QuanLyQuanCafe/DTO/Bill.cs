using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        private int id;
        private DateTime? dateCheckIn;
        private DateTime? dateCheckOut;
        private int idTable;
        private int status;
        private float totalPrice;

        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int status, float totalPrice)
        {
            this.id = id;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.status = status;
            this.totalPrice = totalPrice;
        }

        public Bill(DataRow row)
        {
            this.id = (int) row["id"];
            this.dateCheckIn = (DateTime?) row["DateCheckIn"];
            var dateCheckOutTemp = row["DateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
            {
                this.dateCheckOut = (DateTime?) dateCheckOutTemp;
            }
            this.idTable = (int) row["idTable"];
            this.status = (int) row["status"];
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
            set { dateCheckOut = value; }
        }

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }

        public int IdTable
        {
            get { return idTable; }
            set { idTable = value; }
        }
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
