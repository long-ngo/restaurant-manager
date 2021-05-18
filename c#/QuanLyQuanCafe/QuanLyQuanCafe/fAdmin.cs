using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; }
        }

        BindingSource foodList = new BindingSource();
        BindingSource foodCategoryList = new BindingSource();
        BindingSource foodTableList = new BindingSource();
        BindingSource accountList = new BindingSource();

        public fAdmin(Account account)
        {
            InitializeComponent();
            LoginAccount = account; //dòng mới thêm vào
            LoadAll();
        }

        void LoadAll() //sửa tên
        {
            dtgvFood.DataSource = foodList;
            dtgvCategory.DataSource = foodCategoryList;
            dtgvTable.DataSource = foodTableList;
            dtgvAccount.DataSource = accountList;

            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadCategoryIntoCombobox(cbFoodCategory);
            LoadAccountTypeIntoCombobox(cbAccountType);
            LoadListFoodCategory();
            LoadListTable();
            LoadListAccount();

            AddFoodBinding();
            AddFoodCategoryBinding();
            AddFoodTableBinding();
            AddAccountBidding();
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillSaveDAO.Instance.getListBillSaveByDate(checkIn, checkOut);
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.getListFoodFilter();
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = FoodCategoryDAO.Instence.getListFoodCategory();
            cb.DisplayMember = "name";
        }

        void LoadAccountTypeIntoCombobox(ComboBox cb)
        {
            cb.DataSource = AccountTypeDAO.Instance.getListAccountType();
            cb.DisplayMember = "name";
        }

        void LoadListFoodCategory()
        {
            foodCategoryList.DataSource = FoodCategoryDAO.Instence.getListFoodCategoryFilter();
        }

        void LoadListTable()
        {
            foodTableList.DataSource = FoodTableDAO.Instance.getListFoodTableFilter();
        }

        void LoadListAccount()
        {
            accountList.DataSource = AccountDAO.Instance.getListAccountFilter();
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Tên món", false, DataSourceUpdateMode.Never));
            cbFoodCategory.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Danh mục", false, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Giá", false, DataSourceUpdateMode.Never));
        }

        void AddFoodCategoryBinding()
        {
            txbFoodCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Danh mục", false, DataSourceUpdateMode.Never));
        }

        void AddFoodTableBinding()
        {
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Tên bàn", false, DataSourceUpdateMode.Never));
        }

        void AddAccountBidding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên tài khoản", false, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên hiển thị", false, DataSourceUpdateMode.Never));
            cbAccountType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Loại tài khoản", false, DataSourceUpdateMode.Never));
        }

    #region Event

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnAddFood_Click(object sender, EventArgs e)//thừa cbfood category dòng 134
        {
            if (txbFoodName.Text == "" || cbFoodCategory.Text == "" || nmFoodPrice.Value == 0)
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txbFoodName.Text;
            int idCategory = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmFoodPrice.Value;
            bool result = FoodDAO.Instance.InsertFood(name, idCategory, price);

            if (result)
            {
                MessageBox.Show("Thêm món thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListFood();

                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Tên món đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            if (dtgvFood.CurrentCell == null)
            {
                MessageBox.Show("Hãy thêm món trước khi thực hiện chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txbFoodName.Text == "" || cbFoodCategory.Text == "" || nmFoodPrice.Value == 0)
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string foodName = dtgvFood.SelectedRows[0].Cells["Tên món"].Value.ToString();
            Food food = FoodDAO.Instance.getFoodByName(foodName);

            string name = txbFoodName.Text;
            int idCategory = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmFoodPrice.Value;
            bool result = FoodDAO.Instance.UpdateFood(food.Id, name, idCategory, price);

            if (result)
            {
                MessageBox.Show("Cập nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListFood();

                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Cập nhập không thành công. Tên món, danh mục, giá không được đồng thời trùng nhau", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            if (dtgvFood.CurrentCell == null)
            {
                MessageBox.Show("Bạn đã xóa hết danh sách món", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string foodName = dtgvFood.SelectedRows[0].Cells["Tên món"].Value.ToString();
            Food food = FoodDAO.Instance.getFoodByName(foodName);

            if (MessageBox.Show("Bạn có chắc muốn xóa không? Việc này có thể sẽ xóa cả hóa đơn và không thể hoàn tác", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bool result = FoodDAO.Instance.DeleteFood(food.Id);

                if (result)
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadListFood();

                    if (deleteFood != null)
                    {
                        deleteFood(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            if (txbFoodCategory.Text == "")
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txbFoodCategory.Text;
            bool result = FoodCategoryDAO.Instence.InsertFoodCategory(name);

            if (result)
            {
                MessageBox.Show("Thêm danh mục thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListFoodCategory();
                LoadCategoryIntoCombobox(cbFoodCategory);

                if (insertFoodCategory != null)
                {
                    insertFoodCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Tên danh mục đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            if (dtgvCategory.CurrentCell == null)
            {
                MessageBox.Show("Hãy thêm danh mục trước khi thực hiện chỉnh sủa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txbFoodCategory.Text == "")
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nameFoodCategory = dtgvCategory.SelectedRows[0].Cells["Danh mục"].Value.ToString();
            FoodCategory foodCategory = FoodCategoryDAO.Instence.getFoodCategoryByName(nameFoodCategory);
            string name = txbFoodCategory.Text;
            bool result = FoodCategoryDAO.Instence.UpdateFoodCategory(foodCategory.Id, name);

            if (result)
            {
                MessageBox.Show("Cập nhập danh mục thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListFoodCategory();
                LoadListFood();
                LoadCategoryIntoCombobox(cbFoodCategory);

                if (updateFoodCategory != null)
                {
                    updateFoodCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Tên danh mục đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (dtgvCategory.CurrentCell == null)
            {
                MessageBox.Show("Bạn đã xóa hết danh sách danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nameFoodCategory = dtgvCategory.SelectedRows[0].Cells["Danh mục"].Value.ToString();
            FoodCategory foodCategory = FoodCategoryDAO.Instence.getFoodCategoryByName(nameFoodCategory);

            if (MessageBox.Show("Bạn có chắc muốn xóa không? Việc này sẽ xóa cả danh sách thức ăn đi kèm và không thể hoàn tác", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bool result = FoodCategoryDAO.Instence.DeleteFoodCategory(foodCategory.Id);

                if (result)
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadListFoodCategory();
                    LoadListFood();
                    LoadCategoryIntoCombobox(cbFoodCategory);

                    if (deleteFoodCategory != null)
                    {
                        deleteFoodCategory(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            if (txbTableName.Text == "")
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin vào mục tên bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txbTableName.Text;
            bool result = FoodTableDAO.Instance.InsertFoodTable(name);

            if (result)
            {
                MessageBox.Show("Thêm bàn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListTable();

                if (insertFoodTable != null)
                {
                    insertFoodTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Tên bàn không được trùng nhau. Hãy thử với tên khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            if (dtgvTable.CurrentCell == null)
            {
                MessageBox.Show("Hãy thêm bàn trước khi chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txbTableName.Text == "")
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin vào mục tên bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string foodTableName = dtgvTable.SelectedRows[0].Cells["Tên bàn"].Value.ToString();
            FoodTable foodTable = FoodTableDAO.Instance.getFoodTableByName(foodTableName);
            string name = txbTableName.Text;
            bool result = FoodTableDAO.Instance.UpdateFoodTable(foodTable.Id, name);

            if (result)
            {
                MessageBox.Show("Cập nhập bàn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListTable();

                if (updateFoodTable != null)
                {
                    updateFoodTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Tên bàn không được trùng nhau. Hãy thử với tên khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            if (dtgvTable.CurrentCell == null)
            {
                MessageBox.Show("Bạn đã xóa hết bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string foodTableName = dtgvTable.SelectedRows[0].Cells["Tên bàn"].Value.ToString();
            FoodTable foodTable = FoodTableDAO.Instance.getFoodTableByName(foodTableName);

            if (MessageBox.Show("Bạn có chắc muốn xóa không? Việc này sẽ xóa cả hóa đơn và không thể hoàn tác", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bool result = FoodTableDAO.Instance.DeleteFoodTable(foodTable.Id);

                if (result)
                {
                    MessageBox.Show("Xóa bàn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadListTable();

                    if (deleteFoodTable != null)
                    {
                        deleteFoodTable(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Xóa bàn không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (txbUserName.Text == "" || txbDisplayName.Text == "" || cbAccountType.SelectedItem == null)
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (cbAccountType.SelectedItem as AccountType).Id;
            bool result = AccountDAO.Instance.InsertAccount(userName, displayName, type);

            if (result)
            {
                MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Tên tài khoản đã được sử dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {

            if (txbUserName.Text == "" || txbDisplayName.Text == "" || cbAccountType.SelectedItem == null)
            {
                MessageBox.Show("Hãy điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nameAccount = dtgvAccount.SelectedRows[0].Cells["tên tài khoản"].Value.ToString();
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (cbAccountType.SelectedItem as AccountType).Id;
            bool result = AccountDAO.Instance.UpdateAccount(loginAccount.UserName, nameAccount, userName, displayName, type);

            if (result)
            {
                MessageBox.Show("Cập nhập tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListAccount();

                if (updateAccount != null)
                {
                    updateAccount(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Cập nhập không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = dtgvAccount.SelectedRows[0].Cells["Tên tài khoản"].Value.ToString();

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bool result = AccountDAO.Instance.DeleteAccount(loginAccount.UserName, userName);

                if (result)
                {
                    MessageBox.Show("Xóa tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadListAccount();
                }
                else
                {
                    MessageBox.Show("Bạn đang sử dụng tài khoản này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }          
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListFoodCategory();
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }

        #region Event Handler

        private event EventHandler insertFood;
        private event EventHandler updateFood;
        private event EventHandler deleteFood;
        private event EventHandler insertFoodCategory;
        private event EventHandler updateFoodCategory;
        private event EventHandler deleteFoodCategory;
        private event EventHandler insertFoodTable;
        private event EventHandler updateFoodTable;
        private event EventHandler deleteFoodTable;
        private event EventHandler updateAccount;

        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        public event EventHandler InsertFoodCategory
        {
            add { insertFoodCategory += value; }
            remove { insertFoodCategory -= value; }
        }

        public event EventHandler UpdateFoodCategory
        {
            add { updateFoodCategory += value; }
            remove { updateFoodCategory -= value; }
        }

        public event EventHandler DeleteFoodCategory
        {
            add { deleteFoodCategory += value; }
            remove { deleteFoodCategory -= value; }
        }

        public event EventHandler InsertFoodTable
        {
            add { insertFoodTable += value; }
            remove { insertFoodTable -= value; }
        }

        public event EventHandler UpdateFoodTable
        {
            add { updateFoodTable += value; }
            remove { updateFoodTable -= value; }
        }

        public event EventHandler DeleteFoodTable
        {
            add { deleteFoodTable += value; }
            remove { deleteFoodTable -= value; }
        }

        public event EventHandler UpdateAccount //mới thêm vào
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        #endregion

    #endregion
    }
}
