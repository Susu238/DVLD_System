using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using System.Windows.Forms;
using DVLD_System.Global_Classes;

namespace DVLD_System
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser user = clsUser.FindByUserNameAndPassword(txtUserName.Text.Trim(),txtPassword.Text.Trim());
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (user != null)
            {
                if (chkRememberMe.Checked)
                {
                    Properties.Settings.Default.SavedUserName = userName;
                    Properties.Settings.Default.SavedPassword = password;
                    Properties.Settings.Default.RememberMe = true;

                }
                else
                    {
                    Properties.Settings.Default.SavedUserName = "";
                Properties.Settings.Default.SavedPassword = "";
                    Properties.Settings.Default.RememberMe = false;
                }


                Properties.Settings.Default.Save();
                if (!user.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your account is not active  ,Contact admin.", "Inactive account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsGlobal.CurrentUser = user;
                ////this.Hide();
                ////frmMain frm = new frmMain(this);
                ////frm.ShowDialog();
                ////    this.Close();
                this.Hide();
                frmMain frm = new frmMain(this);

                frm.Show();

            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password", "Wrong credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

         

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            chkRememberMe.Checked = Properties.Settings.Default.RememberMe;
            if (chkRememberMe.Checked)
            {
                txtUserName.Text = Properties.Settings.Default.SavedUserName;
                txtPassword.Text = Properties.Settings.Default.SavedPassword;
            }
            else{
                txtUserName.Text = "";
            txtPassword.Text = "";

                }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
/////////////////////////////////////////////////////

