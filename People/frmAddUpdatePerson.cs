using BusinessLogicLayer;
using Guna.UI2.WinForms;
using Library.Global_Classes;
using Library.Message;
using Library.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library.People
{
    public partial class frmAddUpdatePerson : Form
    {
        enum enMode { Add = 0, Update = 1 }
        enMode Mode = enMode.Add;

        int _PersonID;
        clsPeople Person;

        public int PersonID
        {
            get { return _PersonID; }
            set { _PersonID = value; }
        }

        public frmAddUpdatePerson()
        {
            InitializeComponent();
        }

        public frmAddUpdatePerson(int ID)
        {
            InitializeComponent();
            _PersonID = ID;
            Mode = enMode.Update;
        }

        void FillCountriesBox()
        {
            DataTable dt = clsCountry.GetAllCountries();
            guna2ComboBox1.Items.Clear();

            foreach (DataRow dr in dt.Rows)
                guna2ComboBox1.Items.Add(dr["CountryName"].ToString());
        }

        private bool _HandlePersonImage()
        {

            if (guna2CirclePictureBox1.ImageLocation != null &&
                (string.IsNullOrEmpty(Person.ImageURL) || Person.ImageURL != guna2CirclePictureBox1.ImageLocation))
            {
                if (!string.IsNullOrEmpty(Person.ImageURL))
                {
                    try { File.Delete(Person.ImageURL); }
                    catch (IOException) { }
                }

                string SourceImageFile = guna2CirclePictureBox1.ImageLocation.ToString();

                if (Util.CopyImageToProjectImagesFolder(ref SourceImageFile))
                {
                    guna2CirclePictureBox1.ImageLocation = SourceImageFile;
                    return true;
                }
                else
                {
                    MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

      

        void ConfigureFormForUpdate()
        {
            Person = clsPeople.Find(PersonID);

            if (guna2ComboBox1.Items.Count == 0)
                FillCountriesBox();

            if (Person.IsMale)
            {
                guna2ComboBox2.SelectedIndex = 0;
               // guna2CirclePictureBox1.Image = Resources.man;
            }
            else
            {
                guna2ComboBox2.SelectedIndex = 1;
              //  guna2CirclePictureBox1.Image = Resources.woman;
            }



            lbtitle.Text = "Update Person";

            if (Person == null)
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtfN.Text = Person.FirstName;
            txtSN.Text = Person.SecondName;
            txtthirdname.Text = Person.ThirdName;
            txtlastname.Text = Person.LastName;
            txtGamil.Text = Person.Email;
            guna2ComboBox1.SelectedIndex = guna2ComboBox1.FindString(Person.CountryInfo.Name);
            txtPhone.Text = Person.Phone;
            txtPhone.Text = txtPhone.Text.Trim();

            if (!string.IsNullOrEmpty(Person.ImageURL))
            {
                guna2CirclePictureBox1.ImageLocation = Person.ImageURL;
                btnsetUpdateImg.Text = "Update Image";
                btndeleteImage.Visible = true;
            }

           
        }

        void ResetForm()
        {
            txtfN.Text = "";
            txtSN.Text = "";
            txtthirdname.Text = "";
            txtlastname.Text = "";
            txtGamil.Text = "";
            txtPhone.Text = "";
            guna2ComboBox1.SelectedIndex = 0;
            guna2ComboBox2.SelectedIndex = 0;
            guna2CirclePictureBox1.Image = Resources.man;
            guna2CirclePictureBox1.ImageLocation = null;
            btndeleteImage.Visible = false;
            btnsetUpdateImg.Text = "Add Image";

            Person = new clsPeople();
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            FillCountriesBox();
            if (Mode == enMode.Update)
                ConfigureFormForUpdate();
            else
            ResetForm();
        } 

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtfN_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtfN.Text))
                errorProvider1.SetError(txtfN, "First Name is required");
            else
                errorProvider1.SetError(txtfN, "");
        }

        private void txtSN_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSN.Text))
                errorProvider1.SetError(txtSN, "Second Name is required");
            else
                errorProvider1.SetError(txtSN, "");
        }

        private void txtthirdname_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtthirdname.Text))
                errorProvider1.SetError(txtthirdname, "Third Name is required");
            else
                errorProvider1.SetError(txtthirdname, "");
        }

        private void txtlastname_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtlastname.Text))
                errorProvider1.SetError(txtlastname, "Last Name is required");
            else
                errorProvider1.SetError(txtlastname, "");
        }

        private void txtGamil_Validated(object sender, EventArgs e)
        {
            string email = txtGamil.Text;
            if (string.IsNullOrWhiteSpace(email))
                errorProvider1.SetError(txtGamil, "Email is required");
            else if (!clsValidate.IsValidEmail(email))
                errorProvider1.SetError(txtGamil, "Invalid email format");
            else
                errorProvider1.SetError(txtGamil, "");
        }

    

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                frmMessage message = new frmMessage("Please fill all required fields correctly.");
                message.ShowDialog();
                return;
            }

            // Fill data
            Person.FirstName = txtfN.Text.Trim();
            Person.SecondName = txtSN.Text.Trim();
            Person.ThirdName = txtthirdname.Text.Trim();
            Person.LastName = txtlastname.Text.Trim();
            Person.Email = txtGamil.Text.Trim();
            Person.Phone = txtPhone.Text.Trim();

            if (guna2ComboBox1.SelectedItem != null)
                Person.CountryId = clsCountry.Find(guna2ComboBox1.Text.ToString()).Id;

            Person.IsMale = guna2ComboBox2.SelectedIndex == 0;

            _HandlePersonImage();

            if (guna2CirclePictureBox1.ImageLocation != "")
                Person.ImageURL = guna2CirclePictureBox1.ImageLocation.ToString();
            else
                Person.ImageURL = string.Empty;



            if (!Person.Save())
            {
                frmMessage message = new frmMessage("Failed to save person data.");
                message.ShowDialog();
            }
            else
            {
                frmMessage message = new frmMessage("Data Saved Successfully.", frmMessage.enMode.Info);
                message.ShowDialog();

                Mode = enMode.Update;
                PersonID = Person.Id;

                ConfigureFormForUpdate();
            }
        }

        private void btnsetUpdateImg_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string SelectedImgPath = openFileDialog1.FileName;
                guna2CirclePictureBox1.Load(SelectedImgPath);
                guna2CirclePictureBox1.ImageLocation = SelectedImgPath;
                btnsetUpdateImg.Text = "Update Image";
                btndeleteImage.Visible = true;
            }
        }

        private void btndeleteImage_Click(object sender, EventArgs e)
        {
            guna2CirclePictureBox1.ImageLocation = null;
            btndeleteImage.Visible = false;
            btnsetUpdateImg.Text = "Add Image";


            guna2CirclePictureBox1.Image = (guna2ComboBox2.Text != "Male") ? Resources.woman : Resources.man;


        }

        private void guna2ComboBox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2ComboBox1.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(guna2ComboBox1, "Please select a value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(guna2ComboBox1, "");
            }
        }

        private void guna2ComboBox2_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2ComboBox2.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(guna2ComboBox2, "Please select a value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(guna2ComboBox2, "");
            }
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2CirclePictureBox1.ImageLocation != null)
                return;

            if (guna2ComboBox2.Text == "Male")
                guna2CirclePictureBox1.Image = Resources.man;
            else
                guna2CirclePictureBox1.Image = Resources.woman;


        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            string phone = txtPhone.Text;
            if (string.IsNullOrWhiteSpace(phone))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone is required");
            }
            else if (!clsValidate.IsNumber(phone))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone must contain only numbers");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, "");
            }
        }
    }

}
