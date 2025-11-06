using BusinessLogicLayer;
using Library.Global_Classes;
using Library.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library.LogIn
{
    public partial class frmLoginForm : Form
    {
        public frmLoginForm()
        {
            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string username = "";
        string password = "";
        private void frmLoginForm_Load(object sender, EventArgs e)
        {
            guna2ToggleSwitch1.CheckedState.FillColor = Color.MediumSeaGreen;
            guna2ToggleSwitch1.CheckedState.InnerColor = Color.White;
            guna2ToggleSwitch1.UncheckedState.FillColor = Color.Silver;
            guna2ToggleSwitch1.UncheckedState.InnerColor = Color.WhiteSmoke;
            guna2ToggleSwitch1.CheckedState.BorderColor = Color.MediumSeaGreen;
            guna2ToggleSwitch1.UncheckedState.BorderColor = Color.Gray;
            guna2ToggleSwitch1.Size = new Size(50, 25);
            guna2ToggleSwitch1.Cursor = Cursors.Hand;

            txtUserName.Focus();
            btnLogin.Enabled = false;

           

            if (clsLoginSettings.ReadLoginDataFromRegistry(ref username, ref password))
            {
                txtPassword.Text = password;
                txtUserName.Text = username;
                btnLogin.Enabled = true;
                guna2ToggleSwitch1.Checked = true;
                btnLogin.Focus();
            }

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            btnLogin.Enabled = !string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                frmMessage M = new frmMessage("Some Fields Not Valid", frmMessage.enMode.Error);
            }
            else
            {
                username = txtUserName.Text;
                password = txtPassword.Text;

                clsGlobal.CurrentUser = clsUser.FindByUserNameAndPassword(username, password);


                if (clsGlobal.CurrentUser == null)
                {
                    frmMessage M = new frmMessage($"Can't find user with Username: {username} and Password: {password}", frmMessage.enMode.Error);
                    M.ShowDialog();
                    clsLogger.LogMessage($"Can't find user with Username: {username} and Password: {password}", System.Diagnostics.EventLogEntryType.Error);
                    return;
                }

                if (!clsGlobal.CurrentUser.IsActive)
                {
                    frmMessage M = new frmMessage($"User {username} is inactive. Please contact the system administrator.", frmMessage.enMode.Error);
                    M.ShowDialog();
                    clsLogger.LogMessage($"Inactive user attempted login: {username}", System.Diagnostics.EventLogEntryType.Warning);
                    return;
                }

                if (guna2ToggleSwitch1.Checked)
                {
                    username = txtUserName.Text.Trim();
                    password = txtPassword.Text.Trim();
                    clsLoginSettings.UpdateLoginDataFromRegistry(username, password);
                }
                else
                {
                    clsLoginSettings.DeleteLoginDataFromRegistry();
                }

                clsLogger.LogMessage($"User: {username} Login To The System at {DateTime.Now}", System.Diagnostics.EventLogEntryType.Warning);

                this.Hide();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();

            }

        }
    }
}
