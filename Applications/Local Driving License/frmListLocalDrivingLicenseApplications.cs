using DVLD_Business;
using DVLD_System.Licenses;
using DVLD_System.Licenses.Local_License;
using DVLD_System.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private static DataTable _dtAllLocalDrivingLicensesApplications;
        private void _RefreshLocalLicensesApplicationsList()
        {
            _dtAllLocalDrivingLicensesApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();

            dgvAllLocalDrivingLicenses.DataSource = _dtAllLocalDrivingLicensesApplications;

            lblRecords.Text = _dtAllLocalDrivingLicensesApplications.Rows.Count.ToString();

        }
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicensesApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();

            dgvAllLocalDrivingLicenses.DataSource = _dtAllLocalDrivingLicensesApplications;
            lblRecords.Text = _dtAllLocalDrivingLicensesApplications.Rows.Count.ToString();
            if(dgvAllLocalDrivingLicenses.Rows.Count > 0)
            {
                dgvAllLocalDrivingLicenses.Columns[0].HeaderText = "L.D.L.AppID";
                dgvAllLocalDrivingLicenses.Columns[0].Width = 100;
                dgvAllLocalDrivingLicenses.Columns[1].HeaderText = "Driving Class";
                dgvAllLocalDrivingLicenses.Columns[1].Width = 120;
                dgvAllLocalDrivingLicenses.Columns[2].HeaderText = "National NO.";
                dgvAllLocalDrivingLicenses.Columns[2].Width = 110;
                dgvAllLocalDrivingLicenses.Columns[3].HeaderText = "Full Name";
                dgvAllLocalDrivingLicenses.Columns[3].Width = 110;
                dgvAllLocalDrivingLicenses.Columns[4].HeaderText = "Application Date";
                dgvAllLocalDrivingLicenses.Columns[4].Width = 110;
                dgvAllLocalDrivingLicenses.Columns[5].HeaderText = "Passed Tests";
                dgvAllLocalDrivingLicenses.Columns[5].Width = 100;
                dgvAllLocalDrivingLicenses.Columns[6].HeaderText = "Status";
                dgvAllLocalDrivingLicenses.Columns[6].Width = 100;


                cmbFilter.SelectedIndex = 0;



            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilteredColumn = "";
            switch(cmbFilter.Text)
            {
                case "L.D.L.AppID":
                    FilteredColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "Driving Class":
                    FilteredColumn = "ClassName";
                    break;
                case "National NO.":
                    FilteredColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilteredColumn = "FullName";
                    break;
                case "Application Date":
                    FilteredColumn = "ApplicationDate";
                    break;
                case "Passed Tests":
                    FilteredColumn = "PassedTestCount";
                    break;
                case "Status":
                    FilteredColumn = "Status";
                    break;
                default:
                    FilteredColumn = "None";
                    break;
            }

            if (txtFilter.Text.Trim() == "" || FilteredColumn == "None")
            {
                _dtAllLocalDrivingLicensesApplications.DefaultView.RowFilter = "";
                lblRecords.Text = dgvAllLocalDrivingLicenses.Rows.Count.ToString();
                return;

            }
            if (FilteredColumn == "LocalDrivingLicenseApplicationID")
            {
                //in thiscase we deal with integer not string
               _dtAllLocalDrivingLicensesApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilteredColumn, txtFilter.Text.Trim());

            }
            else
               _dtAllLocalDrivingLicensesApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilteredColumn, txtFilter.Text.Trim());
            lblRecords.Text = dgvAllLocalDrivingLicenses.Rows.Count.ToString();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApplication frm = new frmNewLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo((int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApplication frm = new frmNewLocalDrivingLicenseApplication((int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);     }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID((int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value);
            if(LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null, null);

                }
            }
            else
            {
                MessageBox.Show("Could not delete applicatoin, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID((int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value);
            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null, null);

                }
            }
            else
            {
                MessageBox.Show("Could not  cancel applicatoin, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void _ScheduleTest(clsTestTypes.enTestTypes TestType)
        {
            int LocalDrivingLicenseAppID = (int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value;
            frmListTestAppointment frm = new frmListTestAppointment(LocalDrivingLicenseAppID, TestType);
            frm.Show();
            frmListLocalDrivingLicenseApplications_Load(null, null);


        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void toolVisionTest_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestTypes.VisionTest);

        }

        private void tooWrittenTest_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestTypes.WrittenTest);

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStreetTest_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestTypes.StreetTest);

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            int TotalPassedTests= (int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[5].Value;
            bool LicenseExist = LocalDrivingLicenseApplication.IsLicenseIssued();
            //Enabled only if person passed all tests and Does not have license. 

            issueDrivingLicensFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExist;
            showApplicationDetailsToolStripMenuItem.Enabled = LicenseExist;
            editApplicationToolStripMenuItem.Enabled =
                !LicenseExist && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);
            //Enable/Disable Cancel Menue Item
            //We only canel the applications with status=new.

            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);
            //Enable/Disable Delete Menue Item
            //We only allow delete incase the application status is new not complete or Cancelled.

            deleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);

            bool PassesVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestTypes.VisionTest);
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestTypes.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestTypes.StreetTest);
            secToolStripMenuItem.Enabled =
                (!PassesVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);
            if (secToolStripMenuItem.Enabled)
            {
                toolVisionTest.Enabled = !PassesVisionTest;
                tooWrittenTest.Enabled = PassesVisionTest && !PassedWrittenTest;
                toolStreetTest.Enabled = PassesVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }

        private void contextMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void issueDrivingLicensFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value;
            frmIssueLicenseForTheFirstTime frm = new frmIssueLicenseForTheFirstTime(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value;
            int LicenseID = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID)
                .GetActiveLicense();
            if(LicenseID != -1) { 
            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();
        }
            else
            {
                MessageBox.Show("No License found!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenses.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(LocalDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();
        
                
                }
    }
}
