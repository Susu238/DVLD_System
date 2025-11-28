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
namespace DVLD_System.Applications.Local_Driving_License
{
    public partial class ctrDrivingLicenseApplicationInfo : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int LicenseID;
        public int LocalDrivingLicenseID
        {
            get
            {
                return _LocalDrivingLicenseApplicationID;
            }
        }
       public void LoadApplicationIfoByLocalDrivingLicenseID(int localDrivingLicenseID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(localDrivingLicenseID);
            if(_LocalDrivingLicenseApplication == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No LocaL Driving License with id = " + _LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK);


            }
            _FillApplicationInfo();
        }
        public void LoadApplicationIfoByApplicationID(int ApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with id = " + _LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK);


            }
            _FillApplicationInfo();
        }
        public void ResetApplicationInfo()
        {
           _LocalDrivingLicenseApplicationID = -1;
            lblAppID.Text = "[???]";
            lblPassedTests.Text = "[???]";
            LBLLicenseClass.Text = "[???]";

        }
        private void _FillApplicationInfo()
        {
            LicenseID = _LocalDrivingLicenseApplication.GetActiveLicense();
            llLicenseInfo.Enabled = (LicenseID != -1);

            lblAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            LBLLicenseClass.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTestCount().ToString() + "/3";
            ctrBasicApplicationInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);
        }
        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplicationID = -1;
            ctrBasicApplicationInfo1.ResetApplicationInfo();
            lblAppID.Text = "[???]";
            LBLLicenseClass.Text = "[???]";
        }

        public ctrDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void gbDrivingLicenseAppInfo_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void ctrDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {

        }

        private void llLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
         frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();
        }
    }
}
