using BusinessLogicLayer;
using Library.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library.People.Controls
{
    public partial class ctrlPersonDetails : UserControl
    {
        public ctrlPersonDetails()
        {
            InitializeComponent();
        }
        int _ID;
        public int PersonID
        {
            get { return _ID; }
        }

        public void LoadInfo(int ID)
        {
            if (!clsPeople.Exists(ID))
            {
                MessageBox.Show("This ID does not exist!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _ID = ID;
            clsPeople P = clsPeople.Find(PersonID);
            if (P == null)
                return;
            lbName.Text = P.FullName;
            lbID.Text = P.Id.ToString();
            lbPhone.Text = P.Phone.ToString();
            lbEmail.Text = P.Email.ToString();
            lbCountry.Text = P.CountryInfo.Name;


            if (P.ImageURL != "")
                guna2CirclePictureBox1.ImageLocation = P.ImageURL;


            if (P.IsMale)
                guna2CirclePictureBox1.Image = Resources.man;
            else
                guna2CirclePictureBox1.Image = Resources.woman;

        }

        private void lbName_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void ctrlPersonDetails_Load(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
