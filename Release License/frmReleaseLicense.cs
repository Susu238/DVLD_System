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

namespace DVLD_System
{
    public partial class frmReleaseLicense : Form
    {
        private int _ReleasedLicenseID = -1;
        clsDetainedLicense DetainedLicense;
        public frmReleaseLicense()
        {
            InitializeComponent();
        }
        public frmReleaseLicense(int LicenseID)
        {
            InitializeComponent();
            _ReleasedLicenseID = LicenseID;
            ctrDrivingLicenseInfoWithFilter1.LoadInfo(LicenseID);
            ctrDrivingLicenseInfoWithFilter1.filterEnabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReleaseLicense_Load(object sender, EventArgs e)
        {
           
        }

        private void ctrDrivingLicenseInfoWithFilter1_OnSelectedLicense(int obj)
        {
            _ReleasedLicenseID = obj;
            lblOldLicenseID.Text = _ReleasedLicenseID.ToString();
            llShowNewLicenseInfo.Enabled = (_ReleasedLicenseID != -1);
            if (_ReleasedLicenseID== -1)
            {
                return;
            }
            //check if license is released
            if (!ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is not detained !", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRelease.Enabled = false;
                return;

            }
            lblApplicationFees.Text = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees.ToString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.userName;

            lblDetainedLicenseID.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();
            lblOldLicenseID.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID.ToString();

            lblCreatedByUserID.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.CreatedByUserIDInfo.userName;
            DetainDate.Text = clsFormat.DateToShort(ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainDate);
            lblLicenseFess.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFess.Text)).ToString();

            btnRelease.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want tot release this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

                return;
            int ApplicationID = -1;
           bool NewLicense = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense( clsGlobal.CurrentUser.userID, ref ApplicationID);
            lblApplicationLicenseID.Text = ApplicationID.ToString();
            if (!NewLicense)
            {
                MessageBox.Show("Failed to release the license!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("License Released successfully with ID =" + ApplicationID.ToString(), "License Issued",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRelease.Enabled = false;
            ctrDrivingLicenseInfoWithFilter1.filterEnabled = false;
            llLicenseHistoryu.Enabled = true;
        }

        private void ctrDrivingLicenseInfoWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void frmReleaseLicense_Activated(object sender, EventArgs e)
        {
            ctrDrivingLicenseInfoWithFilter1.txtFilterFocus();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_ReleasedLicenseID);
            frm.ShowDialog();
        }

        private void llLicenseHistoryu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
