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
    public partial class frmPersonInfo : Form
    {
        int PersonID;
        public frmPersonInfo(int ID)
        {
            InitializeComponent();
            
            PersonID = ID;
        }

        private void ctrlPersonDetails1_Load(object sender, EventArgs e)
        {
            if (!clsPeople.Exists(PersonID))
            {
                this.Close();
                MessageBox.Show("This ID does not exist!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ctrlPersonDetails1.LoadInfo(PersonID);
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
