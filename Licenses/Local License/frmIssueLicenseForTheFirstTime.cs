using DVLD_Business;
using DVLD_System.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Licenses.Local_License
{
    public partial class frmIssueLicenseForTheFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public frmIssueLicenseForTheFirstTime(int localDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrDrivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_LocalDrivingLicenseApplicationID);
            frm.ShowDialog();

        }

        private void frmIssueLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID ="+_LocalDrivingLicenseApplicationID,"Not Allowed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            
            }
            if (!_LocalDrivingLicenseApplication.PassedAllTests())
            {
                MessageBox.Show("Person should pass all tests first.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrDrivingLicenseApplicationInfo1.LoadApplicationIfoByLocalDrivingLicenseID(_LocalDrivingLicenseApplicationID);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.userID);
        if (LicenseID!= -1)
            {
                MessageBox.Show("License is issued successfully with ID = " + LicenseID, "Succeed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                    
                    }
            else
            {
                MessageBox.Show("Failed to issue!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        
        }
    }
}
