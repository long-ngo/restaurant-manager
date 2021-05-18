using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public fTableManager(Account account)
        {
            InitializeComponent();
            LoginAccount = account;
            LoadTable();
            LoadCategory();
        }

        #region Method

        void ChangeAccount(int type)
        {
            if (type == 2)
                adminToolStripMenuItem.Enabled = false;
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<FoodTable> tableList = FoodTableDAO.Instance.LoadTableList();
            string str = "";

            foreach (FoodTable item in tableList)
            {
                Button btn = new Button();
                btn.Width = FoodTableDAO.TABLE_WIDTH;
                btn.Height = FoodTableDAO.TABLE_WIDTH;
                btn.Text = String.Format("{0}\n{1}", item.Name, item.Status);
                btn.Click += btn_Click; //thêm sự kiện click
                btn.Tag = item; //lưu thông tin của button vào thẻ tag để sử dụng

                str = item.Status.ToLower();

                if (str.Contains("trống"))
                {
                    btn.BackColor = Color.Pink;
                }
                else if (str.Contains("có"))
                {
                    btn.BackColor = Color.Aqua;
                }
                else
                {
                    btn.BackColor = Color.Gold;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void LoadCategory()
        {
            List<FoodCategory> listFoodCategory = FoodCategoryDAO.Instence.getListFoodCategory();
            cbCategory.DataSource = listFoodCategory;
            cbCategory.DisplayMember = "name";
        }

        void LoadFoodListByCategoryID(int categoryId)
        {
            List<Food> listFood = FoodDAO.Instance.getFoodListByCategoryID(categoryId);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "name";
        }

        void ReloadFood()
        {
            if (cbCategory.SelectedItem == null)
                cbFood.SelectedItem = null;
        }

        void ShowBill(int foodTableId)
        {
            lsvBill.Items.Clear();
            float totalPrice = 0;
            List <MenuTable> listMenuTable = MenuTableDAO.Instanse.getListMenuByTable(foodTableId);

            foreach (MenuTable item in listMenuTable)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName);
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.IntoMoney.ToString());
                lsvBill.Items.Add(lsvItem);
                totalPrice += item.IntoMoney;
            }

            CultureInfo culture = new CultureInfo("vi-VN"); //chuyển tiền tệ
            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }

        #endregion

        #region Events

        private void btn_Click(object sender, EventArgs e)
        {
            FoodTable foodTable = ((sender as Button).Tag) as FoodTable;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(foodTable.Id);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            FoodCategory foodCategory = cb.SelectedItem as FoodCategory;
            LoadFoodListByCategoryID(foodCategory.Id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (lsvBill.Tag == null)
            {
                MessageBox.Show("Hãy chọn bàn trước khi thêm món", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbCategory.SelectedItem == null)
            {
                MessageBox.Show("Hãy thêm danh mục trước khi thêm món", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbFood.SelectedItem == null)
            {
                MessageBox.Show("Chưa có món cho danh mục này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FoodTable foodTable = (lsvBill.Tag) as FoodTable;
            int idBill = BillDAO.Instence.GetUnCheckBillIdByTableId(foodTable.Id);
            int idFood = (cbFood.SelectedItem as Food).Id;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1) //kiểm tra bill tồn tại chưa
            {
                BillDAO.Instence.InsertBill(foodTable.Id);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instence.getMaxIdBill(), idFood, count);
            }
            else // bill đã tồn tại
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }

            ShowBill(foodTable.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            FoodTable foodTable = lsvBill.Tag as FoodTable;

            if (foodTable == null)
            {
                MessageBox.Show("Hãy chọn bàn trước khi thanh toán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idBill = BillDAO.Instence.GetUnCheckBillIdByTableId(foodTable.Id);
            float totalPrice = 0;
            int i = 0;

            foreach (ListViewItem item in lsvBill.Items)
            {
                totalPrice += float.Parse(lsvBill.Items[i].SubItems[3].Text);
                i++;
            }

            if (foodTable.Status.ToLower().Contains("có người") && idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho {0}", foodTable.Name), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    BillDAO.Instence.CheckOut(idBill, totalPrice);
                    ShowBill(foodTable.Id);
                    LoadTable();
                }
            }
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loginAccount = AccountDAO.Instance.getAccountByUserName(loginAccount.UserName);
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin(loginAccount);
            f.InsertFood += F_InsertFood;
            f.UpdateFood += F_UpdateFood;
            f.DeleteFood += F_DeleteFood;
            f.InsertFoodCategory += F_InsertFoodCategory;
            f.UpdateFoodCategory += F_UpdateFoodCategory;
            f.DeleteFoodCategory += F_DeleteFoodCategory;
            f.InsertFoodTable += F_InsertFoodTable;
            f.UpdateFoodTable += F_UpdateFoodTable;
            f.DeleteFoodTable += F_DeleteFoodTable;
            f.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();
        }

        private void F_UpdateAccount(object sender, EventArgs e)
        {
            LoginAccount = AccountDAO.Instance.getAccountByUserName(loginAccount.UserName);
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as FoodCategory).Id);

            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as FoodTable).Id);
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as FoodCategory).Id);

            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as FoodTable).Id);
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as FoodCategory).Id);

            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as FoodTable).Id);
        }

        private void F_InsertFoodCategory(object sender, EventArgs e)
        {
            LoadCategory();

            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as FoodTable).Id);
        }

        private void F_UpdateFoodCategory(object sender, EventArgs e)
        {
            LoadCategory();

            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as FoodTable).Id);
        }

        private void F_DeleteFoodCategory(object sender, EventArgs e)
        {
            LoadCategory();
            ReloadFood();

            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as FoodTable).Id);
        }

        private void F_InsertFoodTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void F_UpdateFoodTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void F_DeleteFoodTable(object sender, EventArgs e)
        {
            LoadTable();

            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as FoodTable).Id);
        }

        private void btnRefreshTable_Click(object sender, EventArgs e)
        {
            FoodTable foodTable = (lsvBill.Tag) as FoodTable;

            if (foodTable == null)
            {
                MessageBox.Show("Hãy chọn bàn trước khi chọn chức năng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (foodTable.Status.ToLower().Contains("đã thanh toán") && MessageBox.Show(string.Format("Bạn có chắc muốn dọn {0}", foodTable.Name), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                FoodTableDAO.Instance.RefreshTable(foodTable.Id);
                LoadTable();
            }
        }

        #endregion
    }
}
