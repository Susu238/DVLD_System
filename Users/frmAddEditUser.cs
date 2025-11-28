using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Users
{
    public partial class frmAddEditUser : Form
    {
        //Declare a delegate .

        public delegate void DataBackEventHandler(object sender, int PersonID);
        //declare an event using delegate.
        public event DataBackEventHandler DataBack;
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        clsUser _User;
        int _UserID = -1;
        public frmAddEditUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddEditUser(int userID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _UserID = userID;
        }
        private void _RetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
                tabLogInfo.Enabled = false;
                btnSave.Enabled = false;
                ctrPersonDetailsWithFilter1.FilterFocus();
            }
            else
            {
                lblMode.Text = "Update User";
                this.Text = "Update  User";
                tabLogInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chbIsActive.Checked = true;
        }
        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            ctrPersonDetailsWithFilter1.FilterEnabled = false;
            if (_User == null)
            {
                MessageBox.Show(" No user with ID = " + _User, "User not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            lblUseID.Text = _User.userID.ToString();
            txtUserName.Text = _User.userName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chbIsActive.Checked = _User.IsActive;
            ctrPersonDetailsWithFilter1.LoadPersonInfo(_User.PersonID);

        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            _RetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            _User.PersonID = ctrPersonDetailsWithFilter1.PersonID;
            _User.userName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chbIsActive.Checked;
            if (_User.Save()) {
                lblUseID.Text = _User.userID.ToString();
                _Mode = enMode.Update;
                lblMode.Text = " Update User";
                this.Text = " Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
        private void btnNext2_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tabLogInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tabLogInfo"];
                return;

            }

            //in case of add new user.
            if (ctrPersonDetailsWithFilter1.PersonID != -1)
            {
                if (clsUser.IsExistForPersonID(ctrPersonDetailsWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected person has already a user,choose another one", "Select another person", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ctrPersonDetailsWithFilter1.FilterFocus();

                }
                else
                {
                    btnSave.Enabled = true;
                    tabLogInfo.Enabled = true;
                    tabControl1.SelectedTab = tabControl1.TabPages["tabLogInfo"];

                }

            }
            else
            {
                MessageBox.Show("Select a person   please ", "Select a  person", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void tabLogInfo_Click(object sender, EventArgs e)
        {

        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim()) {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password confirmation does not match password.");

            }

            errorProvider1.SetError(txtConfirmPassword, null);

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password can not be empty");

            }
            else
                errorProvider1.SetError(txtPassword, null);
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim())){
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username can not be empty.");
                    
            }
            else {
                errorProvider1.SetError(txtUserName, null);


            }
            if(_Mode == enMode.AddNew)
            {
                if (clsUser.IsUserExist(txtUserName.Text.Trim())) {
                    
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "Username is used by another user.");
                     }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
            }
            else
            {
                //in case update make sure usename is not used.
                if(_User.userName != txtUserName.Text.Trim())
                {
                    if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                    {

                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "Username is used by another user.");
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    }
                }
            }
        }

        private void frmAddEditUser_Activated(object sender, EventArgs e)
        {
            ctrPersonDetailsWithFilter1.FilterFocus();
        }
    }
}
 