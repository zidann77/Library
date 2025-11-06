using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library.People
{
    public partial class frmListPeople : Form
    {
        DataTable dt;
        string CurrentFilter;

        public frmListPeople()
        {
            InitializeComponent();
        }

        void Reloaddata()
        {
            dt = clsPeople.GetAllPeople();
            guna2DataGridView1.DataSource = dt;
        }

        void setupDataGridView()
        {
            //   guna2DataGridView1.EnableHeadersVisualStyles = false;
            guna2DataGridView1.DataSource = dt;

            // rename headers
            guna2DataGridView1.Columns[0].HeaderText = "ID";
            guna2DataGridView1.Columns[1].HeaderText = "Full Name";
            guna2DataGridView1.Columns[2].HeaderText = "Phone";
            guna2DataGridView1.Columns[3].HeaderText = "Email";

            // adjust widths
            guna2DataGridView1.Columns[0].Width = 60;
            guna2DataGridView1.Columns[1].Width = 300;
            guna2DataGridView1.Columns[2].Width = 250;
            guna2DataGridView1.Columns[3].Width = 200;

            // color theme
            guna2DataGridView1.EnableHeadersVisualStyles = false;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.MediumSeaGreen;
            guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            //*****************

            guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Rows background / text
            guna2DataGridView1.DefaultCellStyle.BackColor = Color.White;
            guna2DataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew; // soft green tint

            // Selection highlight (to match the theme)
            guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
            guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // Grid & borders
            guna2DataGridView1.GridColor = Color.MediumSeaGreen;
            guna2DataGridView1.BorderStyle = BorderStyle.None;
            guna2DataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Row headers off (optional for clean look)
            guna2DataGridView1.RowHeadersVisible = false;

            // Rounded edges & smooth scroll (Guna2 bonus)
            guna2DataGridView1.ColumnHeadersHeight = 35;
            guna2DataGridView1.RowTemplate.Height = 30;
            guna2DataGridView1.BackgroundColor = Color.White;
        }

        void UpdateSearchPlaceholder()
        {
            switch (CurrentFilter)
            {
                case "PersonID":
                    guna2TextBox1.PlaceholderText = "Search by Person ID...";
                    break;
                case "Full Name":
                    guna2TextBox1.PlaceholderText = "Search by Full Name...";
                    break;
                case "Phone":
                    guna2TextBox1.PlaceholderText = "Search by Phone...";
                    break;
                case "Email":
                    guna2TextBox1.PlaceholderText = "Search by Email...";
                    break;
                default:
                    guna2TextBox1.PlaceholderText = "Search...";
                    break;
            }
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            Reloaddata();
            setupDataGridView();
            guna2ComboBox1.SelectedIndex = 0;
            UpdateSearchPlaceholder(); // تحديث placeholder عند التحميل الأولي
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentFilter = guna2ComboBox1.Text.ToString();
            UpdateSearchPlaceholder(); // تحديث placeholder عند تغيير الفلتر

            // إعادة تعيين البحث عند تغيير الفلتر
            guna2TextBox1.Text = "";
            dt.DefaultView.RowFilter = null;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "")
            {
                dt.DefaultView.RowFilter = null;
                return;
            }

            if (CurrentFilter == "PersonID")
            {
                if (int.TryParse(guna2TextBox1.Text, out int value))
                {
                    dt.DefaultView.RowFilter = $"CONVERT([{CurrentFilter}], 'System.String') LIKE '{value}%'";
                }
                else
                {
                    // إذا لم يكن الإدخال رقماً، لا تطبق أي فلتر
                    dt.DefaultView.RowFilter = null;
                }
            }
            else
            {
                dt.DefaultView.RowFilter = $"[{CurrentFilter}] like '%{guna2TextBox1.Text.Trim()}%'";
            }
        }

        private void guna2TextBox1_Validated(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CurrentFilter == "PersonID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void _DisableFormWhileRefresh()
        {
            this.Enabled = false;
            Application.DoEvents(); // Allow UI to update
            Cursor.Current = Cursors.WaitCursor;
        }

        private void _EnableFormAfterRefresh()
        {
            this.Enabled = true;
            Application.DoEvents(); // Allow UI to update
            Cursor.Current = Cursors.Default;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _DisableFormWhileRefresh();
            Reloaddata();
            _EnableFormAfterRefresh();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)guna2DataGridView1.CurrentRow.Cells[0].Value;
            frmPersonInfo frm = new frmPersonInfo(ID);
            frm.ShowDialog();
        }

        private void editPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)guna2DataGridView1.CurrentRow.Cells[0].Value;
            frmAddUpdatePerson frm = new frmAddUpdatePerson(ID);
            frm.ShowDialog();
            Reloaddata();
        }

        private void btnAddnewBook_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
        }

        // طرق مساعدة للبحث المباشر
        public void SearchByPersonID(int personID)
        {
            guna2ComboBox1.SelectedItem = "PersonID";
            guna2TextBox1.Text = personID.ToString();
        }

        public void SearchByName(string name)
        {
            guna2ComboBox1.SelectedItem = "Full Name";
            guna2TextBox1.Text = name;
        }

        public void SearchByPhone(string phone)
        {
            guna2ComboBox1.SelectedItem = "Phone";
            guna2TextBox1.Text = phone;
        }

        public void SearchByEmail(string email)
        {
            guna2ComboBox1.SelectedItem = "Email";
            guna2TextBox1.Text = email;
        }
    }
}
