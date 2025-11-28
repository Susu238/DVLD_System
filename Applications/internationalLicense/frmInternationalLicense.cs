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

namespace DVLD_System.Applications.internationalLicense
{
    public partial class frmInternationalLicense : Form
    {
        private int _InternationalLicenseID = -1;
        public frmInternationalLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInternationalLicense_Load(object sender, EventArgs e)
        {
            ctrDrivingLicenseInfoWithFilter1.txtFilterFocus();
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));
            lblApplicationFees.Text = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.userName;
            llShowNewLicenseInfo.Enabled = false;
        }

        private void ctrDrivingLicenseInfoWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void ctrDrivingLicenseInfoWithFilter1_OnSelectedLicense(int obj)
        {
            
                int SelectedLicenseID = obj;
                lblLocalLicenseID.Text = SelectedLicenseID.ToString();
                llShowNewLicenseInfo.Enabled = (SelectedLicenseID != -1);
                if (SelectedLicenseID == -1)
                {
                    return;
                }

            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));
                lblLicenseFess.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ClassInfo.ClassFees.ToString();
                lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFess.Text)).ToString();

            //check if license is not class 3
            if (ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }


            //Check if person have an active international license.
            int ActiveInternationalLicenseID = clsInternationalLicense.GetActiveLicenseIDByDriverID(ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);
               if(ActiveInternationalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternationalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowNewLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                btnIssue.Enabled = false;
                return;

            }
            

            btnIssue.Enabled = true;

            }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            clsInternationalLicense InternationalLicense = new clsInternationalLicense();
            //we add base app info too
            InternationalLicense.ApplicantPersonID = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.NewInternationalLicense).ApplicationFees;
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.userID;
            // NOW WE add international license info
            InternationalLicense.DriverID = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.userID;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }
            lblAppLicenseID.Text = InternationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnIssue.Enabled = false;
            ctrDrivingLicenseInfoWithFilter1.filterEnabled = false;
            llShowNewLicenseInfo.Enabled = true;



        }

        private void frmInternationalLicense_Activated(object sender, EventArgs e)
        {
            ctrDrivingLicenseInfoWithFilter1.txtFilterFocus();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void llLicenseHistoryu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
