using DVLD_Business;
using DVLD_System.Global_Classes;
using DVLD_System.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Detain_Licenses
{
    public partial class frmDetainLicense : Form
    {
        private int _DetainedLicenseID = -1;
        private int _SelectedLicenseID = -1;
        private clsLicenses License;
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llLicenseHistoryu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.userName;

        }

        private void ctrDrivingLicenseInfoWithFilter1_OnSelectedLicense(int obj)
        {
            _SelectedLicenseID = obj;
            lblOldLicenseID.Text = _SelectedLicenseID.ToString();
            llShowNewLicenseInfo.Enabled = (_SelectedLicenseID != -1);
            if (_SelectedLicenseID == -1)
            {
                return;
            }
            //check if license is detained
            if (ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is already detained!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;

            }
            txtFineFees.Focus();
            btnDetain.Enabled = true;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want tot detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

                return;
           int  NewLicense = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(
             float.Parse( txtFineFees.Text) , clsGlobal.CurrentUser.userID);
            if (NewLicense == -1)
            {
                MessageBox.Show("Failed to detain the license!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DetainedLicenseID = NewLicense;
            lblApplicationLicenseID.Text = _DetainedLicenseID.ToString();
            //_DetainedLicenseID = NewLicense.LicenseID;
            //lblApplicationLicenseID.Text = _DetainedLicenseID.ToString();
            MessageBox.Show("License Detained successfully with ID =" + _DetainedLicenseID.ToString(), "License Issued",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnDetain.Enabled = false;
            ctrDrivingLicenseInfoWithFilter1.filterEnabled = false;
            llLicenseHistoryu.Enabled = true;
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void frmDetainLicense_Activated(object sender, EventArgs e)
        {
            ctrDrivingLicenseInfoWithFilter1.txtFilterFocus();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                errorProvider1.SetError(txtFineFees, "Fees can not be empty");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);

            }
            if (!clsValidation.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid numder");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);

            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
