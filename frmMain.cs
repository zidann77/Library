using BusinessLogicLayer;
using Library.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Library
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
          
            SetTabColors();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SetTabColors()
        {
            // Idle (الحالة العادية)
            guna2TabControl1.TabButtonIdleState.FillColor = Color.SeaGreen;
            guna2TabControl1.TabButtonIdleState.ForeColor = Color.White;

            // Hover (أخف من SeaGreen)
            guna2TabControl1.TabButtonHoverState.FillColor = Color.MediumSeaGreen;
            guna2TabControl1.TabButtonHoverState.ForeColor = Color.White;

            // Selected (أفتح قليلاً لتوضيح الاختيار)
            guna2TabControl1.TabButtonSelectedState.FillColor = Color.LightSeaGreen;
            guna2TabControl1.TabButtonSelectedState.ForeColor = Color.White;

            // إزالة الحدود
            guna2TabControl1.TabButtonIdleState.BorderColor = Color.Transparent;
            guna2TabControl1.TabButtonHoverState.BorderColor = Color.Transparent;
            guna2TabControl1.TabButtonSelectedState.BorderColor = Color.Transparent;
        }





        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTimer.Text = DateTime.Now.ToString("hh:mm tt");
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2TabControl1_Click(object sender, EventArgs e)
        {
          
        }

        private void tbSignOut_Click(object sender, EventArgs e)
        {
          //  this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmListPeople frm = new frmListPeople();
            frm.ShowDialog();
        }
    }
}
