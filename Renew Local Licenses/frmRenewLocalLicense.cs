using DVLD_Business;
using DVLD_System.Applications.Local_Driving_License;
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

namespace DVLD_System.Renew_Local_Licenses
{
    public partial class frmRenewLocalLicense : Form
    {
        private int _NewLicenseID = -1;
        public frmRenewLocalLicense()
        {
            InitializeComponent();
        }
        
        private void frmRenewLocalLicense_Load(object sender, EventArgs e)
        {
            ctrDrivingLicenseInfoWithFilter1.txtFilterFocus();
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = "[???]";
            lblApplicationFees.Text = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.RenewDrivingLicense).ApplicationFees.ToString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.userName;
        }

        private void gbNewLicenseInfo_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblIssueDate_Click(object sender, EventArgs e)
        {

        }

        private void ctrDrivingLicenseInfoWithFilter1_OnSelectedLicense(int obj)
        {
            int SelectedLicenseID = obj;
            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            llShowNewLicenseInfo.Enabled = (SelectedLicenseID != -1);
            if(SelectedLicenseID == -1)
            {
                return;
            }
            int DefaultValidityLength = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ClassInfo.DefaultValidityLength;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFess.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ClassInfo.ClassFees.ToString();
            lblTotalFees.Text = Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFess.Text).ToString();
            txtNotes.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;
            //Check if the license is not expired.
            if (!ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is not expired yet !", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }
            //check if license is active
            if (!ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not active !", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;

            }
            btnRenew.Enabled = true;
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
         if(   MessageBox.Show("Are you sure you want tot renew this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

            return;
            clsLicenses NewLicense = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(), clsGlobal.CurrentUser.userID);
            if(NewLicense == null)
            {
                MessageBox.Show("Failed to renew the license!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblRenewLicenseID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
            MessageBox.Show("License Renewed successfully with ID =" + _NewLicenseID.ToString(), "License Issued",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRenew.Enabled = false;
            ctrDrivingLicenseInfoWithFilter1.filterEnabled = false;
            llLicenseHistoryu.Enabled = true;
        }

        private void frmRenewLocalLicense_Activated(object sender, EventArgs e)
        {
            ctrDrivingLicenseInfoWithFilter1.txtFilterFocus();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void llLicenseHistoryu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
         
        private void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}
