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
using static DVLD_Business.clsLicenses;
namespace DVLD_System.Replace_For_Lost_or_Damage
{
    public partial class frmReplaceForLostOrDamage : Form
    {
        private int _ReplacedLicenseID = -1;
        public frmReplaceForLostOrDamage()
        {
            InitializeComponent();
        }
        private  string  ReplaceFees()
        {
            if(rbDamaged.Checked)
               return  clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString();
            else
              return  clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees.ToString();

           
        }
        private int _GetApplicationID()
        {
            if (rbDamaged.Checked)
                return (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplications.enApplicationType.ReplaceLostDrivingLicense;
        }
        private enIssueReason _GetIssueReason()
        {
            if (rbDamaged.Checked)
                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }
        private void frmReplaceForLostOrDamage_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.userName;
            rbDamaged.Checked = true;

        }

        private void ctrDrivingLicenseInfoWithFilter1_OnSelectedLicense(int obj)
        {
            int SelectedLicenseID = obj;
            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            llShowNewLicenseInfo.Enabled = (SelectedLicenseID != -1);
            if (SelectedLicenseID == -1)
            {
                return;
            }
            int DefaultValidityLength = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ClassInfo.DefaultValidityLength;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFess.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ClassInfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFess.Text)).ToString();
            txtNotes.Text = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;
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
            if (MessageBox.Show("Are you sure you want tot rplace this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

                return;
            clsLicenses NewLicense = ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ReplaceLicense(txtNotes.Text.Trim(), clsGlobal.CurrentUser.userID);
            if (NewLicense == null)
            {
                MessageBox.Show("Failed to replace the license!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblLicenseApplicationID.Text = NewLicense.ApplicationID.ToString();
            _ReplacedLicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
            MessageBox.Show("License  Replaced successfully with ID =" + _ReplacedLicenseID.ToString(), "License Issued",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRenew.Enabled = false;
            ctrDrivingLicenseInfoWithFilter1.filterEnabled = false;
            llLicenseHistoryu.Enabled = true;
        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {

            lblTitle.Text = "Replacement For Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.FindByApplicationTypeID(_GetApplicationID()).ApplicationFees.ToString();
        }

        private void rbLost_CheckedChanged(object sender, EventArgs e)
        {

            lblTitle.Text = "Replacement For Lost License";
            this.Text = "Replacement For Lost License";
            lblApplicationFees.Text = clsApplicationType.FindByApplicationTypeID(_GetApplicationID()).ApplicationFees.ToString();

        }

        private void llLicenseHistoryu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseInfoWithHistory frm =
                new frmShowPersonLicenseInfoWithHistory(ctrDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_ReplacedLicenseID);
        }
    }
}
    

